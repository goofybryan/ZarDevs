using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Xml.Serialization;

namespace ZarDevs.DependencyInjection.SourceGenerator;

/// <summary>
/// Loader that will deserialize the xml paths and add to container.
/// </summary>
public class GenerationLoader : IEnumerable<BindingInfo>
{
    #region Fields

    /// <summary>
    /// Get all the bindings
    /// </summary>
    public IDictionary<string, IList<BindingInfo>> Bindings;

    private readonly CancellationToken _cancellation;
    private readonly IDiagnosticLogger _logger;
    private readonly XmlSerializer _serializer;

    #endregion Fields

    #region Constructors

    /// <summary>
    /// Create a new instance of the <see cref="GenerationLoader"/>
    /// </summary>
    /// <param name="logger">Spaicify the diagnostic logger</param>
    /// <param name="cancellation">Specify the <see cref="CancellationToken"/></param>
    /// <param name="paths"></param>
    /// <exception cref="ArgumentNullException"></exception>
    public GenerationLoader(IDiagnosticLogger logger, CancellationToken cancellation, IList<string> paths)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _serializer = new(typeof(Bindings));
        _cancellation = cancellation;

        Bindings = new Dictionary<string, IList<BindingInfo>>();

        Load(paths);
    }

    #endregion Constructors

    #region Methods

    /// <inheritdoc/>
    public IEnumerator<BindingInfo> GetEnumerator()
    {
        return Bindings.Values.SelectMany(v => v).GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    private void AddAssemblyScan(IList<BindingInfo> infoList, BindingInfo binding)
    {
        var normalBindings = infoList.Where(i => !i.IsMethodBinding()).ToList();

        foreach (var bindingInfo in normalBindings)
        {
            _logger.RemovedScannedNamespace(bindingInfo.Namespace!, binding.Class!);

            infoList.Remove(bindingInfo);
        }

        infoList.Add(binding);
        _logger.AddedNamespace(binding.Namespace!);
    }

    private void AddMethod(IList<BindingInfo> infoList, BindingInfo binding)
    {
        if (infoList.Any(i => i.Method == binding.Method)) return;

        _logger.AddedNamespace(binding.Namespace!, binding.Class!, binding.Method!);
        infoList.Add(binding);
    }

    private void AddNormal(IList<BindingInfo> infoList, BindingInfo binding)
    {
        if (infoList.Any(i => i.Class == binding.Class)) return;

        _logger.AddedNamespace(binding.Namespace!, binding.Class!);
        infoList.Add(binding);
    }

    private Bindings Deserialize(string path)
    {
        _logger.InfomationXmlLoading(path);
        using StreamReader reader = new(path);
        return (Bindings)_serializer.Deserialize(reader);
    }

    private void Load(IList<string> paths)
    {
        foreach (string path in paths)
        {
            var bindings = LoadFile(path);
            if (bindings != null)
            {
                Merge(bindings);
            }

            _cancellation.ThrowIfCancellationRequested();
        }
    }

    private Bindings? LoadFile(string path)
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

    private void Merge(Bindings bindings)
    {
        for (int i = 0; i < bindings.BindingInfoList!.Count; i++)
        {
            BindingInfo binding = bindings.BindingInfoList[i];

            if (!ValidateBindingInfo(i, binding))
            {
                continue;
            }

            AddBinding(binding);

            _cancellation.ThrowIfCancellationRequested();
        }
    }

    private void AddBinding(BindingInfo binding)
    {

        _logger.LoadingBinding(binding);

        if (!Bindings.TryGetValue(binding.Namespace!, out var infoList))
        {
            Bindings[binding.Namespace!] = infoList = new List<BindingInfo>();
        }

        if (binding.IsMethodBinding())
        {
            AddMethod(infoList, binding);
        }
        else
        {
            AddNormal(infoList, binding);
        }
    }

    private bool ValidateBindingInfo(int index, BindingInfo binding)
    {
        if (string.IsNullOrWhiteSpace(binding.Namespace))
        {
            _logger.NamespaceIsNullOrWhiteSpace(index);
            return false;
        }

        if (!string.IsNullOrEmpty(binding.Method) && string.IsNullOrWhiteSpace(binding.Class))
        {
            _logger.NamespaceClassForMethodNotSpecified(binding.Namespace!, binding.Method!);
            return false;
        }

        return true;
    }

    #endregion Methods
}