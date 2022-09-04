using Microsoft.CodeAnalysis;
using System.Diagnostics;

namespace ZarDevs.DependencyInjection.SourceGenerator;

/// <inheritdoc/>
public class DiagnosticLogger : IDiagnosticLogger
{
    #region Fields

    private readonly SourceProductionContext _context;

    #endregion Fields

    #region Constructors

    /// <summary>
    /// Create a new instance of the <see cref="DiagnosticLogger"/>
    /// </summary>
    /// <param name="context">Specify the generation exception context.</param>
    public DiagnosticLogger(SourceProductionContext context)
    {
        _context = context;
    }

    #endregion Constructors

    #region Methods

    /// <inheritdoc/>
    public void Log(string id, string title, string message, DiagnosticSeverity severity, string category)
    {
        Log(id, title, message, severity, category, null);
    }

    /// <inheritdoc/>
    public void Log(string id, string title, string message, DiagnosticSeverity severity, string category, Location? location)
    {
        Debug.WriteLine($"{nameof(DependencyGenerator)}:{severity}:{category}:{id}:{title}:{message}");

        var descriptor = new DiagnosticDescriptor(id, title, message, category, severity, true);
        var diagnostic = Diagnostic.Create(descriptor, location, message);
        _context.ReportDiagnostic(diagnostic);
    }

    #endregion Methods
}