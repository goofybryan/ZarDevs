namespace ZarDevs.DependencyInjection.SourceGenerator;

internal class FunctionCodeGenerator : ITypeCodeGenerator
{
    public bool TryGenerate(IResolveBinding binding, out string className)
    {
        if (binding is not BindingFunctionBuilder)
        {
            className = string.Empty;
            return false;
        }

        className = Code.FunctionClass;
        return true;
    }
}