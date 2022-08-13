using Microsoft.CodeAnalysis;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using TypeInfo = Microsoft.CodeAnalysis.TypeInfo;

namespace ZarDevs.DependencyInjection.SourceGenerator;

internal class TypeCodeGenerator : CodeGeneratorBase<BindingTypeBuilder>
{
    #region Constructors

    public TypeCodeGenerator(IContentPersistence contentPersistence, CancellationToken cancellation) : base(contentPersistence, cancellation)
    {
    }

    #endregion Constructors

    #region Methods

    protected override ClassDefinition GenerateClassName(BindingTypeBuilder binding, INamedTypeSymbol namedType) => Code.TypeClassName(namedType);

    protected override string GenerateReturnWithNoParameters(BindingTypeBuilder binding, ClassDefinition classDefinition) => Code.ReturnNewType(classDefinition);

    protected override string GenerateReturnWithParameters(BindingTypeBuilder binding, ClassDefinition classDefinition, List<string> parameterNames) => Code.ReturnNewType(classDefinition, parameterNames);

    protected override IMethodSymbol GetTargetMethodOrConstructor(BindingTypeBuilder binding, INamedTypeSymbol namedType) => namedType.Constructors.Where(c => c.DeclaredAccessibility != Accessibility.Private && c.DeclaredAccessibility != Accessibility.Protected).OrderByDescending(c => c.Parameters.Length).FirstOrDefault();

    protected override TypeInfo GetTargetType(BindingTypeBuilder binding) => binding.TargetType;

    #endregion Methods
}