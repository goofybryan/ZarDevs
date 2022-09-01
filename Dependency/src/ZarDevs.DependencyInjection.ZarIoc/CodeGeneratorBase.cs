using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using TypeInfo = Microsoft.CodeAnalysis.TypeInfo;

namespace ZarDevs.DependencyInjection.SourceGenerator;

internal abstract class CodeGeneratorBase<T> : ITypeCodeGenerator where T : IResolveBinding
{
    #region Fields

    private readonly CancellationToken _cancellation;
    private readonly IContentPersistence _contentPersistence;
    private readonly HashSet<string> _generated;
    private readonly INamedTypeSymbol _enumerableTypeInfo;

    #endregion Fields

    #region Constructors

    protected CodeGeneratorBase(IContentPersistence contentPersistence, INamedTypeSymbol enumerableTypeInfo, CancellationToken cancellation)
    {
        _contentPersistence = contentPersistence ?? throw new System.ArgumentNullException(nameof(contentPersistence));
        _enumerableTypeInfo = enumerableTypeInfo ?? throw new ArgumentNullException(nameof(enumerableTypeInfo));
        _cancellation = cancellation;
        _generated = new(StringComparer.Ordinal);
    }

    #endregion Constructors

    #region Methods

    public bool TryGenerate(IResolveBinding binding, out string className)
    {
        if (binding is not T typeBuilder)
        {
            className = string.Empty;
            return false;
        }

        var targetType = GetTargetType(typeBuilder);
        var nameType = (INamedTypeSymbol)targetType.Type!;
        var classDefinition = GenerateClassName(typeBuilder, nameType);
        var classBuilder = new ClassBuilder(classDefinition);

        if (_generated.Contains(classDefinition.ClassName))
        {
            className = classDefinition.ClassName;
            return true;
        }

        var methodOrConstructor = GetTargetMethodOrConstructor(typeBuilder, nameType);

        Generate(typeBuilder, methodOrConstructor, classBuilder);

        className = classDefinition.ClassName;

        string[] usings = classDefinition.HasNullability ? new[] { "#nullable enable" } : Array.Empty<string>();
        _cancellation.ThrowIfCancellationRequested();
        _contentPersistence.Persist(className, classBuilder.Build(), usings);
        _generated.Add(className);

        return true;
    }

    protected abstract TypeDefinition GenerateClassName(T binding, INamedTypeSymbol namedType);

    protected abstract string GenerateReturnWithNoParameters(T binding, TypeDefinition classDefinition);

    protected abstract string GenerateReturnWithParameters(T binding, TypeDefinition classDefinition, List<string> parameterNames);

    protected abstract IMethodSymbol[] GetTargetMethodOrConstructor(T binding, INamedTypeSymbol namedType);

    protected abstract TypeInfo GetTargetType(T binding);

    private bool CheckIfTypeHasParameters(IMethodSymbol[] methodOrConstructors)
    {
        if (methodOrConstructors is null || methodOrConstructors.Length == 0)
        {
            return false;
        }

        if (methodOrConstructors.Length == 1 && methodOrConstructors[0].Parameters.Length == 0)
        {
            return false;
        }

        return true;
    }

    private void ConstructResolve(T binding, ClassBuilder classBuilder, IMethodSymbol[] methodOrConstructors)
    {
        var methodOrConstructor = methodOrConstructors.FirstOrDefault();

        string content = methodOrConstructor is null || methodOrConstructor.Parameters.Length == 0 ?
            GenerateResolveContent(binding, classBuilder.ClassDefinition) :
            GenerateResolveContent(binding, classBuilder.ClassDefinition, methodOrConstructor);

        StringBuilder builder = new StringBuilder()
            .AppendLine(Code.ResolveMethod)
            .AppendLine(Code.OpenBrace)
            .AppendTab(content)
            .AppendLine()
            .Append(Code.CloseBrace);

        classBuilder.AddMethod(builder.ToString());
    }

    private void ConstructResolveWithNamedParameters(T binding, ClassBuilder classBuilder, IMethodSymbol[] methodOrConstructors)
    {
        if (!CheckIfTypeHasParameters(methodOrConstructors))
        {
            GenerateReturnWithResolve(classBuilder, Code.ResolveWithNamedParametersMethod);
            return;
        }

        var queue = new Queue<IMethodSymbol>(methodOrConstructors.OrderByDescending(m => m.Parameters.Length));

        StringBuilder contentBuilder = new();

        while (queue.Count > 0)
        {
            var methodOrConstructor = queue.Dequeue();

            contentBuilder.AppendLine(Code.ParameterCheckCount(methodOrConstructor.Parameters.Length))
                .AppendLine(Code.OpenBrace)
                .AppendTab(GenerateResolveWithNamedParameters(binding, classBuilder, methodOrConstructor))
                .AppendLine()
                .Append(Code.CloseBrace);

            if (queue.Count > 0)
            {
                contentBuilder.AppendLine().Append(Code.Else);
            }
        }

        contentBuilder.AppendLine().Append(Code.InvalidParameterResolution);

        StringBuilder builder = new StringBuilder()
            .AppendLine(Code.ResolveWithNamedParametersMethod)
            .AppendLine(Code.OpenBrace)
            .AppendTab().AppendLine(Code.ParameterCheck)
            .AppendLine()
            .AppendTab(contentBuilder.ToString())
            .AppendLine()
            .Append(Code.CloseBrace);

        classBuilder.AddMethod(builder.ToString());
    }

    private void ConstructResolveWithParameters(T binding, ClassBuilder classBuilder, IMethodSymbol[] methodOrConstructors)
    {
        if (!CheckIfTypeHasParameters(methodOrConstructors))
        {
            GenerateReturnWithResolve(classBuilder, Code.ResolveWithObjectParametersMethod);
            return;
        }

        var queue = new Queue<IMethodSymbol>(methodOrConstructors.OrderByDescending(m => m.Parameters.Length));

        StringBuilder contentBuilder = new();

        while (queue.Count > 0)
        {
            var methodOrConstructor = queue.Dequeue();

            if (methodOrConstructor.Parameters.Length == 0) continue;

            contentBuilder.AppendLine(Code.ParameterCheckCount(methodOrConstructor.Parameters.Length))
                .AppendLine(Code.OpenBrace)
                .AppendTab(GenerateResolveParameters(binding, classBuilder, methodOrConstructor))
                .AppendLine()
                .AppendLine(Code.CloseBrace);

            if (queue.Count > 1 || (queue.Count == 1 && queue.Peek().Parameters.Length != 0))
            {
                contentBuilder.Append(Code.Else);
            }
        }

        contentBuilder.AppendLine().Append(Code.InvalidParameterResolution);

        StringBuilder builder = new StringBuilder()
            .AppendLine(Code.ResolveWithObjectParametersMethod)
            .AppendLine(Code.OpenBrace)
            .AppendTab().AppendLine(Code.ParameterCheck)
            .AppendLine()
            .AppendTab(contentBuilder.ToString())
            .AppendLine()
            .Append(Code.CloseBrace);

        classBuilder.AddMethod(builder.ToString());
    }

    private void Generate(T binding, IMethodSymbol[] methodOrConstructors, ClassBuilder classBuilder)
    {
        string className = classBuilder.ClassDefinition.ClassName;

        classBuilder.AddConstructor(new StringBuilder()
            .AppendLine(Code.Constructor(className))
            .AppendLine(Code.OpenBrace)
            .AppendTab().AppendLine(Code.InfoPropertyAssign)
            .Append(Code.CloseBrace)
            .ToString());

        classBuilder.AddProperty(Code.InfoProperty);
        ConstructResolve(binding, classBuilder, methodOrConstructors);
        ConstructResolveWithParameters(binding, classBuilder, methodOrConstructors);
        ConstructResolveWithNamedParameters(binding, classBuilder, methodOrConstructors);
        classBuilder.AddMethod(Code.ResolveParameter);
        classBuilder.AddMethod(Code.ResolveAllParameter);
    }

    private string GenerateResolveContent(T binding, TypeDefinition classDefinition) => GenerateReturnWithNoParameters(binding, classDefinition);

    private string GenerateResolveContent(T binding, TypeDefinition classDefinition, IMethodSymbol methodOrConstructor)
    {
        StringBuilder builder = new();

        List<string> parameterNames = new();
        if (methodOrConstructor is not null && methodOrConstructor.Parameters.Length > 0)
        {
            builder.AppendLine(Code.Ioc);
            foreach (var parameter in methodOrConstructor.Parameters)
            {
                parameterNames.Add(parameter.Name);

                string resolve;

                if (parameter.Type.TypeKind == TypeKind.Array)
                {
                    resolve = Code.ResolveAll(parameter.Name, ((IArrayTypeSymbol)parameter.Type).ElementType, parameter.Type);
                }
                else if (parameter.Type.AllInterfaces.Any(i => SymbolEqualityComparer.IncludeNullability.Equals(i, _enumerableTypeInfo)))
                {
                    resolve = Code.ResolveAll(parameter);
                }
                else
                {
                    resolve = parameter.Type.IsValueType ? Code.ResolveDefault(parameter.Name, parameter.Type) : Code.Resolve(parameter);
                }

                builder.AppendLine(resolve);
            }
        }

        builder.Append(GenerateReturnWithParameters(binding, classDefinition, parameterNames));

        return builder.ToString();
    }

    private string GenerateResolveParameters(T binding, ClassBuilder classBuilder, IMethodSymbol methodOrConstructor)
    {
        if (methodOrConstructor.Parameters.Length == 0)
        {
            return Code.ReturnResolve;
        }
        StringBuilder resolveMethodBuilder = new StringBuilder()
            .AppendLine(Code.ResolveWithNumberedObjectParametersMethod(methodOrConstructor.Parameters.Length))
            .AppendLine(Code.OpenBrace)
            .AppendTab(Code.Ioc)
            .AppendLine();

        List<string> parameterNames = new();
        int index = 0;
        foreach (var parameter in methodOrConstructor.Parameters)
        {
            parameterNames.Add(parameter.Name);
            resolveMethodBuilder.AppendTab().AppendLine(Code.ParameterCast(parameter, index));
            index++;
        }

        resolveMethodBuilder.AppendTab(GenerateReturnWithParameters(binding, classBuilder.ClassDefinition, parameterNames))
            .AppendLine()
            .Append(Code.CloseBrace);

        classBuilder.AddMethod(resolveMethodBuilder.ToString());

        return Code.ReturnResolveWithNumbered(methodOrConstructor.Parameters.Length);
    }

    private string GenerateResolveWithNamedParameters(T binding, ClassBuilder classBuilder, IMethodSymbol methodOrConstructor)
    {
        if (methodOrConstructor.Parameters.Length == 0)
        {
            return Code.ReturnResolve;
        }

        StringBuilder resolveMethodBuilder = new StringBuilder()
            .AppendLine(Code.ResolveWithNumberedNamedParametersMethod(methodOrConstructor.Parameters.Length))
            .AppendLine(Code.OpenBrace)
            .AppendTab(Code.Ioc)
            .AppendLine();

        StringBuilder foreachLoopBuilder = new();
        foreachLoopBuilder.AppendLine(Code.ForeachLoop)
            .AppendLine(Code.OpenBrace);

        List<string> parameterNames = new();

        foreach (var parameter in methodOrConstructor.Parameters)
        {
            parameterNames.Add(parameter.Name);
            resolveMethodBuilder.AppendTab().AppendLine(Code.NamedTypedParameterDeclaration(parameter));

            foreachLoopBuilder.AppendTab().AppendLine(Code.ParameterIfEquals(parameter))
                .AppendTab().AppendLine(Code.OpenBrace)
                .AppendTab(2).AppendLine(Code.ParameterValueAssignment(parameter))
                .AppendTab(2).AppendLine(Code.ContinueStatement)
                .AppendTab().AppendLine(Code.CloseBrace);
            foreachLoopBuilder.AppendLine();
        }

        foreachLoopBuilder.AppendLine(Code.CloseBrace);

        resolveMethodBuilder.AppendLine()
            .AppendTab(foreachLoopBuilder.ToString())
            .AppendLine()
            .AppendTab(GenerateReturnWithParameters(binding, classBuilder.ClassDefinition, parameterNames))
            .AppendLine()
            .AppendLine(Code.CloseBrace);

        classBuilder.AddMethod(resolveMethodBuilder.ToString());

        return Code.ReturnResolveWithNumbered(methodOrConstructor.Parameters.Length);
    }

    private void GenerateReturnWithResolve(ClassBuilder classBuilder, string methodDeclaration)
    {
        StringBuilder builder = new StringBuilder()
            .AppendLine(methodDeclaration)
            .AppendLine(Code.OpenBrace)
            .AppendTab(Code.ReturnResolve)
            .AppendLine()
            .AppendLine(Code.CloseBrace);

        classBuilder.AddMethod(builder.ToString());
    }

    #endregion Methods
}
