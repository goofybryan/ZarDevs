using Microsoft.CodeAnalysis;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace ZarDevs.DependencyInjection.SourceGenerator;

/// <summary>
/// Generation container that contains a list of bindings in external assemblies. Will be used for
/// external resource building.
/// </summary>
public class GenerationContainer : IEnumerable<AssemblyBindingInfo>
{
    #region Fields

    private readonly IDictionary<string, IList<AssemblyBindingInfo>> _bindings;
    private readonly CancellationToken _cancellation;
    private readonly GenerationReferences _references;
    private IDiagnosticLogger _logger;

    #endregion Fields

    #region Constructors

    /// <summary>
    /// Create a new instance of the <see cref="GenerationContainer"/>
    /// </summary>
    /// <param name="references">Specify the list of assembly references the current project has</param>
    /// <param name="logger">Spaicify the diagnostic logger</param>
    /// <param name="cancellation">Specify the <see cref="CancellationToken"/></param>
    /// <exception cref="ArgumentNullException"></exception>
    public GenerationContainer(GenerationReferences references, IDiagnosticLogger logger, CancellationToken cancellation)
    {
        _references = references ?? throw new ArgumentNullException(nameof(references));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _cancellation = cancellation;
        _bindings = new Dictionary<string, IList<AssemblyBindingInfo>>();
    }

    #endregion Constructors

    #region Properties

    /// <summary>
    /// Get a list of assemblies this container has.
    /// </summary>
    public IReadOnlyCollection<string> Assemblies => _bindings.Keys.ToArray();

    #endregion Properties

    #region Methods

    /// <inheritdoc/>
    public IEnumerator<AssemblyBindingInfo> GetEnumerator()
    {
        return _bindings.Values.SelectMany(v => v).GetEnumerator();
    }

    /// <inheritdoc/>
    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    /// <summary>
    /// Merge the current bindings with the container. Filter out already specified bindings.
    /// </summary>
    /// <param name="bindings">The bindings to merged.</param>
    public void Merge(Bindings bindings)
    {
        for (int i = 0; i < bindings.BindingInfoList.Count; i++)
        {
            AssemblyBindingInfo binding = bindings.BindingInfoList[i];

            if (!ValidateBindingInfo(i, binding))
            {
                continue;
            }

            _logger.LoadingBinding(binding);

            if (!_bindings.TryGetValue(binding.Assembly, out var infoList))
            {
                _bindings[binding.Assembly] = infoList = new List<AssemblyBindingInfo>();
            }

            if (binding.IsAssemblyScan())
            {
                AddAssemblyScan(infoList, binding);
            }
            else if (binding.IsMethodBinding())
            {
                AddMethod(infoList, binding);
            }
            else
            {
                AddNormal(infoList, binding);
            }

            _cancellation.ThrowIfCancellationRequested();
        }
    }

    private void AddAssemblyScan(IList<AssemblyBindingInfo> infoList, AssemblyBindingInfo binding)
    {
        var normalBindings = infoList.Where(i => !i.IsMethodBinding()).ToList();

        foreach (var bindingInfo in normalBindings)
        {
            _logger.RemovedScannedAssembly(bindingInfo.Assembly, binding.Class);

            infoList.Remove(bindingInfo);
        }

        infoList.Add(binding);
        _logger.AddedAssembly(binding.Assembly);
    }

    private void AddMethod(IList<AssemblyBindingInfo> infoList, AssemblyBindingInfo binding)
    {
        if (infoList.Any(i => i.Method == binding.Method)) return;

        _logger.AddedAssembly(binding.Assembly, binding.Class, binding.Method);
        infoList.Add(binding);
    }

    private void AddNormal(IList<AssemblyBindingInfo> infoList, AssemblyBindingInfo binding)
    {
        if (infoList.Any(i => i.IsAssemblyScan() || i.Class == binding.Class)) return;

        _logger.AddedAssembly(binding.Assembly, binding.Class);
        infoList.Add(binding);
    }

    private bool ValidateBindingInfo(int index, AssemblyBindingInfo binding)
    {
        if (string.IsNullOrWhiteSpace(binding.Assembly))
        {
            _logger.AssemblyNotSpecified(index);
            return false;
        }

        if (!_references.ContainsAssemblyName(binding.Assembly))
        {
            _logger.AssemblyNotReferenced(binding.Assembly);
            return false;
        }

        if (!string.IsNullOrEmpty(binding.Method) && string.IsNullOrWhiteSpace(binding.Class))
        {
            _logger.AssemblyClassNotSpecified(binding.Assembly, binding.Method);
            return false;
        }

        return true;
    }

    #endregion Methods
}