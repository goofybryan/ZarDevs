using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Linq;
using System.Reflection;
using System.Text;

namespace ZarDevs.DependencyInjection.SourceGenerator;

internal static class GeneratorExtensions
{
    #region Fields

    public static string Tab = "\t";

    #endregion Fields

    #region Methods

    public static StringBuilder AppendTab(this StringBuilder builder) => builder.Append(Tab);
    public static StringBuilder AppendTab(this StringBuilder builder, int count) => builder.Append(CreateDuplicates(Tab, count));
    public static StringBuilder AppendTab(this StringBuilder builder, string content) => builder.AppendTab().Append(content.Replace(Environment.NewLine, Environment.NewLine + Tab));
    public static StringBuilder AppendTab(this StringBuilder builder, string content, int count) => builder.AppendTab(count).Append(content.Replace(Environment.NewLine, CreateDuplicates(Tab, count)));

    public static bool ContainsAttributeName<TAttribute>(this SyntaxList<AttributeListSyntax> attributeList) where TAttribute : Attribute
    {
        string partialName = typeof(TAttribute).Name.Replace("Attribute", "");

        return attributeList.Count > 0 && attributeList.Any(a => a.Attributes.ContainsAttributeName(partialName));
    }

    public static bool ContainsAttributeName(this SeparatedSyntaxList<AttributeSyntax> attributeList, string name)
    {
        if (attributeList.Count == 0) return false;

        foreach (var attribute in attributeList)
        {
            if (attribute.Name is IdentifierNameSyntax identifier) return identifier.Identifier.Text == name;
            if (attribute.Name is QualifiedNameSyntax qualifiedName) return qualifiedName.ToFullString().EndsWith(name);
        }

        return false;
    }

    public static Microsoft.CodeAnalysis.TypeInfo[] GetTypeInfos(this ArgumentListSyntax argumentList, SemanticModel model)
    {
        var types = new Microsoft.CodeAnalysis.TypeInfo[argumentList.Arguments.Count];

        for (int i = 0; i < argumentList.Arguments.Count; i++)
        {
            types[i] = model.GetTypeInfo(argumentList.Arguments[i]);
        }

        return types;
    }

    public static Microsoft.CodeAnalysis.TypeInfo[] GetTypeInfos(this TypeArgumentListSyntax argumentList, SemanticModel model)
    {
        var types = new Microsoft.CodeAnalysis.TypeInfo[argumentList.Arguments.Count];

        for (int i = 0; i < argumentList.Arguments.Count; i++)
        {
            types[i] = model.GetTypeInfo(argumentList.Arguments[i]);
        }

        return types;
    }

    public static bool Is(this SyntaxToken token, string value) => string.Equals(token.ValueText, value, StringComparison.Ordinal);

    public static bool IsNot(this SyntaxToken token, string value) => !token.Is(value);

    public static StringBuilder TabContent(this StringBuilder builder) => builder.AppendTab().Replace(Environment.NewLine, Environment.NewLine + Tab);

    public static StringBuilder TabContent(this StringBuilder builder, int count) => builder.AppendTab(count).Replace(Environment.NewLine, Environment.NewLine + CreateDuplicates(Tab, count));

    private static string CreateDuplicates(string of, int count)
    {
        string value = "";
        for(int i = 0; i < count; i++)
        {
            value += of;
        }

        return value;
    }

    public static bool TraverseParentForSyntaxType<TSyntax>(this SyntaxToken token, out TSyntax? syntax) where TSyntax : SyntaxNode
    {
        var current = token.Parent;

        while (current != null)
        {
            if (current is TSyntax syntaxTarget)
            {
                syntax = syntaxTarget;
                return true;
            }

            current = current.Parent;
        }

        syntax = null;
        return false;
    }

    public static bool IsNone(this Microsoft.CodeAnalysis.TypeInfo typeInfo) => typeInfo.Type == null;

    #endregion Methods
}