using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace ZarDevs.DependencyInjection.SourceGenerator;

internal class BindingFunctionBuilder : BindingBuilder, IResolveBinding, IFunctionBinding
{
    #region Constructors

    public BindingFunctionBuilder(InvocationExpressionSyntax? function)
    {
        Function = function ?? throw new System.ArgumentNullException(nameof(function));
    }

    #endregion Constructors

    #region Properties

    public InvocationExpressionSyntax Function { get; }

    #endregion Properties

    #region Methods

    protected override bool OnIsValid()
    {
        return Function.ArgumentList.Arguments.Count == 1;
    }

    #endregion Methods
}
