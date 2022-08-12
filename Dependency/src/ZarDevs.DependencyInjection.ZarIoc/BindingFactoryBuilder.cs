using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace ZarDevs.DependencyInjection.SourceGenerator;

internal class BindingFactoryBuilder : BindingBuilder, IResolveBinding, IFactoryBinding
{
    #region Constructors

    public BindingFactoryBuilder(TypeInfo? factory, ArgumentSyntax methodName)
    {
        Factory = factory ?? throw new System.ArgumentNullException(nameof(factory));
        MethodName = methodName ?? throw new System.ArgumentNullException(nameof(methodName));
    }

    #endregion Constructors

    #region Properties

    public TypeInfo Factory { get; }
    public ArgumentSyntax MethodName { get; }

    #endregion Properties
}