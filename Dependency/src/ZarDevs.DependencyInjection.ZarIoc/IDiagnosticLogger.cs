using Microsoft.CodeAnalysis;
using System;
using ZarDevs.DependencyInjection.Properties;

namespace ZarDevs.DependencyInjection.SourceGenerator
{
    /// <summary>
    /// Diagnostic logger to log generation messages
    /// </summary>
    public interface IDiagnosticLogger
    {
        /// <summary>
        /// Log a diagnostic message.
        /// </summary>
        /// <param name="id">Specify the message ID. Ideally this will be dependent on the where the generation occurs.</param>
        /// <param name="title">Specify the title</param>
        /// <param name="message">Specify the message</param>
        /// <param name="severity">Specify the severity</param>
        /// <param name="category">Specify the category</param>
        void Log(string id, string title, string message, DiagnosticSeverity severity, string category);
    }

    internal static class DiagnosticLoggerExtensions
    {
        public static void InformationBindingCount(this IDiagnosticLogger logger, int bindingLength) => logger.Log(nameof(Resources.ZarI001), Resources.TitleInformation, string.Format(Resources.ZarI001, bindingLength), DiagnosticSeverity.Info, Resources.CategoryLoading);
        public static void InfomationXmlLoading(this IDiagnosticLogger logger, string path) => logger.Log(nameof(Resources.ZarI002), Resources.TitleInformation, string.Format(Resources.ZarI002, path), DiagnosticSeverity.Info, Resources.CategoryLoading);
        public static void Exception(this IDiagnosticLogger logger, Exception exception) => logger.Log(nameof(Resources.ZarE001), string.Format(Resources.TitleException, exception.Message), string.Format(Resources.ZarE001, exception), DiagnosticSeverity.Error, Resources.CategoryException);
        public static void AddedAssembly(this IDiagnosticLogger logger, string assembly) => logger.Log(nameof(Resources.ZarI004), Resources.TitleInformation, string.Format(Resources.ZarI004, assembly), DiagnosticSeverity.Info, Resources.CategoryLoading);
        public static void AddedAssembly(this IDiagnosticLogger logger, string assembly, string className) => logger.Log(nameof(Resources.ZarI005), Resources.TitleInformation, string.Format(Resources.ZarI005, assembly, className), DiagnosticSeverity.Info, Resources.CategoryLoading);
        public static void AddedAssembly(this IDiagnosticLogger logger, string assembly, string className, string methodName) => logger.Log(nameof(Resources.ZarI006), Resources.TitleInformation, string.Format(Resources.ZarI006, assembly, className, methodName), DiagnosticSeverity.Info, Resources.CategoryLoading);
        public static void RemovedScannedAssembly(this IDiagnosticLogger logger, string assembly, string className) => logger.Log(nameof(Resources.ZarI007), Resources.TitleInformation, string.Format(Resources.ZarI007, assembly, className), DiagnosticSeverity.Info, Resources.CategoryLoading);
        public static void AssemblyNotReferenced(this IDiagnosticLogger logger, string assembly) => logger.Log(nameof(Resources.ZarE004), Resources.TitleError, string.Format(Resources.ZarE004, assembly), DiagnosticSeverity.Error, Resources.CategoryLoading);
        public static void AssemblyNotSpecified(this IDiagnosticLogger logger, int index) => logger.Log(nameof(Resources.ZarE002), Resources.TitleError, string.Format(Resources.ZarE002, index), DiagnosticSeverity.Error, Resources.CategoryLoading);
        public static void AssemblyClassNotSpecified(this IDiagnosticLogger logger, string assembly, string methodName) => logger.Log(nameof(Resources.ZarE003), Resources.TitleError, string.Format(Resources.ZarE003, assembly, methodName), DiagnosticSeverity.Error, Resources.CategoryLoading);
        public static void LoadingBinding(this IDiagnosticLogger logger, BindingInfo binding) => logger.Log(nameof(Resources.ZarI003), Resources.TitleInformation, string.Format(Resources.ZarI003, binding), DiagnosticSeverity.Info, Resources.CategoryLoading);
        public static void CreatingInstances(this IDiagnosticLogger logger, BindingInfo binding) => logger.Log(nameof(Resources.ZarI008), Resources.TitleInformation, string.Format(Resources.ZarI008, binding), DiagnosticSeverity.Info, Resources.CategoryLoading);
    }
}