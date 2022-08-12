using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace ZarDevs.DependencyInjection.SourceGenerator;

internal abstract class CodeGeneratorBase<T> : ITypeCodeGenerator where T : IResolveBinding
{
    #region Fields

    private readonly CancellationToken _cancellation;
    private readonly IContentPersistence _contentPersistence;
    private readonly Dictionary<ITypeSymbol, string> _generated;

    #endregion Fields

    #region Constructors

    protected CodeGeneratorBase(IContentPersistence contentPersistence, CancellationToken cancellation)
    {
        _contentPersistence = contentPersistence ?? throw new System.ArgumentNullException(nameof(contentPersistence));
        _cancellation = cancellation;
        _generated = new(SymbolEqualityComparer.Default);
    }

    #endregion Constructors

    #region Methods

    public bool TryGenerate(IResolveBinding binding, out string className)
    {
        if (binding is not T typeBuilder)
        {
            className = string.Empty;
            return false;
        }

        var targetType = GetTargetType(typeBuilder);
        var nameType = (INamedTypeSymbol)targetType.Type!;

        if (_generated.TryGetValue(nameType, out className))
        {
            return true;
        }

        className = GenerateClassName(typeBuilder, nameType);

        var methodOrConstructor = GetTargetMethodOrConstructor(typeBuilder, nameType);

        var content = Generate(typeBuilder, nameType, methodOrConstructor);

        _cancellation.ThrowIfCancellationRequested();
        _contentPersistence.Persist(className, content);
        _generated.Add(nameType, className);

        return true;
    }

    protected abstract TypeInfo GetTargetType(T binding);
    protected abstract IMethodSymbol GetTargetMethodOrConstructor(T binding, INamedTypeSymbol namedType);
    protected abstract string GenerateClassName(T binding, INamedTypeSymbol namedType);
    protected abstract string GenerateReturnWithNoParameters(T binding, INamedTypeSymbol namedType);
    protected abstract string GenerateReturnWithParameters(T binding, INamedTypeSymbol namedType, List<string> parameterNames);

    private string ConstructResolve(T binding, INamedTypeSymbol typeSymbol, IMethodSymbol methodOrConstructor)
    {
        if (methodOrConstructor is null || methodOrConstructor.Parameters.Length == 0)
        {
            return GenerateReturnWithNoParameters(binding, typeSymbol);
        }

        StringBuilder builder = new();

        List<string> parameterNames = new();
        if (methodOrConstructor is not null && methodOrConstructor.Parameters.Length > 0)
        {
            builder.AppendLine(Code.Ioc);
            foreach (var parameter in methodOrConstructor.Parameters)
            {
                parameterNames.Add(parameter.Name);
                builder.AppendLine(Code.Resolve(parameter));
            }
        }

        builder.Append(GenerateReturnWithParameters(binding, typeSymbol, parameterNames));

        return builder.ToString();
    }

    private string ConstructResolveWithNamedParameters(T binding, INamedTypeSymbol typeSymbol, IMethodSymbol methodOrConstructor)
    {
        if (methodOrConstructor is null || methodOrConstructor.Parameters.Length == 0)
        {
            return GenerateReturnWithNoParameters(binding, typeSymbol);
        }

        StringBuilder builder = new();
        builder.AppendLine(Code.ParameterCheck);

        StringBuilder foreachLoopBuilder = new();
        foreachLoopBuilder.AppendLine(Code.ForeachLoop)
            .AppendLine(Code.OpenBrace);

        List<string> parameterNames = new();

        foreach (var parameter in methodOrConstructor.Parameters)
        {
            parameterNames.Add(parameter.Name);
            builder.AppendLine(Code.NamedTypedParameterDeclaration(parameter));

            foreachLoopBuilder.AppendTab().AppendLine(Code.ParameterIfEquals(parameter))
                .AppendTab().AppendLine(Code.OpenBrace)
                .AppendTab(2).AppendLine(Code.ParameterValueAssignment(parameter))
                .AppendTab(2).AppendLine(Code.ContinueStatement)
                .AppendTab().AppendLine(Code.CloseBrace);
            foreachLoopBuilder.AppendLine();
        }

        foreachLoopBuilder.AppendLine(Code.CloseBrace);

        builder.AppendLine(foreachLoopBuilder.ToString());

        builder.Append(GenerateReturnWithParameters(binding, typeSymbol, parameterNames));

        return builder.ToString();
    }

    private string ConstructResolveWithParameters(T binding, INamedTypeSymbol typeSymbol, IMethodSymbol methodOrConstructor)
    {
        if (methodOrConstructor is null || methodOrConstructor.Parameters.Length == 0)
        {
            return GenerateReturnWithNoParameters(binding, typeSymbol);
        }

        StringBuilder builder = new();
        builder.AppendLine(Code.ParameterCheck);

        List<string> parameterNames = new();
        int index = 0;
        foreach (var parameter in methodOrConstructor.Parameters)
        {
            parameterNames.Add(parameter.Name);
            builder.AppendLine(Code.ParameterCast(parameter, index));
            index++;
        }

        builder.Append(GenerateReturnWithParameters(binding, typeSymbol, parameterNames));

        return builder.ToString();
    }

    private string Generate(T binding, INamedTypeSymbol typeSymbol, IMethodSymbol constructor)
    {
        string resolve = ConstructResolve(binding, typeSymbol, constructor);
        string resolveObjectParameters = ConstructResolveWithParameters(binding, typeSymbol, constructor);
        string resolveNamedParameters = ConstructResolveWithNamedParameters(binding, typeSymbol, constructor);

        var classBuilder = new StringBuilder()
            .AppendLine(Code.ClassDeclaration(typeSymbol))
            .AppendLine(Code.OpenBrace)
            .AppendTab().AppendLine(Code.Constructor(typeSymbol))
            .AppendTab().AppendLine(Code.OpenBrace)
            .AppendTab(2).AppendLine(Code.InfoPropertyAssign)
            .AppendTab().AppendLine(Code.CloseBrace)
            .AppendLine()
            .AppendTab().AppendLine(Code.InfoProperty)
            .AppendLine()
            .AppendTab().AppendLine(Code.ResolveMethod)
            .AppendTab().AppendLine(Code.OpenBrace)
            .AppendTab(resolve, 2).AppendLine()
            .AppendTab().AppendLine(Code.CloseBrace)
            .AppendLine()
            .AppendTab().AppendLine(Code.ResolveWithObjectParametersMethod)
            .AppendTab().AppendLine(Code.OpenBrace)
            .AppendTab(resolveObjectParameters, 2).AppendLine()
            .AppendTab().AppendLine(Code.CloseBrace)
            .AppendLine()
            .AppendTab().AppendLine(Code.ResolveWithNamedParametersMethod)
            .AppendTab().AppendLine(Code.OpenBrace)
            .AppendTab(resolveNamedParameters, 2).AppendLine()
            .AppendTab().AppendLine(Code.CloseBrace)
            .AppendLine(Code.CloseBrace);

        string content = classBuilder.ToString();

        return content;
    }

    #endregion Methods
}

internal class TypeCodeGenerator : CodeGeneratorBase<BindingTypeBuilder>
{
    public TypeCodeGenerator(IContentPersistence contentPersistence, CancellationToken cancellation) : base(contentPersistence, cancellation)
    {
    }

    protected override string GenerateClassName(BindingTypeBuilder binding, INamedTypeSymbol namedType) => Code.TypeClassName(namedType);

    protected override string GenerateReturnWithNoParameters(BindingTypeBuilder binding, INamedTypeSymbol namedType) => Code.ReturnNewType(namedType);

    protected override string GenerateReturnWithParameters(BindingTypeBuilder binding, INamedTypeSymbol namedType, List<string> parameterNames) => Code.ReturnNewType(namedType, parameterNames);

    protected override IMethodSymbol GetTargetMethodOrConstructor(BindingTypeBuilder binding, INamedTypeSymbol namedType) => namedType.Constructors.Where(c => c.DeclaredAccessibility != Accessibility.Private && c.DeclaredAccessibility != Accessibility.Protected).OrderByDescending(c => c.Parameters.Length).FirstOrDefault();

    protected override TypeInfo GetTargetType(BindingTypeBuilder binding) => binding.TargetType;
}

//internal class TypeCodeGenerator1 : ITypeCodeGenerator
//{
//    #region Fields

//    private readonly CancellationToken _cancellation;
//    private readonly IContentPersistence _contentPersistence;
//    private readonly Dictionary<ITypeSymbol, string> _generated;

//    #endregion Fields

//    #region Constructors

//    public TypeCodeGenerator1(IContentPersistence contentPersistence, CancellationToken cancellation)
//    {
//        _contentPersistence = contentPersistence ?? throw new System.ArgumentNullException(nameof(contentPersistence));
//        _cancellation = cancellation;
//        _generated = new(SymbolEqualityComparer.Default);
//    }

//    #endregion Constructors

//    #region Methods

//    public bool TryGenerate(IResolveBinding binding, out string className)
//    {
//        if (binding is not BindingTypeBuilder typeBuilder)
//        {
//            className = string.Empty;
//            return false;
//        }

//        if(_generated.TryGetValue(typeBuilder.TargetType.Type!, out className))
//        {
//            return true;
//        }

//        var nameType = (INamedTypeSymbol)typeBuilder.TargetType.Type!;
//        var constructor = nameType.Constructors.Where(c => c.DeclaredAccessibility != Accessibility.Private && c.DeclaredAccessibility != Accessibility.Protected).OrderByDescending(c => c.Parameters.Length).FirstOrDefault();

//        className = Code.TypeClassName(nameType);

//        var content = Generate(nameType, constructor);

//        _cancellation.ThrowIfCancellationRequested();
//        _contentPersistence.Persist(className, content);
//        _generated[typeBuilder.TargetType.Type!] = className;

//        return true;
//    }

//    private string ConstructResolve(INamedTypeSymbol typeSymbol, IMethodSymbol constructor)
//    {
//        if (constructor is null || constructor.Parameters.Length == 0)
//        {
//            return new StringBuilder(Code.ReturnNewType(typeSymbol)).ToString();
//        }

//        StringBuilder builder = new();

//        List<string> parameterNames = new();
//        if (constructor is not null && constructor.Parameters.Length > 0)
//        {
//            builder.AppendLine(Code.Ioc);
//            foreach (var parameter in constructor.Parameters)
//            {
//                parameterNames.Add(parameter.Name);
//                builder.AppendLine(Code.Resolve(parameter));
//            }
//        }

//        builder.Append(Code.ReturnNewType(typeSymbol, parameterNames));

//        return builder.ToString();
//    }

//    private string ConstructResolveWithNamedParameters(INamedTypeSymbol typeSymbol, IMethodSymbol constructor)
//    {
//        if (constructor is null || constructor.Parameters.Length == 0)
//        {
//            return new StringBuilder(Code.ReturnNewType(typeSymbol)).ToString();
//        }

//        StringBuilder builder = new();
//        builder.AppendLine(Code.ParameterCheck);

//        StringBuilder foreachLoopBuilder = new();
//        foreachLoopBuilder.AppendLine(Code.ForeachLoop)
//            .AppendLine(Code.OpenBrace);

//        List<string> parameterNames = new();

//        foreach (var parameter in constructor.Parameters)
//        {
//            parameterNames.Add(parameter.Name);
//            builder.AppendLine(Code.NamedTypedParameterDeclaration(parameter));

//            foreachLoopBuilder.AppendTab().AppendLine(Code.ParameterIfEquals(parameter))
//                .AppendTab().AppendLine(Code.OpenBrace)
//                .AppendTab(2).AppendLine(Code.ParameterValueAssignment(parameter))
//                .AppendTab(2).AppendLine(Code.ContinueStatement)
//                .AppendTab().AppendLine(Code.CloseBrace);
//            foreachLoopBuilder.AppendLine();
//        }

//        foreachLoopBuilder.AppendLine(Code.CloseBrace);

//        builder.AppendLine(foreachLoopBuilder.ToString());

//        builder.Append(Code.ReturnNewType(typeSymbol, parameterNames));

//        return builder.ToString();
//    }

//    private string ConstructResolveWithParameters(INamedTypeSymbol typeSymbol, IMethodSymbol constructor)
//    {
//        if (constructor is null || constructor.Parameters.Length == 0)
//        {
//            return new StringBuilder(Code.ReturnNewType(typeSymbol)).ToString();
//        }

//        StringBuilder builder = new();
//        builder.AppendLine(Code.ParameterCheck);

//        List<string> parameterNames = new();
//        int index = 0;
//        foreach (var parameter in constructor.Parameters)
//        {
//            parameterNames.Add(parameter.Name);
//            builder.AppendLine(Code.ParameterCast(parameter, index));
//            index++;
//        }

//        builder.Append(Code.ReturnNewType(typeSymbol, parameterNames));

//        return builder.ToString();
//    }

//    private string Generate(INamedTypeSymbol typeSymbol, IMethodSymbol constructor)
//    {
//        string resolve = ConstructResolve(typeSymbol, constructor);
//        string resolveObjectParameters = ConstructResolveWithParameters(typeSymbol, constructor);
//        string resolveNamedParameters = ConstructResolveWithNamedParameters(typeSymbol, constructor);

//        var classBuilder = new StringBuilder()
//            .AppendLine(Code.ClassDeclaration(typeSymbol))
//            .AppendLine(Code.OpenBrace)
//            .AppendTab().AppendLine(Code.Constructor(typeSymbol))
//            .AppendTab().AppendLine(Code.OpenBrace)
//            .AppendTab(2).AppendLine(Code.InfoPropertyAssign)
//            .AppendTab().AppendLine(Code.CloseBrace)
//            .AppendLine()
//            .AppendTab().AppendLine(Code.InfoProperty)
//            .AppendLine()
//            .AppendTab().AppendLine(Code.ResolveMethod)
//            .AppendTab().AppendLine(Code.OpenBrace)
//            .AppendTab(resolve, 2).AppendLine()
//            .AppendTab().AppendLine(Code.CloseBrace)
//            .AppendLine()
//            .AppendTab().AppendLine(Code.ResolveWithObjectParametersMethod)
//            .AppendTab().AppendLine(Code.OpenBrace)
//            .AppendTab(resolveObjectParameters, 2).AppendLine()
//            .AppendTab().AppendLine(Code.CloseBrace)
//            .AppendLine()
//            .AppendTab().AppendLine(Code.ResolveWithNamedParametersMethod)
//            .AppendTab().AppendLine(Code.OpenBrace)
//            .AppendTab(resolveNamedParameters, 2).AppendLine()
//            .AppendTab().AppendLine(Code.CloseBrace)
//            .AppendLine(Code.CloseBrace);

//        string content = classBuilder.ToString();

//        return content;
//    }

//    #endregion Methods
//}