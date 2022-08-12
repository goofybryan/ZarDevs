using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace ZarDevs.DependencyInjection.SourceGenerator;

internal static class Code
{
    #region Fields

    public const string ClassDeclarationFormat = "internal class {0} : {1}";
    public const string CloseBrace = "}";
    public const string CloseParam = ")";
    public const string ConstructorFormat = "public {0}(ZarDevs.DependencyInjection.IDependencyInfo Info)";
    public const string ContinueStatement = "continue;";
    public const string FactoryClassNameFormat = "FactoryResolved{0}";
    public const string ForeachLoop = "foreach (var parameter in parameters)";
    public const string FunctionClass = nameof(FunctionResolution);
    public const string InfoProperty = "public ZarDevs.DependencyInjection.IDependencyInfo Info { get; }";
    public const string InfoPropertyAssign = "Info = info ?? throw new System.ArgumentNullException(nameof(info));";
    public const string InstanceClass = nameof(InstanceResolution);
    public const string Ioc = "var ioc = ZarDevs.DependencyInjection.Ioc.Container;";
    public const string NamedTypedParameterDeclarationFormat = "{0}? {1} = null;";
    public const string NamedValueTypedParameterDeclarationFormat = "{0}? {1} = null;";
    public const string NamespaceFormat = "namespace {0}";
    public const string OpenBrace = "{";
    public const string OpenParam = "(";
    public const string ParameterCastFormat = "{0} {1} = ({0})parameters[{2}];";
    public const string ParameterCheck = @"if(parameters.Length == 0) return Resolve();";
    public const string ParameterIfEqualsFormat = "if(parameter.key == \"{0}\")";
    public const string ParameterValueAssignmentFormat = "{0} = ({1})parameter.value;";
    public const string Quote = "\"";
    public const string ResolveFormat = "var {0} = ioc.TryResolve<{1}>(Key) ?? ioc.Resolve<{1}>();";
    public const string ResolveMethod = "public object Resolve()";
    public const string ResolveWithNamedParametersMethod = "public object Resolve(params (string key, object value)[] parameters)";
    public const string ResolveWithObjectParametersMethod = "public object Resolve(params object[] parameters)";
    public const string ReturnNewTypeFormat = "return new {0}();";
    public const string ReturnNewTypeWithParametersFormat = "return new {0}({1});";
    public const string TypeClassNameFormat = "TypeResolved{0}";
    public const string ValueTypeResolveFormat = "{1} {0} = default";
    public const string ReturnFactoryMethodFormat = "return factory.{0}();";
    public const string ReturnFactoryMethodWithParametersFormat = "return factory.{0}({1});";
    public const string FactoryVariableName = "factory";

    #endregion Fields

    #region Methods

    public static string ClassDeclaration(INamedTypeSymbol type) => ClassDeclaration(TypeClassName(type));
    public static string ClassDeclaration(string className) => string.Format(CultureInfo.InvariantCulture, ClassDeclarationFormat, className, typeof(ITypeResolution).FullName);

    public static string Constructor(INamedTypeSymbol type) => Constructor(TypeClassName(type));

    public static string Constructor(string className) => string.Format(CultureInfo.InvariantCulture, ConstructorFormat, className);

    public static string FactoryClassName(ITypeSymbol type) => string.Format(CultureInfo.InvariantCulture, FactoryClassNameFormat, type.Name);

    public static string NamedTypedParameterDeclaration(IParameterSymbol parameter) => string.Format(CultureInfo.InvariantCulture, parameter.Type.IsValueType ? NamedValueTypedParameterDeclarationFormat : NamedTypedParameterDeclarationFormat, parameter.Type.ToDisplayString(), parameter.Name);

    public static string ParameterCast(IParameterSymbol parameter, int index) => string.Format(CultureInfo.InvariantCulture, ParameterCastFormat, parameter.Type.ToDisplayString(), parameter.Name, index);

    public static string ParameterIfEquals(IParameterSymbol parameter) => string.Format(CultureInfo.InvariantCulture, ParameterIfEqualsFormat, parameter.Name);

    public static string ParameterValueAssignment(IParameterSymbol parameter) => string.Format(CultureInfo.InvariantCulture, ParameterValueAssignmentFormat, parameter.Name, parameter.Type.ToDisplayString());

    public static string Resolve(IParameterSymbol parameter) => Resolve(parameter.Name, parameter.Type);

    public static string Resolve(string name, ITypeSymbol type) => string.Format(CultureInfo.InvariantCulture, ResolveFormat, name, type.ToDisplayString());

    public static string ReturnNewType(ITypeSymbol type) => string.Format(CultureInfo.InvariantCulture, ReturnNewTypeFormat, type);

    public static string ReturnNewType(ITypeSymbol type, IEnumerable<string> parameterNames) => string.Format(CultureInfo.InvariantCulture, ReturnNewTypeWithParametersFormat, type, string.Join(", ", parameterNames));

    public static string ReturnFactoryMethod(string methodName) => string.Format(CultureInfo.InvariantCulture, ReturnFactoryMethodFormat, methodName);

    public static string ReturnFactoryMethod(string methodName, IEnumerable<string> parameterNames) => string.Format(CultureInfo.InvariantCulture, ReturnFactoryMethodWithParametersFormat, methodName, string.Join(", ", parameterNames));

    public static string TypeClassName(INamedTypeSymbol type) => string.Format(CultureInfo.InvariantCulture, TypeClassNameFormat, type.Name);

    public static string TypeClassName(INamedTypeSymbol type, string additional) => string.Format(CultureInfo.InvariantCulture, TypeClassNameFormat, $"{type.Name}{additional}");

    public static string ValueTypeResolve(IParameterSymbol parameter) => string.Format(CultureInfo.InvariantCulture, ValueTypeResolveFormat, parameter.Name, parameter.Type.ToDisplayString());

    internal static string Namespace(string @namespace) => string.Format(CultureInfo.InvariantCulture, NamespaceFormat, @namespace);

    internal static string ClassName(INamedTypeSymbol namedType)
    {
        if (!namedType.IsUnboundGenericType) return namedType.Name;

        namedType.
    }

    #endregion Methods
}