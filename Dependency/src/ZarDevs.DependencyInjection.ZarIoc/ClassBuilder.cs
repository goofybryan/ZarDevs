using System;
using System.Collections.Generic;
using System.Text;

namespace ZarDevs.DependencyInjection.SourceGenerator;

internal class ClassBuilder
{
    #region Fields

    private readonly IList<string> _constructors;
    private readonly IList<string> _methods;
    private readonly IList<string> _properties;

    #endregion Fields

    #region Constructors

    public ClassBuilder(TypeDefinition classDefinition)
    {
        _constructors = new List<string>();
        _methods = new List<string>();
        _properties = new List<string>();
        ClassDefinition = classDefinition ?? throw new ArgumentNullException(nameof(classDefinition));
        Usings = new List<string>();
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

    private void AppendTabbedSection(IList<string> toAppend, StringBuilder builder)
    {
        foreach (var section in toAppend)
        {
            builder.AppendLine().AppendTab(section).AppendLine();
        }
    }

    #endregion Methods
}