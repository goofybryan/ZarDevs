using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace ZarDevs.DependencyInjection.SourceGenerator;

internal class FactoryCodeGenerator : CodeGeneratorBase<BindingFactoryBuilder>
{
    public FactoryCodeGenerator(IContentPersistence contentPersistence, CancellationToken cancellation) : base(contentPersistence, cancellation)
    {
    }

    protected override TypeDefinition GenerateClassName(BindingFactoryBuilder binding, INamedTypeSymbol namedType)
    {
        var methodName = GetMethodName(binding.MethodName);
        return Code.FactoryClassName((INamedTypeSymbol)binding.Factory.Type!, methodName);
    }

    protected override string GenerateReturnWithNoParameters(BindingFactoryBuilder binding, TypeDefinition classDefinition)
    {
        var methodName = GetMethodName(binding.MethodName);
        StringBuilder builder = new StringBuilder()
            .AppendLine(Code.Ioc)
            .AppendLine(Code.Resolve(Code.FactoryVariableName, classDefinition))
            .AppendLine(Code.ReturnFactoryMethod(methodName));
        return builder.ToString();
    }

    protected override string GenerateReturnWithParameters(BindingFactoryBuilder binding, TypeDefinition classDefinition, List<string> parameterNames)
    {
        var methodName = GetMethodName(binding.MethodName);
        StringBuilder builder = new StringBuilder()
            .AppendLine(Code.Resolve(Code.FactoryVariableName, classDefinition))
            .AppendLine(Code.ReturnFactoryMethod(methodName, parameterNames));
        return builder.ToString();
    }

    protected override IMethodSymbol[] GetTargetMethodOrConstructor(BindingFactoryBuilder binding, INamedTypeSymbol namedType)
    {
        var methodName = GetMethodName(binding.MethodName);
        var typeToUse = namedType.OriginalDefinition ?? namedType;
        return typeToUse.GetMembers(methodName).Where(m => m.DeclaredAccessibility != Accessibility.Private && m.DeclaredAccessibility != Accessibility.Protected).Cast<IMethodSymbol>().ToArray();
    }

    protected override TypeInfo GetTargetType(BindingFactoryBuilder binding) => binding.Factory;

    private string GetMethodName(ArgumentSyntax argumentSyntax)
    {
        const string nameofKeyword = "nameof(";
        var methodName = argumentSyntax.ToFullString();

        if (methodName.StartsWith(Code.Quote)) return methodName.Replace(Code.Quote, string.Empty);

        return methodName.Replace(nameofKeyword, string.Empty).Split('.').Last().Replace(Code.CloseParam, string.Empty);
    }
}