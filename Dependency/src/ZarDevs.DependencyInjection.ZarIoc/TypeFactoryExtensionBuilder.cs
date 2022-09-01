using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZarDevs.DependencyInjection.SourceGenerator;

internal class TypeFactoryExtensionBuilder
{
    private readonly IContentPersistence _contentPersistor;

    public TypeFactoryExtensionBuilder(IContentPersistence contentPersistor)
    {
        _contentPersistor = contentPersistor ?? throw new System.ArgumentNullException(nameof(contentPersistor));
    }

    public void Create(string @namespace, IEnumerable<string> typeMappers)
    {
        string className = @namespace.Replace(".", "") + "Extensions";
        string methodName = string.Concat(@namespace.Split('.').Reverse().Take(2));
        StringBuilder builder = new StringBuilder()
            .Append("public static class ").AppendLine(className)
            .AppendLine(Code.OpenBrace)
            .AppendTab().Append("public static ZarDevs.DependencyInjection.ZarIoc.IDependencyInfoToResolutionMapper Configure").Append(methodName).AppendLine("Mappers(this ZarDevs.DependencyInjection.ZarIoc.IDependencyInfoToResolutionMapper infoToMapper)")
            .AppendTab().AppendLine(Code.OpenBrace);

        foreach(string typeMapper in typeMappers)
        {
            builder.AppendTab(2).Append("infoToMapper.Add(new ").Append(@namespace).Append('.').Append(typeMapper).AppendLine("());");
        }

        builder.AppendLine()
            .AppendTab().AppendLine("return infoToMapper;")
            .AppendTab().AppendLine(Code.CloseBrace)
            .AppendLine(Code.CloseBrace);

        _contentPersistor.Persist(className, builder.ToString());
    }
}
