using Microsoft.CodeAnalysis;

namespace ZarDevs.DependencyInjection.SourceGenerator
{
    /// <summary>
    /// Diagnostic logger to log generation messages
    /// </summary>
    public interface IDiagnosticLogger
    {
        #region Methods

        /// <summary>
        /// Log a diagnostic message.
        /// </summary>
        /// <param name="id">
        /// Specify the message ID. Ideally this will be dependent on the where the generation occurs.
        /// </param>
        /// <param name="title">Specify the title</param>
        /// <param name="message">Specify the message</param>
        /// <param name="severity">Specify the severity</param>
        /// <param name="category">Specify the category</param>
        void Log(string id, string title, string message, DiagnosticSeverity severity, string category);

        /// <summary>
        /// Log a diagnostic message.
        /// </summary>
        /// <param name="id">
        /// Specify the message ID. Ideally this will be dependent on the where the generation occurs.
        /// </param>
        /// <param name="title">Specify the title</param>
        /// <param name="message">Specify the message</param>
        /// <param name="severity">Specify the severity</param>
        /// <param name="category">Specify the category</param>
        /// <param name="location">Specify the location.</param>
        void Log(string id, string title, string message, DiagnosticSeverity severity, string category, Location? location);

        #endregion Methods
    }
}