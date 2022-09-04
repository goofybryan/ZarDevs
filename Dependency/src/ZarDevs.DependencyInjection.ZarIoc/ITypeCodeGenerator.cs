namespace ZarDevs.DependencyInjection.SourceGenerator;

internal interface ITypeCodeGenerator
{
    #region Methods

    bool TryGenerate(IResolveBinding binding, out string className);

    #endregion Methods
}