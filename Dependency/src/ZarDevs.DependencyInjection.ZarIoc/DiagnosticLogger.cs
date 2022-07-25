using Microsoft.CodeAnalysis;
using System.Diagnostics;

namespace ZarDevs.DependencyInjection.SourceGenerator;

/// <inheritdoc/>
public class DiagnosticLogger : IDiagnosticLogger
{
    #region Fields

    private readonly GeneratorExecutionContext _context;

    #endregion Fields

    #region Constructors

    /// <summary>
    /// Create a new instance of the <see cref="DiagnosticLogger"/>
    /// </summary>
    /// <param name="context">Specify the generation exception context.</param>
    public DiagnosticLogger(GeneratorExecutionContext context)
    {
        _context = context;
    }

    #endregion Constructors

    #region Methods

    /// <inheritdoc/>
    public void Log(string id, string title, string message, DiagnosticSeverity severity, string category)
    {
        Debug.WriteLine($"{nameof(DependencyGenerator)}:{severity}:{category}:{id}:{title}:{message}");

        var descriptor = new DiagnosticDescriptor(id, title, message, category, severity, true);
        var diagnostic = Diagnostic.Create(descriptor, null, message);
        _context.ReportDiagnostic(diagnostic);
    }

    #endregion Methods
}