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
    public static string NewLine = "\r\n";

    #endregion Fields

    #region Methods

    public static StringBuilder AppendTab(this StringBuilder builder) => builder.Append(Tab);

    public static StringBuilder AppendTab(this StringBuilder builder, int count) => builder.Append(CreateDuplicates(Tab, count));

    public static StringBuilder AppendTab(this StringBuilder builder, string content) => builder.AppendTab().Append(content.Replace(NewLine, NewLine + Tab));

    public static StringBuilder AppendTab(this StringBuilder builder, string content, int count) => builder.AppendTab(count).Append(content.Replace(NewLine, NewLine + CreateDuplicates(Tab, count)));

    public static StringBuilder AppendWhen(this StringBuilder builder, Func<bool> check, params string[] values)
    {
        if (!check()) return builder;

        foreach (var value in values)
        {
            builder.Append(value);
        }

        return builder;
    }

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

    public static Microsoft.CodeAnalysis.TypeInfo GetTypeInfo(this ArgumentSyntax argument, SemanticModel model)
    {
        var typeInfo = model.GetTypeInfo(argument);

        if (!typeInfo.IsNone())
        {
            return typeInfo;
        }

        if (argument.Expression is TypeOfExpressionSyntax typeOfExpression)
        {
            typeInfo = model.GetTypeInfo(typeOfExpression.Type);
        }

#if DEBUG
        if (typeInfo.IsNone() && System.Diagnostics.Debugger.IsAttached)
        {
            System.Diagnostics.Debugger.Break();
        }
#endif
        return typeInfo;
    }

    public static Microsoft.CodeAnalysis.TypeInfo[] GetTypeInfos(this ArgumentListSyntax argumentList, SemanticModel model)
    {
        var types = new Microsoft.CodeAnalysis.TypeInfo[argumentList.Arguments.Count];

        for (int i = 0; i < argumentList.Arguments.Count; i++)
        {
            var typeInfo = argumentList.Arguments[i].GetTypeInfo(model);

            types[i] = typeInfo;
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

    public static bool IsNone(this Microsoft.CodeAnalysis.TypeInfo typeInfo) => typeInfo.Type == null;

    public static bool IsNot(this SyntaxToken token, string value) => !token.Is(value);

    public static StringBuilder TabContent(this StringBuilder builder) => builder.AppendTab().Replace(NewLine, NewLine + Tab);

    public static StringBuilder TabContent(this StringBuilder builder, int count) => builder.AppendTab(count).Replace(NewLine, NewLine + CreateDuplicates(Tab, count));

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

    private static string CreateDuplicates(string of, int count)
    {
        string value = "";
        for (int i = 0; i < count; i++)
        {
            value += of;
        }

        return value;
    }

    #endregion Methods
}