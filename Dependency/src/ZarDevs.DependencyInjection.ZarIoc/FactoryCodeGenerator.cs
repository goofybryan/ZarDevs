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

    protected override string GenerateClassName(BindingFactoryBuilder binding, INamedTypeSymbol namedType)
    {
        var methodName = GetMethodName(binding.MethodName);
        return Code.TypeClassName(binding.Factory.Type!, methodName);
    }

    protected override string GenerateReturnWithNoParameters(BindingFactoryBuilder binding, INamedTypeSymbol namedType)
    {
        var methodName = GetMethodName(binding.MethodName);
        StringBuilder builder = new StringBuilder(Code.Resolve(Code.FactoryVariableName, namedType))
            .AppendLine(Code.ReturnFactoryMethod(methodName));
        return builder.ToString();
    }

    protected override string GenerateReturnWithParameters(BindingFactoryBuilder binding, INamedTypeSymbol namedType, List<string> parameterNames)
    {
        var methodName = GetMethodName(binding.MethodName);
        StringBuilder builder = new StringBuilder(Code.Resolve(Code.FactoryVariableName, namedType))
            .AppendLine(Code.ReturnFactoryMethod(methodName, parameterNames));
        return builder.ToString();
    }

    protected override IMethodSymbol GetTargetMethodOrConstructor(BindingFactoryBuilder binding, INamedTypeSymbol namedType)
    {
        var methodName = GetMethodName(binding.MethodName);
        return namedType.GetMembers(methodName).OfType<IMethodSymbol>().OrderByDescending(m => m.Parameters.Length).FirstOrDefault();
    }

    protected override TypeInfo GetTargetType(BindingFactoryBuilder binding) => binding.Factory;

    private string GetMethodName(ArgumentSyntax argumentSyntax)
    {
        const string nameofKeyword = "nameof(";
        var methodName = argumentSyntax.ToFullString();

        if (methodName.StartsWith(Code.Quote)) return methodName.Replace(Code.Quote, string.Empty);

        return methodName.Replace(nameofKeyword, string.Empty).Replace(Code.CloseParam, string.Empty);
    }
}