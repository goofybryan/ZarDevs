using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace ZarDevs.DependencyInjection.SourceGenerator;

internal interface IFunctionBinding
{
    #region Properties

    InvocationExpressionSyntax Function { get; }

    #endregion Properties
}
