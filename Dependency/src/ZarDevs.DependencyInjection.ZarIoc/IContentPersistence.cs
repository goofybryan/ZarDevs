namespace ZarDevs.DependencyInjection.SourceGenerator;

/// <summary>
/// This will persist the current content to a class
/// </summary>
public interface IContentPersistence
{
    /// <summary>
    /// Persist the <paramref name="content"/> to the <paramref name="className"/> to file to be compiled.
    /// </summary>
    /// <param name="className">The name of the class. This will become the filename</param>
    /// <param name="content">The content that will be persisted. This must not include any namespaces as this will be added.</param>
    /// <param name="usings">Add any usings required by the file</param>
    void Persist(string className, string content, params string[] usings);
}
