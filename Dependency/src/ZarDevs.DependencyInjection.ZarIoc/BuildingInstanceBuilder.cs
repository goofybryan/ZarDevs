using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace ZarDevs.DependencyInjection.SourceGenerator;

internal class BuildingInstanceBuilder : BindingBuilder, IResolveBinding, IIntanceBinding
{
    #region Constructors

    public BuildingInstanceBuilder(ArgumentSyntax? instance)
    {
        Instance = instance ?? throw new System.ArgumentNullException(nameof(instance));
    }

    #endregion Constructors

    #region Properties

    public ArgumentSyntax Instance { get; }

    #endregion Properties
}
