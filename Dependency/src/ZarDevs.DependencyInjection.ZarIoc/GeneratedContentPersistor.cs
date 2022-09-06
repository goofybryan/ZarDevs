using Microsoft.CodeAnalysis;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZarDevs.DependencyInjection.SourceGenerator;

internal class GeneratedContentPersistor : IContentPersistence
{
    private readonly SourceProductionContext _context;
    private readonly string _namespace;
    private readonly HashSet<string> _fileNames;

    public GeneratedContentPersistor(SourceProductionContext context, string @namespace)
    {
        if (string.IsNullOrWhiteSpace(@namespace))
        {
            throw new System.ArgumentException($"'{nameof(@namespace)}' cannot be null or whitespace.", nameof(@namespace));
        }

        _context = context;
        _namespace = @namespace;
        _fileNames = new ();
    }

    public void Persist(string className, string content, params string[] usings)
    {
        StringBuilder fileContentBuilder = new();

        AddUsings(usings, fileContentBuilder);

        fileContentBuilder.AppendLine(Code.Namespace(_namespace))
            .AppendLine(Code.OpenBrace)
            .AppendTab(content).AppendLine()
            .AppendLine(Code.CloseBrace);

        string fileName = CompressClassName(className);
        _context.AddSource($"{fileName}.g", fileContentBuilder.ToString());
    }

    private string CompressClassName(string className)
    {
        string fileName = new string(className.Where(c => char.IsUpper(c) || char.IsNumber(c)).ToArray());

        return AddToCache(fileName);
    }

    private string AddToCache(string fileName)
    {
        if (!_fileNames.Contains(fileName))
        {
            _fileNames.Add(fileName);
            return fileName;
        }


        int index = 0;
        string name;
        do
        {
            index++;
            name = fileName + index;
        }
        while (_fileNames.Contains(name));

        _fileNames.Add(name);
        return name;

    }

    private void AddUsings(string[] usings, StringBuilder fileContentBuilder)
    {
        if(usings.Length == 0) return;

        foreach (var @using in usings)
        {
            fileContentBuilder.AppendLine(@using);
        }

        fileContentBuilder.AppendLine();
    }
}