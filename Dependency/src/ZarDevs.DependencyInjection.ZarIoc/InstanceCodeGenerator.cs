namespace ZarDevs.DependencyInjection.SourceGenerator;

internal class InstanceCodeGenerator : ITypeCodeGenerator
{
    public bool TryGenerate(IResolveBinding binding, out string className)
    {
        if (binding is not BuildingInstanceBuilder)
        {
            className = string.Empty;
            return false;
        }

        className = Code.InstanceClass;
        return true;
    }
}
