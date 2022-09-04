using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace ZarDevs.DependencyInjection.SourceGenerator;

internal interface IIntanceBinding
{
    #region Properties

    ArgumentSyntax Instance { get; }

    #endregion Properties
}
