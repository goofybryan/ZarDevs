using Microsoft.CodeAnalysis;
using System.Collections.Generic;
using System.Globalization;

namespace ZarDevs.DependencyInjection.SourceGenerator;

internal static class Code
{
    #region Fields

    public const string ClassDeclarationFormat = "public class {0} : {1}";
    public const string CloseBrace = "}";
    public const string CloseParam = ")";
    public const string ConstraintDeclarationFormat = "where {0} : {1}";
    public const string ConstructorFormat = "public {0}(ZarDevs.DependencyInjection.IDependencyInfo info)";
    public const string ContinueStatement = "continue;";
    public const string Else = "else ";
    public const string FactoryClassNameFormat = "FactoryResolved{0}";
    public const string FactoryVariableName = "factory";
    public const string ForeachLoop = "foreach (var parameter in parameters)";
    public const string FunctionClass = nameof(FunctionResolution);
    public const string InfoProperty = "public ZarDevs.DependencyInjection.IDependencyInfo Info { get; }";
    public const string InfoPropertyAssign = "Info = info ?? throw new System.ArgumentNullException(nameof(info));";
    public const string InstanceClass = nameof(InstanceResolution);
    public const string InvalidParameterResolution = "throw new System.ArgumentException(\"There is a parameter mismatch and the correct constructor or method cannot be found\");";
    public const string Ioc = "var __ioc = ZarDevs.DependencyInjection.Ioc.Container;";
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
    public const string ResolveDefaultFormat = "{0} {1} = default";
    public const string ResolveFormat = "var {0} = ResolveParameter<{1}>();";
    public const string ResolveMethod = "public object Resolve()";
    public const string ResolveParameter = "private TZT ResolveParameter<TZT>() where TZT : class\r\n{\r\n\tvar __ioc = ZarDevs.DependencyInjection.Ioc.Container;\r\n\tif(Info.Key == null) return __ioc.Resolve<TZT>();\r\n\treturn __ioc.TryResolve<TZT>(Info.Key) ?? __ioc.Resolve<TZT>();\r\n}";
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
    public const string ValueTypeResolveFormat = "{1} {0} = default";

    #endregion Fields

    #region Methods

    public static string Constructor(string className) => string.Format(CultureInfo.InvariantCulture, ConstructorFormat, className);

    public static TypeDefinition FactoryClassName(INamedTypeSymbol type, string method) => new(type, method);

    public static string NamedTypedParameterDeclaration(IParameterSymbol parameter) => string.Format(CultureInfo.InvariantCulture, parameter.Type.IsValueType ? NamedValueTypedParameterDeclarationFormat : NamedTypedParameterDeclarationFormat, parameter.Type.ToDisplayString(), parameter.Name);

    public static string ParameterCast(IParameterSymbol parameter, int index) => string.Format(CultureInfo.InvariantCulture, ParameterCastFormat, parameter.Type.ToDisplayString(), parameter.Name, index);

    public static string ParameterCheckCount(int count) => string.Format(CultureInfo.InvariantCulture, ParameterCheckCountFormat, count);

    public static string ParameterIfEquals(IParameterSymbol parameter) => string.Format(CultureInfo.InvariantCulture, ParameterIfEqualsFormat, parameter.Name);

    public static string ParameterValueAssignment(IParameterSymbol parameter) => string.Format(CultureInfo.InvariantCulture, ParameterValueAssignmentFormat, parameter.Name, parameter.Type.ToDisplayString());

    public static string Resolve(IParameterSymbol parameter) => Resolve(parameter.Name, parameter.Type);

    public static string Resolve(string name, ITypeSymbol type) => string.Format(CultureInfo.InvariantCulture, ResolveFormat, name, type.ToDisplayString());

    public static string Resolve(string name, TypeDefinition classDefinition) => string.Format(CultureInfo.InvariantCulture, ResolveFormat, name, classDefinition.ResolveName);

    public static string ResolveDefault(string name, ITypeSymbol type) => string.Format(CultureInfo.InvariantCulture, ResolveDefaultFormat, type.ToDisplayString(), name);

    public static string ResolveWithNumberedNamedParametersMethod(int number) => string.Format(CultureInfo.InvariantCulture, ResolveWithNumberedNamedParametersMethodFormat, number);

    public static string ResolveWithNumberedObjectParametersMethod(int number) => string.Format(CultureInfo.InvariantCulture, ResolveWithNumberedObjectParametersMethodFormat, number);

    public static string ReturnFactoryMethod(string methodName) => string.Format(CultureInfo.InvariantCulture, ReturnFactoryMethodFormat, methodName);

    public static string ReturnFactoryMethod(string methodName, IEnumerable<string> parameterNames) => string.Format(CultureInfo.InvariantCulture, ReturnFactoryMethodWithParametersFormat, methodName, string.Join(", ", parameterNames));

    public static string ReturnNewType(TypeDefinition classDefinition) => string.Format(CultureInfo.InvariantCulture, ReturnNewTypeFormat, classDefinition.ResolveName);

    public static string ReturnNewType(TypeDefinition classDefinition, IEnumerable<string> parameterNames) => string.Format(CultureInfo.InvariantCulture, ReturnNewTypeWithParametersFormat, classDefinition.ResolveName, string.Join(", ", parameterNames));

    public static string ReturnResolveWithNumbered(int number) => string.Format(CultureInfo.InvariantCulture, ReturnResolveFormat, number);

    public static TypeDefinition TypeClassName(INamedTypeSymbol type) => new(type);

    public static string ValueTypeResolve(IParameterSymbol parameter) => string.Format(CultureInfo.InvariantCulture, ValueTypeResolveFormat, parameter.Name, parameter.Type.ToDisplayString());

    internal static string Namespace(string @namespace) => string.Format(CultureInfo.InvariantCulture, NamespaceFormat, @namespace);

    #endregion Methods
}