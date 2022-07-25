using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Xml.Serialization;

namespace ZarDevs.DependencyInjection.SourceGenerator;

/// <summary>
/// Loader that will deserialize the xml paths and add to container.
/// </summary>
public class GenerationLoader
{
    private readonly IDiagnosticLogger _logger;
    private readonly CancellationToken _cancellation;
    private readonly XmlSerializer _serializer;

    /// <summary>
    /// Create a new instance of the <see cref="GenerationLoader"/>
    /// </summary>
    /// <param name="container">Specify the container that will house the bindings.</param>
    /// <param name="logger">Spaicify the diagnostic logger</param>
    /// <param name="cancellation">Specify the <see cref="CancellationToken"/></param>
    /// <param name="paths"></param>
    /// <exception cref="ArgumentNullException"></exception>
    public GenerationLoader(GenerationContainer container, IDiagnosticLogger logger, CancellationToken cancellation, IList<string> paths)
    {
        Container = container ?? throw new ArgumentNullException(nameof(container));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _serializer = new(typeof(Bindings));
        _cancellation = cancellation;

        Load(paths);
    }

    /// <summary>
    /// Generation container.
    /// </summary>
    public GenerationContainer Container { get; }

    private void Load(IList<string> paths)
    {
        foreach(string path in paths)
        {
            var bindings = LoadFile(path);
            if (bindings != null)
            {
                Container.Merge(bindings);
            }

            _cancellation.ThrowIfCancellationRequested();
        }
    }

    private Bindings LoadFile(string path)
    {
        try
        {
            var bindings = Deserialize(path);
            return bindings;
        }
        catch (Exception exc)
        {
            _logger.Exception(exc);
        }

        return null;
    }

    private Bindings Deserialize(string path)
    {
        _logger.InfomationXmlLoading(path);
        using StreamReader reader = new(path);
        return (Bindings)_serializer.Deserialize(reader);
    }
}
