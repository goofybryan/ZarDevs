namespace ZarDevs.DependencyInjection.SourceGenerator;

internal interface ITargetBinding
{
    #region Properties

    Microsoft.CodeAnalysis.TypeInfo TargetType { get; }

    #endregion Properties
}