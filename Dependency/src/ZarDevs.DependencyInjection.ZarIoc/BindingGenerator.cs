using System.Text;

namespace ZarDevs.DependencyInjection.SourceGenerator;

internal class BindingGenerator
{
    public BindingGenerator()
    {
        FactoryMappings = new StringBuilder();
        TypeMappings = new StringBuilder();
    }

    public StringBuilder FactoryMappings { get; }
    public StringBuilder TypeMappings { get; }

    public bool TryGenerate(IResolveBinding binding, string className)
    {
        switch(binding)
        {
            case BindingTypeBuilder typeBuilder:
                GenerateType(typeBuilder, className);
                return true;
            case BindingFactoryBuilder factoryBuilder:
                GenerateFactory(factoryBuilder, className);
                return true;
        }

        return false;
    }

    private void GenerateType(BindingTypeBuilder typeBuilder, string className)
    {
        string ifStateMent = Code.TypeMapperGenericIf(typeBuilder, className) ?? Code.TypeMapperIf(typeBuilder, className);
        TypeMappings.Append(ifStateMent).AppendLine();
    }

    private void GenerateFactory(BindingFactoryBuilder factoryBuilder, string className)
    {
        string ifStateMent = Code.FactoryMapperGenericIf(factoryBuilder, className) ?? Code.FactoryMapperIf(factoryBuilder, className);
        FactoryMappings.Append(ifStateMent).AppendLine();
    }
}