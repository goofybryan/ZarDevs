using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;

namespace ZarDevs.DependencyInjection.SourceGenerator;

internal class BindParser
{
    #region Fields

    private readonly CancellationToken _cancellation;
    private readonly Compilation _compilation;
    private readonly SemanticModel _model;

    #endregion Fields

    #region Constructors

    public BindParser(Compilation compilation, SemanticModel model, CancellationToken cancellation)
    {
        _compilation = compilation ?? throw new ArgumentNullException(nameof(compilation));
        _model = model ?? throw new ArgumentNullException(nameof(model));
        _cancellation = cancellation;
    }

    #endregion Constructors

    #region Methods

    public IEnumerable<IResolveBinding> ParseSyntax(MethodDeclarationSyntax methodDeclaration, SyntaxToken containerToken)
    {
        var expressionStatements = methodDeclaration.DescendantNodes().Where(x => x.IsKind(SyntaxKind.ExpressionStatement));

        foreach (var expressionStatement in expressionStatements)
        {
            var firstToken = expressionStatement.GetFirstToken();
            if (firstToken.IsNot(containerToken.ValueText))
            {
                ProcessUnknown(expressionStatement);

                break;
            }

            var next = firstToken.GetNextToken();
            if (!next.IsKind(SyntaxKind.DotToken)) break;

            next = next.GetNextToken();

            if (TryProcessToken(next, out IResolveBinding? bindingBuilder))
            {
                yield return bindingBuilder!;
            }
            else
            {
                ProcessUnknown(expressionStatement);
            }

            _cancellation.ThrowIfCancellationRequested();
        }
    }

    private IDictionary<string, SyntaxToken> BuildNextTokenMap(SyntaxToken token)
    {
        Dictionary<string, SyntaxToken> mapping = new();
        var currentToken = token.GetNextToken();
        int open = 0;
        int closed = 0;
        bool MapComplected(SyntaxKind kind) => open == closed && kind == SyntaxKind.SemicolonToken;

        do
        {
            var syntaxKind = currentToken.Kind();

            switch (syntaxKind)
            {
                case SyntaxKind.OpenBraceToken:
                    open++;
                    break;

                case SyntaxKind.CloseBraceToken:
                    closed++;
                    break;

                case SyntaxKind.IdentifierToken:
                    mapping[currentToken.ValueText] = currentToken;
                    break;
            }

            if (MapComplected(syntaxKind))
            {
                break;
            }

            currentToken = currentToken.GetNextToken();
        } while (currentToken != default);

        return mapping;
    }

    private void ProcessKey(IDictionary<string, SyntaxToken> tokenMapping, IResolveBinding bindingBuilder)
    {
        if (!tokenMapping.TryGetValue(nameof(IDependencyBuilderInfo.WithKey), out var token))
        {
            return;
        }

        bindingBuilder.KeyToken = token;
    }

    private bool ProcessResolve(IDictionary<string, SyntaxToken> tokenMapping, IResolveBinding bindingBuilder) => TryProcessResolve(tokenMapping, bindingBuilder) || TryProcessResolveAll(tokenMapping, bindingBuilder);

    private void ProcessScope(IDictionary<string, SyntaxToken> tokenMapping, IResolveBinding bindingBuilder)
    {
        if (tokenMapping.ContainsKey(nameof(IDependencyBuilderInfo.InTransientScope)))
        {
            bindingBuilder.Scope = DependyBuilderScopes.Transient;
        }
        else if (tokenMapping.ContainsKey(nameof(IDependencyBuilderInfo.InRequestScope)))
        {
            bindingBuilder.Scope = DependyBuilderScopes.Request;
        }
        else if (tokenMapping.ContainsKey(nameof(IDependencyBuilderInfo.InSingletonScope)))
        {
            bindingBuilder.Scope = DependyBuilderScopes.Singleton;
        }
        else if (tokenMapping.ContainsKey(nameof(IDependencyBuilderInfo.InThreadScope)))
        {
            bindingBuilder.Scope = DependyBuilderScopes.Thread;
        }
        else
        {
            bindingBuilder.Scope = DependyBuilderScopes.Transient;
        }
    }

    private void ProcessUnknown(SyntaxNode node)
    {
        Debug.WriteLine($"Unknown node found {node.ToFullString()}");
        // TODO: Log unknown tokens
    }

    private bool TryCreateFactoryArgumentBuilder(SyntaxToken token, out IResolveBinding? bindingBuilder)
    {
        bindingBuilder = null;

        var next = token.GetNextToken();
        if (!next.TraverseParentForSyntaxType<ArgumentListSyntax>(out var argumentList))
        {
            return false;
        }

        if (argumentList!.Arguments.Count != 2)
        {
            return false;
        }

        TypeInfo typeInfo = argumentList.Arguments[0].GetTypeInfo(_model);

        ArgumentSyntax argument = argumentList.Arguments[1];
        bindingBuilder = new BindingFactoryBuilder(typeInfo, argument);

        return true;
    }

    private bool TryCreateFactoryGenericBuilder(SyntaxToken next, out IResolveBinding? bindingBuilder)
    {
        bindingBuilder = null;

        next = next.GetNextToken();
        if (!next.TraverseParentForSyntaxType<TypeArgumentListSyntax>(out var typeArguments))
        {
            return false;
        }

        if (typeArguments == null || typeArguments.Arguments.Count != 1)
        {
            return false;
        }
        
        TypeInfo typeInfo = _model.GetTypeInfo(typeArguments.Arguments[0]);

        next = next.GetNextToken().GetNextToken().GetNextToken();

        if (!next.TraverseParentForSyntaxType<ArgumentListSyntax>(out var argumentList))
        {
            return false;
        }

        if (argumentList == null || argumentList.Arguments.Count != 1)
        {
            return false;
        }

        ArgumentSyntax argument = argumentList.Arguments[0];
        bindingBuilder = new BindingFactoryBuilder(typeInfo, argument);

        return true;
    }

    private bool TryProcessBind(SyntaxToken token, out IResolveBinding? bindingBuilder)
    {
        bindingBuilder = null;

        if (!TryProcessTypes(token, out var types))
        {
            return false;
        }

        if (types.Length == 0 || types.Length > 2)
        {
            return false;
        }

        bindingBuilder = new BindingTypeBuilder(types[0]);

        var nextMap = BuildNextTokenMap(token);

        if (types.Length == 1)
        {
            if (!ProcessResolve(nextMap, bindingBuilder))
            {
                return false;
            }
        }
        else if (types.Length == 2)
        {
            bindingBuilder.ResolveTypes.Add(types[1]);
        }

        ProcessScope(nextMap, bindingBuilder);
        ProcessKey(nextMap, bindingBuilder);

        return true;
    }

    private bool TryProcessFactory(SyntaxToken token, out IResolveBinding? bindingBuilder)
    {
        var next = token.GetNextToken();

        SyntaxKind syntaxKind = next.Kind();
        switch (syntaxKind)
        {
            default:
                bindingBuilder = null;
                return false;

            case SyntaxKind.LessThanToken:
                if (!TryCreateFactoryGenericBuilder(token, out bindingBuilder))
                {
                    return false;
                }
                break;

            case SyntaxKind.OpenParenToken:
                if (!TryCreateFactoryArgumentBuilder(token, out bindingBuilder))
                {
                    return false;
                }
                break;
        }

        var nextMap = BuildNextTokenMap(token);

        if (!ProcessResolve(nextMap, bindingBuilder!))
        {
            return false;
        }

        ProcessScope(nextMap, bindingBuilder!);
        ProcessKey(nextMap, bindingBuilder!);

        return true;
    }

    private bool TryProcessFunction(SyntaxToken token, out IResolveBinding? bindingBuilder)
    {
        if (!token.TraverseParentForSyntaxType<InvocationExpressionSyntax>(out var expressionSyntax))
        {
            bindingBuilder = null;
            return false;
        }

        bindingBuilder = new BindingFunctionBuilder(expressionSyntax);

        var nextMap = BuildNextTokenMap(token);

        if (!ProcessResolve(nextMap, bindingBuilder))
        {
            return false;
        }

        ProcessScope(nextMap, bindingBuilder);
        ProcessKey(nextMap, bindingBuilder);

        return true;
    }

    private bool TryProcessInstance(SyntaxToken token, out IResolveBinding? bindingBuilder)
    {
        var next = token.GetNextToken().GetNextToken();

        if (!next.TraverseParentForSyntaxType<ArgumentListSyntax>(out var argumentList) && argumentList!.Arguments.Count != 1)
        {
            bindingBuilder = null;
            return false;
        }

        bindingBuilder = new BuildingInstanceBuilder(argumentList?.Arguments[0]);

        var nextMap = BuildNextTokenMap(token);

        if (!ProcessResolve(nextMap, bindingBuilder))
        {
            return false;
        }

        ProcessScope(nextMap, bindingBuilder);
        ProcessKey(nextMap, bindingBuilder);

        return true;
    }

    private bool TryProcessResolve(IDictionary<string, SyntaxToken> tokenMapping, IResolveBinding bindingBuilder)
    {
        if (!tokenMapping.TryGetValue(Keywords.Resolve, out var token) || !TryProcessTypes(token, out var types))
        {
            return false;
        }

        foreach (var type in types)
        {
            bindingBuilder.ResolveTypes.Add(type);
        }

        return true;
    }

    private bool TryProcessResolveAll(IDictionary<string, SyntaxToken> tokenMapping, IResolveBinding bindingBuilder)
    {
        if (!tokenMapping.TryGetValue(Keywords.ResolveAll, out var token))
        {
            return false;
        }

        bindingBuilder.ResolveAll = true;

        if (TryProcessTypes(token, out var types))
        {
            foreach (var type in types)
            {
                bindingBuilder.IgnoreTypes.Add(type);
            }
        }

        return true;
    }

    private bool TryProcessToken(SyntaxToken token, out IResolveBinding? builder)
    {
        switch (token.ValueText)
        {
            case Keywords.Bind:
                return TryProcessBind(token, out builder);

            case Keywords.BindFunction:
                return TryProcessFunction(token, out builder);

            case Keywords.BindFactory:
                return TryProcessFactory(token, out builder);

            case Keywords.BindInstance:
                return TryProcessInstance(token, out builder);
        }

        builder = null;

        return false;
    }

    private bool TryProcessTypes(SyntaxToken token, out TypeInfo[] types)
    {
        var next = token.GetNextToken();
        if (next.IsKind(SyntaxKind.LessThanToken) && next.Parent is TypeArgumentListSyntax typeArgumentList && typeArgumentList.Arguments.Count > 0)
        {
            types = typeArgumentList.GetTypeInfos(_model);

            return true;
        }

        if (next.IsKind(SyntaxKind.OpenParenToken) && next.Parent is ArgumentListSyntax argumentList && argumentList.Arguments.Count > 0)
        {
            types = argumentList.GetTypeInfos(_model);

            return true;
        }

        types = Array.Empty<TypeInfo>();

        return false;
    }

    #endregion Methods

    #region Classes

    private static class Keywords
    {
        #region Fields

        public const string Bind = nameof(IDependencyBuilder.Bind);
        public const string BindFactory = nameof(IDependencyBuilder.BindFactory);
        public const string BindFunction = nameof(IDependencyBuilder.BindFunction);
        public const string BindInstance = nameof(IDependencyBuilder.BindInstance);
        public const string InRequestScope = nameof(IDependencyBuilderInfo.InRequestScope);
        public const string InSingletonScope = nameof(IDependencyBuilderInfo.InSingletonScope);
        public const string InThreadScope = nameof(IDependencyBuilderInfo.InThreadScope);
        public const string InTransientScope = nameof(IDependencyBuilderInfo.InTransientScope);
        public const string Resolve = nameof(IDependencyBuilderBindingResolve.Resolve);
        public const string ResolveAll = nameof(IDependencyBuilderBindingResolve.ResolveAll);
        public const string WithKey = nameof(IDependencyBuilderInfo.WithKey);

        #endregion Fields
    }

    #endregion Classes
}