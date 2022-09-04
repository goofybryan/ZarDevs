using System;
using System.Collections.Generic;
using System.Text;

namespace ZarDevs.DependencyInjection.SourceGenerator;

internal class ClassBuilder
{
    #region Fields

    private readonly ISet<string> _constructors;
    private readonly ISet<string> _fields;
    private readonly ISet<string> _methods;
    private readonly ISet<string> _properties;

    #endregion Fields

    #region Constructors

    public ClassBuilder(TypeDefinition classDefinition)
    {
        _fields = new HashSet<string>();
        _constructors = new HashSet<string>();
        _methods = new HashSet<string>();
        _properties = new HashSet<string>();
        ClassDefinition = classDefinition ?? throw new ArgumentNullException(nameof(classDefinition));
        Usings = new List<string>
        {
            "using System.Linq;",
            "using ZarDevs.DependencyInjection.ZarIoc;"
        };
    }

    #endregion Constructors

    #region Properties

    public TypeDefinition ClassDefinition { get; }
    public IList<string> Usings { get; }

    #endregion Properties

    #region Methods

    public void AddConstructor(string content)
    {
        if (string.IsNullOrWhiteSpace(content))
        {
            throw new ArgumentException($"'{nameof(content)}' cannot be null or whitespace.", nameof(content));
        }

        _constructors.Add(content.Trim());
    }

    public void AddFields(string content)
    {
        if (string.IsNullOrWhiteSpace(content))
        {
            throw new ArgumentException($"'{nameof(content)}' cannot be null or whitespace.", nameof(content));
        }

        _fields.Add(content.Trim());
    }

    public void AddMethod(string content)
    {
        if (string.IsNullOrWhiteSpace(content))
        {
            throw new ArgumentException($"'{nameof(content)}' cannot be null or whitespace.", nameof(content));
        }

        _methods.Add(content.Trim());
    }

    public void AddProperty(string content)
    {
        if (string.IsNullOrWhiteSpace(content))
        {
            throw new ArgumentException($"'{nameof(content)}' cannot be null or whitespace.", nameof(content));
        }

        _properties.Add(content.Trim());
    }

    public void AddUsings(string content)
    {
        if (string.IsNullOrWhiteSpace(content))
        {
            throw new ArgumentException($"'{nameof(content)}' cannot be null or whitespace.", nameof(content));
        }

        Usings.Add(content.Trim());
    }

    public string Build()
    {
        StringBuilder builder = new();

        builder.AppendLine(ClassDefinition.Declaration)
            .AppendLine(Code.OpenBrace);

        AppendTabbedSection(_fields, builder);
        builder.AppendLine();
        AppendTabbedSection(_constructors, builder);
        builder.AppendLine();
        AppendTabbedSection(_properties, builder);
        builder.AppendLine();
        AppendTabbedSection(_methods, builder);

        builder.Append(Code.CloseBrace);

        return builder.ToString();
    }

    public override string ToString()
    {
        return Build();
    }

    private void AppendTabbedSection(ISet<string> toAppend, StringBuilder builder)
    {
        foreach (var section in toAppend)
        {
            builder.AppendLine().AppendTab(section).AppendLine();
        }
    }

    #endregion Methods
}