using Microsoft.CodeAnalysis;

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

    protected override bool OnIsValid()
    {
        return TargetType.Type is INamedTypeSymbol;
    }

    #endregion Methods
}
