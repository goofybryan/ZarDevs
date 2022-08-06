using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Linq;
using System.Reflection;

namespace ZarDevs.DependencyInjection.SourceGenerator;

public static class RoslynExtensions
{
    #region Methods

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

    public static bool Is(this SyntaxToken token, string value) => string.Equals(token.ValueText, value, StringComparison.Ordinal);

    public static bool IsNot(this SyntaxToken token, string value) => !token.Is(value);

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

    #endregion Methods
}
