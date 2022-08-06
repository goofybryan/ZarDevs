using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace ZarDevs.DependencyInjection.SourceGenerator;

internal class BindingTypeBuilder : BindingBuilder, IResolveBinding, ITargetBinding
{
    #region Constructors

    public BindingTypeBuilder(TypeInfo targetType)
    {
        TargetType = targetType;
    }

    #endregion Constructors

    #region Properties

    public TypeInfo TargetType { get; }

    #endregion Properties

    #region Methods

    #endregion Methods
}

internal class BuildingInstanceBuilder : BindingBuilder, IResolveBinding
{
    public BuildingInstanceBuilder(ArgumentSyntax? instance)
    {
        Instance = instance ?? throw new System.ArgumentNullException(nameof(instance));
    }

    public ArgumentSyntax Instance { get; }
}

internal class BuildingFunctionBuilder : BindingBuilder, IResolveBinding
{
    public BuildingFunctionBuilder(InvocationExpressionSyntax? function)
    {
        Function = function ?? throw new System.ArgumentNullException(nameof(function));
    }

    public InvocationExpressionSyntax Function { get; }

    public override bool OnIsValid()
    {
        return Function.ArgumentList.Arguments.Count == 1;
    }
}

internal class BuildingFactoryBuilder : BindingBuilder, IResolveBinding
{
    public BuildingFactoryBuilder(TypeInfo? factory, ArgumentSyntax methodName)
    {
        Factory = factory ?? throw new System.ArgumentNullException(nameof(factory));
        MethodName = methodName ?? throw new System.ArgumentNullException(nameof(methodName));
    }

    public TypeInfo Factory { get; }
    public ArgumentSyntax MethodName { get; }
}