using System;
using System.Collections;
using System.Collections.Generic;

namespace ZarDevs.DependencyInjection.SourceGenerator;

/// <inheritdoc/>
public class GenerationContainer : IEnumerable<GenerationNamespace>
{
    #region Fields

    private readonly IDictionary<string, GenerationNamespace> _bindings;

    #endregion Fields

    #region Constructors

    /// <summary>
    /// Create a new instance of the <see cref="GenerationContainer"/>
    /// </summary>
    /// <exception cref="ArgumentNullException"></exception>
    public GenerationContainer(IEnumerable<BindingInfo> bindingInfos)
    {
        _bindings = new Dictionary<string, GenerationNamespace>();

        AddBindings(bindingInfos);
    }

    #endregion Constructors

    #region Indexers

    /// <summary>
    /// Get the <see cref="GenerationNamespace"/> for the <paramref name="namespace"/>
    /// </summary>
    public GenerationNamespace this[string @namespace] => _bindings[@namespace];

    #endregion Indexers

    #region Methods

    private void AddBindings(IEnumerable<BindingInfo> bindingInfos)
    {
        foreach (var bindingInfo in bindingInfos)
        {
            AddBinding(bindingInfo);
        }
    }

    /// <inheritdoc/>
    public IEnumerator<GenerationNamespace> GetEnumerator()
    {
        return _bindings.Values.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    private void AddBinding(BindingInfo bindingInfo)
    {
        if (bindingInfo is null)
        {
            throw new ArgumentNullException(nameof(bindingInfo));
        }

        var @namespace = bindingInfo.Namespace!;

        if (!_bindings.TryGetValue(@namespace, out var generationNameSpance))
        {
            generationNameSpance = new(@namespace);
            _bindings.Add(@namespace, generationNameSpance);
        }

        generationNameSpance.Add(bindingInfo);
    }

    private bool ContainsNamespace(string name)
    {
        return _bindings.ContainsKey(name);
    }

    #endregion Methods
}