using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace ZarDevs.DependencyInjection.SourceGenerator;

internal interface IFactoryBinding
{
    #region Properties

    TypeInfo Factory { get; }
    ArgumentSyntax MethodName { get; }

    #endregion Properties
}
