using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZarDevs.DependencyInjection.ZarIoc;

namespace ZarDevs.DependencyInjection.SourceGenerator;

internal class TypeDefinition
{
    #region Fields

    private const string _constraintSeparator = ", ";
    private readonly string _additional;
    private readonly INamedTypeSymbol _namedType;
    private string? _className;
    private IList<(ITypeParameterSymbol parameter, string constraints)>? _constraints;
    private string? _declaration;
    private string? _genericArguments;
    private string? _resolveName;

    #endregion Fields

    #region Constructors

    public TypeDefinition(INamedTypeSymbol namedType) : this(namedType, string.Empty)
    {
    }

    public TypeDefinition(INamedTypeSymbol namedType, string additional)
    {
        _namedType = namedType ?? throw new ArgumentNullException(nameof(namedType));
        _additional = additional ?? throw new ArgumentNullException(nameof(additional));
        HasNullability = _namedType.TypeArgumentNullableAnnotations.Any(t => t == NullableAnnotation.Annotated);
    }

    #endregion Constructors

    #region Properties

    public string ClassName => _className ??= GenerateClassName();
    public IList<(ITypeParameterSymbol parameter, string constraints)> Constraints => _constraints ??= GetClassConstraints().ToArray();
    public string Declaration => _declaration ??= GenerateDeclaration();
    public string GenericArguments => _genericArguments ??= ConstructGenericArguments();
    public bool HasNullability { get; set; }
    public string ResolveName => _resolveName ??= GenerateResolveName();
    public INamedTypeSymbol Type => _namedType;

    #endregion Properties

    #region Methods

    public static bool IsClassConstraint(ITypeParameterSymbol parameter) => parameter.HasReferenceTypeConstraint
        && parameter.ReferenceTypeConstraintNullableAnnotation != NullableAnnotation.Annotated
        && !parameter.HasValueTypeConstraint;

    public static bool IsDefaultConstraint(ITypeParameterSymbol parameter) => parameter.HasReferenceTypeConstraint
        && parameter.HasValueTypeConstraint;

    public static bool IsStructConstraint(ITypeParameterSymbol parameter) => !parameter.HasReferenceTypeConstraint
        && parameter.HasValueTypeConstraint;

    public string ConstructGenericArguments()
    {
        if (Constraints.Count == 0) return string.Empty;

        StringBuilder builder = new StringBuilder().Append('<');
        foreach (var (parameter, _) in Constraints)
        {
            builder.Append(parameter.Name).Append(_constraintSeparator);
        }

        builder.Length -= _constraintSeparator.Length;

        return builder.Append('>').ToString();
    }

    public string GenerateClassName()
    {
        string name = _namedType.Name;
        if (!_namedType.IsGenericType || !_namedType.IsUnboundGenericType) return name + _additional + "Factory";

        var typesPart = _namedType.IsUnboundGenericType ?
            _namedType.TypeParameters.SelectMany(p => p.ToDisplayString()) :
            _namedType.TypeArguments.SelectMany(p => "A" + p.ToDisplayString());

        return name + string.Join("", typesPart) + _additional + "Factory";
    }

    public string GenerateDeclaration()
    {
        StringBuilder builder = new StringBuilder().Append("public class ").Append(ClassName);//.AppendFormat(Code.ClassDeclarationFormat, ClassName, typeof(ITypeResolution).Name);
        StringBuilder constraints = new();

        if (Constraints.Count > 0)
        {
            builder.Append('<');

            foreach (var constraint in Constraints)
            {
                builder.Append(constraint.parameter.ToDisplayString()).Append(_constraintSeparator);
                if (!string.IsNullOrWhiteSpace(constraint.constraints))
                {
                    constraints.AppendLine().AppendTab().AppendFormat(Code.ConstraintDeclarationFormat, constraint.parameter.ToDisplayString(), constraint.constraints);
                }
            }

            builder.Length -= _constraintSeparator.Length;
            builder.Append('>');
        }

        builder.Append(" : ").Append(typeof(ITypeResolution).FullName).Append(constraints);

        return builder.ToString();
    }

    private string GenerateConstraint(ITypeParameterSymbol parameter)
    {
        const string unmanaged = nameof(unmanaged);
        const string notnull = nameof(notnull);
        const string @class = nameof(@class);
        const string @default = nameof(@default);
        const string @new = "new()";
        const string @struct = nameof(@struct);

        if (parameter.ConstraintTypes.Length == 0) return string.Empty;

        StringBuilder builder = new();

        UpdateNullability(parameter.NullableAnnotation);

        foreach (var constraint in parameter.ConstraintTypes)
        {
            UpdateNullability(constraint.NullableAnnotation);
            builder.Append(constraint.ToDisplayString());

            builder.Append(_constraintSeparator);
        }

        builder.AppendWhen(() => parameter.HasNotNullConstraint, notnull, _constraintSeparator);
        builder.AppendWhen(() => parameter.HasUnmanagedTypeConstraint, unmanaged, _constraintSeparator);
        builder.AppendWhen(() => IsDefaultConstraint(parameter), @default, _constraintSeparator);
        builder.AppendWhen(() => IsStructConstraint(parameter), @struct, _constraintSeparator);

        if (IsClassConstraint(parameter))
        {
            builder.Append(@class).Append(_constraintSeparator);
            if (parameter.ReferenceTypeConstraintNullableAnnotation == NullableAnnotation.Annotated)
            {
                HasNullability = true;
                builder.Append('?');
            }
        }

        builder.AppendWhen(() => parameter.HasConstructorConstraint, @new, _constraintSeparator);

        builder.Length -= _constraintSeparator.Length;
        return builder.ToString();
    }

    private string GenerateResolveName()
    {
        if (Constraints.Count == 0) return _namedType.ToDisplayString();

        string name = _namedType.ToDisplayString().Substring(0, _namedType.ToDisplayString().IndexOf('<'));

        return name + GenericArguments;
    }

    private IEnumerable<(ITypeParameterSymbol parameter, string constraints)> GetClassConstraints()
    {
        if (!_namedType.IsGenericType || !_namedType.IsUnboundGenericType)
        {
            yield break;
        }

        foreach (var typeParameter in _namedType.TypeParameters)
        {
            string constraints = typeParameter.ConstraintTypes.Length == 0 ? string.Empty : GenerateConstraint(typeParameter);

            yield return new(typeParameter, constraints);
        }
    }

    private void UpdateNullability(NullableAnnotation annotation)
    {
        if (annotation == NullableAnnotation.Annotated)
        {
            HasNullability = true;
        }
    }

    #endregion Methods
}