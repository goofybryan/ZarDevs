using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using ZarDevs.DependencyInjection.ZarIoc;

namespace ZarDevs.DependencyInjection.SourceGenerator;

internal static class Code
{
    #region Fields

    public const string ClassDeclarationFormat = "public class {0} : {1}";
    public const string CloseBrace = "}";
    public const string CloseParam = ")";
    public const string ConstraintDeclarationFormat = "where {0} : {1}";
    public const string ConstructorFormat = "public {0}(ZarDevs.DependencyInjection.IDependencyInfo info)";
    public const string ContainerField = "private ITypeFactoryContainter _container;";
    public const string ContainerProperty = "private ITypeFactoryContainter Container => _container ??= ((IIocContainer<ITypeFactoryContainter>)ZarDevs.DependencyInjection.Ioc.Container).Kernel;";
    public const string ContinueStatement = "continue;";
    public const string Else = "else ";
    public const string FactoryClassNameFormat = "FactoryResolved{0}";
    public const string FactoryFieldName = "private IResolution _factoryResolution;";
    public const string FactoryPropertyNameFormat = "private IResolution FactoryResolution => _factoryResolution ??= Container.Find(typeof({0}), Info.Key);";
    public const string FactoryVariableName = "factory";
    public const string ForeachLoop = "foreach (var parameter in parameters)";
    public const string FunctionClass = nameof(FunctionResolution);
    public const string InfoProperty = "public ZarDevs.DependencyInjection.IDependencyInfo Info { get; }";
    public const string InfoPropertyAssign = "Info = info ?? throw new System.ArgumentNullException(nameof(info));";
    public const string InstanceClass = nameof(InstanceResolution);
    public const string InvalidParameterResolution = "throw new System.ArgumentException(\"There is a parameter mismatch and the correct constructor or method cannot be found\");";
    public const string NamedTypedParameterDeclarationFormat = "{0} {1} = null;";
    public const string NamedValueTypedParameterDeclarationFormat = "{0} {1} = default;";
    public const string NamespaceFormat = "namespace {0}";
    public const string OpenBrace = "{";
    public const string OpenParam = "(";
    public const string ParameterCastFormat = "{0} {1} = ({0})parameters[{2}];";
    public const string ParameterCheck = @"if(parameters.Length == 0) return Resolve();";
    public const string ParameterCheckCountFormat = @"if(parameters.Length >= {0})";
    public const string ParameterIfEqualsFormat = "if(parameter.key == \"{0}\")";
    public const string ParameterValueAssignmentFormat = "{0} = ({1})parameter.value;";
    public const string Quote = "\"";
    public const string ResolveAllFormat = "var {0} = {2}Resolutions.ResolveAll().Cast<{1}>().ToArray();";
    public const string ResolveDefaultFormat = "{0} {1} = default";
    public const string ResolveFormat = "var {0} = ({1}){2}Resolution.Resolve();";
    public const string ResolveMethod = "public object Resolve()";
    public const string ResolveWithNamedParametersMethod = "public object Resolve(params (string key, object value)[] parameters)";
    public const string ResolveWithNumberedNamedParametersMethodFormat = "private object Resolve{0}((string key, object value)[] parameters)";
    public const string ResolveWithNumberedObjectParametersMethodFormat = "private object Resolve{0}(object[] parameters)";
    public const string ResolveWithObjectParametersMethod = "public object Resolve(params object[] parameters)";
    public const string ReturnFactoryMethodFormat = "return factory.{0}();";
    public const string ReturnFactoryMethodWithParametersFormat = "return factory.{0}({1});";
    public const string ReturnNewTypeFormat = "return new {0}();";
    public const string ReturnNewTypeWithParametersFormat = "return new {0}({1});";
    public const string ReturnResolve = "return Resolve();";
    public const string ReturnResolveFormat = "return Resolve{0}(parameters);";
    public const string TypeClassNameFormat = "TypeResolved{0}";
    public const string TypeResolutionFieldFormat = "private IResolution _{0}Resolution;";
    public const string TypeResolutionPropertyFormat = "private IResolution {0}Resolution => _{1}Resolution ??= Container.Find(typeof({2}), Info.Key);";
    public const string TypeResolutionsFieldFormat = "private IDependencyResolutions _{0}Resolutions;";
    public const string TypeResolutionsPropertyFormat = "private IDependencyResolutions {0}Resolutions => _{1}Resolutions ??= (IDependencyResolutions)Container.Find(typeof({2}), Info.Key);";
    public const string ValueTypeResolveFormat = "{1} {0} = default";

    #endregion Fields

    #region Methods

    public static string Constructor(string className) => string.Format(CultureInfo.InvariantCulture, ConstructorFormat, className);

    public static TypeDefinition FactoryClassName(INamedTypeSymbol type, string method) => new(type, method);

    public static string FactoryMapper(string classNameHint, Func<string> factoryMapping)
    {
        var mapperBuilder = new StringBuilder()
            .Append("public class FactoryMapper").Append(classNameHint).AppendLine(" : ZarDevs.DependencyInjection.ZarIoc.IResolutionMapper")
            .AppendLine(Code.OpenBrace)
            .AppendTab().AppendLine("public bool TryMap(ZarDevs.DependencyInjection.IDependencyInfo definition, out ZarDevs.DependencyInjection.ZarIoc.IDependencyResolution resolution)")
            .AppendTab().AppendLine(Code.OpenBrace)
            .AppendTab(2).AppendLine("resolution = null;")
            .AppendLine()
            .AppendTab(2).AppendLine("if (definition is not ZarDevs.DependencyInjection.IDependencyFactoryInfo info)")
            .AppendTab(2).AppendLine(Code.OpenBrace)
            .AppendTab(3).AppendLine("return false;")
            .AppendTab(2).AppendLine(Code.CloseBrace)
            .AppendLine()
            .AppendTab(factoryMapping(), 2).AppendLine()
            .AppendLine()
            .AppendTab().AppendLine("return false;")
            .AppendTab().AppendLine(Code.CloseBrace)
            .AppendLine(Code.CloseBrace);

        return mapperBuilder.ToString();
    }

    public static string? FactoryMapperGenericIf(this BindingFactoryBuilder builder, string className)
    {
        if (builder.Factory.Type is not INamedTypeSymbol namedType || !namedType.IsUnboundGenericType) return null;

        var mapperBuilder = new StringBuilder()
            .Append("if (info.FactoryType == typeof(").Append(builder.Factory.Type!.ToDisplayString()).Append(") && info.MethodName == ").Append(builder.MethodName).AppendLine(")")
            .AppendMapperGenericIfContent(className, namedType.TypeArguments.Length);

        return mapperBuilder.ToString();
    }

    public static string FactoryMapperIf(this BindingFactoryBuilder builder, string className)
    {
        var mapperBuilder = new StringBuilder()
            .Append("if (info.FactoryType == typeof(").Append(builder.Factory.Type!.ToDisplayString()).Append(") && info.MethodName == ").Append(builder.MethodName).AppendLine(")")
            .AppendMapperIfContent(className);

        return mapperBuilder.ToString();
    }

    public static string NamedTypedParameterDeclaration(IParameterSymbol parameter) => string.Format(CultureInfo.InvariantCulture, parameter.Type.IsValueType ? NamedValueTypedParameterDeclarationFormat : NamedTypedParameterDeclarationFormat, parameter.Type.ToDisplayString(), parameter.Name);

    public static string ParameterCast(IParameterSymbol parameter, int index) => string.Format(CultureInfo.InvariantCulture, ParameterCastFormat, parameter.Type.ToDisplayString(), parameter.Name, index);

    public static string ParameterCheckCount(int count) => string.Format(CultureInfo.InvariantCulture, ParameterCheckCountFormat, count);

    public static string ParameterIfEquals(IParameterSymbol parameter) => string.Format(CultureInfo.InvariantCulture, ParameterIfEqualsFormat, parameter.Name);

    public static string ParameterValueAssignment(IParameterSymbol parameter) => string.Format(CultureInfo.InvariantCulture, ParameterValueAssignmentFormat, parameter.Name, parameter.Type.ToDisplayString());

    public static string Resolve(IParameterSymbol parameter) => Resolve(parameter.Name, parameter.Type);

    public static string Resolve(string name, ITypeSymbol type) => string.Format(CultureInfo.InvariantCulture, ResolveFormat, name.ToCamelCase(), type.ToDisplayString(), name.ToPascalCase());

    public static string Resolve(string name, TypeDefinition classDefinition) => string.Format(CultureInfo.InvariantCulture, ResolveFormat, name.ToCamelCase(), classDefinition.ResolveName, name.ToPascalCase());

    public static string ResolveAll(string name, IArrayTypeSymbol type) => string.Format(CultureInfo.InvariantCulture, ResolveAllFormat, name, type.ElementType.ToDisplayString(), name.ToPascalCase());

    public static string ResolveAll(string name, INamedTypeSymbol type) => string.Format(CultureInfo.InvariantCulture, ResolveAllFormat, name, type.TypeArguments[0].ToDisplayString(), name.ToPascalCase());

    public static string ResolveDefault(string name, ITypeSymbol type) => string.Format(CultureInfo.InvariantCulture, ResolveDefaultFormat, type.ToDisplayString(), name);

    public static string ResolveWithNumberedNamedParametersMethod(int number) => string.Format(CultureInfo.InvariantCulture, ResolveWithNumberedNamedParametersMethodFormat, number);

    public static string ResolveWithNumberedObjectParametersMethod(int number) => string.Format(CultureInfo.InvariantCulture, ResolveWithNumberedObjectParametersMethodFormat, number);

    public static string ReturnFactoryMethod(string methodName) => string.Format(CultureInfo.InvariantCulture, ReturnFactoryMethodFormat, methodName);

    public static string ReturnFactoryMethod(string methodName, IEnumerable<string> parameterNames) => string.Format(CultureInfo.InvariantCulture, ReturnFactoryMethodWithParametersFormat, methodName, string.Join(", ", parameterNames));

    public static string ReturnNewType(TypeDefinition classDefinition) => string.Format(CultureInfo.InvariantCulture, ReturnNewTypeFormat, classDefinition.ResolveName);

    public static string ReturnNewType(TypeDefinition classDefinition, IEnumerable<string> parameterNames) => string.Format(CultureInfo.InvariantCulture, ReturnNewTypeWithParametersFormat, classDefinition.ResolveName, string.Join(", ", parameterNames));

    public static string ReturnResolveWithNumbered(int number) => string.Format(CultureInfo.InvariantCulture, ReturnResolveFormat, number);

    public static TypeDefinition TypeClassName(INamedTypeSymbol type) => new(type);

    public static string TypeMapper(string classNameHint, Func<string> typeMapping)
    {
        var mapperBuilder = new StringBuilder()
            .Append("public class TypeMapper").Append(classNameHint).AppendLine(" : ZarDevs.DependencyInjection.ZarIoc.IResolutionMapper")
            .AppendLine(Code.OpenBrace)
            .AppendTab().AppendLine("public bool TryMap(ZarDevs.DependencyInjection.IDependencyInfo definition, out ZarDevs.DependencyInjection.ZarIoc.IDependencyResolution resolution)")
            .AppendTab().AppendLine(Code.OpenBrace)
            .AppendTab(2).AppendLine("resolution = null;")
            .AppendLine()
            .AppendTab(2).AppendLine("if (definition is not ZarDevs.DependencyInjection.IDependencyTypeInfo info)")
            .AppendTab(2).AppendLine(Code.OpenBrace)
            .AppendTab(3).AppendLine("return false;")
            .AppendTab(2).AppendLine(Code.CloseBrace)
            .AppendLine()
            .AppendTab(typeMapping(), 2)
            .AppendLine()
            .AppendTab(2).AppendLine("return false;")
            .AppendTab().AppendLine(Code.CloseBrace)
            .AppendLine(Code.CloseBrace);

        return mapperBuilder.ToString();
    }

    public static string? TypeMapperGenericIf(this BindingTypeBuilder builder, string className)
    {
        if (builder.TargetType.Type is not INamedTypeSymbol namedType || !namedType.IsUnboundGenericType) return null;

        var mapperBuilder = new StringBuilder()
            .Append("if (info.ResolutionType == typeof(").Append(builder.TargetType.Type!.ToDisplayString()).AppendLine("))")
            .AppendMapperGenericIfContent(className, namedType.TypeArguments.Length);

        return mapperBuilder.ToString();
    }

    public static string TypeMapperIf(this BindingTypeBuilder builder, string className)
    {
        var mapperBuilder = new StringBuilder()
            .Append("if (info.ResolutionType == typeof(").Append(builder.TargetType.Type!.ToDisplayString()).AppendLine("))")
            .AppendMapperIfContent(className);

        return mapperBuilder.ToString();
    }

    public static string TypeResolutionField(IParameterSymbol parameter) => string.Format(CultureInfo.InvariantCulture, TypeResolutionFieldFormat, parameter.Name.ToCamelCase());

    public static string TypeResolutionProperty(IParameterSymbol parameter) => string.Format(CultureInfo.InvariantCulture, TypeResolutionPropertyFormat, parameter.Name.ToPascalCase(), parameter.Name.ToCamelCase(), parameter.Type.ToDisplayString());

    public static string TypeResolutionsField(IParameterSymbol parameter) => string.Format(CultureInfo.InvariantCulture, TypeResolutionsFieldFormat, parameter.Name.ToCamelCase());

    public static string TypeResolutionsProperty(string name, IArrayTypeSymbol type) => string.Format(CultureInfo.InvariantCulture, TypeResolutionsPropertyFormat, name.ToPascalCase(), name.ToCamelCase(), type.ElementType.ToDisplayString());

    public static string TypeResolutionsProperty(string name, INamedTypeSymbol type) => string.Format(CultureInfo.InvariantCulture, TypeResolutionsPropertyFormat, name.ToPascalCase(), name.ToCamelCase(), type.TypeArguments[0].ToDisplayString());

    public static string ValueTypeResolve(IParameterSymbol parameter) => string.Format(CultureInfo.InvariantCulture, ValueTypeResolveFormat, parameter.Name, parameter.Type.ToDisplayString());

    internal static string FactoryPropertyName(TypeDefinition classDefinition) => string.Format(CultureInfo.InvariantCulture, FactoryPropertyNameFormat, classDefinition.ResolveName);

    internal static string Namespace(string @namespace) => string.Format(CultureInfo.InvariantCulture, NamespaceFormat, @namespace);

    private static StringBuilder AppendMapperGenericIfContent(this StringBuilder builder, string className, int genericCount)
    {
        return builder
            .AppendLine(Code.OpenBrace)
            .AppendTab().Append("resolution = new ZarDevs.DependencyInjection.ZarIoc.GenericTypeResolution(ZarDevs.Runtime.Create.Instance, info, typeof(").Append(className).AppendUnboundGeneric(genericCount - 1).AppendLine("));")
            .AppendTab().AppendLine("return true;")
            .AppendLine(Code.CloseBrace);
    }

    private static StringBuilder AppendMapperIfContent(this StringBuilder builder, string className)
    {
        return builder
            .AppendLine(Code.OpenBrace)
            .AppendTab().Append("resolution = new ").Append(className).AppendLine("(info);")
            .AppendTab().AppendLine("return true;")
            .AppendLine(Code.CloseBrace);
    }

    private static StringBuilder AppendUnboundGeneric(this StringBuilder builder, int amountToAdd)
    {
        const char separator = ',';

        builder.Append('<');

        for (int i = 0; i < amountToAdd; i++)
        {
            builder.Append(separator);
        }

        builder.Append('>');

        return builder;
    }

    private static string ToCamelCase(this string value) => char.ToLowerInvariant(value[0]) + value.Substring(1);

    private static string ToPascalCase(this string value) => char.ToUpperInvariant(value[0]) + value.Substring(1);

    #endregion Methods
}