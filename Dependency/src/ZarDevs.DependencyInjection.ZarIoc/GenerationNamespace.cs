using Microsoft.CodeAnalysis;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace ZarDevs.DependencyInjection.SourceGenerator;

/// <summary>
/// Container that has all the <see cref="BindingInfo"/> for a namespace.
/// </summary>
public class GenerationNamespace : IEnumerable<BindingInfo>
{
    private readonly HashSet<BindingInfo> _dependencyRegistrations;
    private readonly HashSet<BindingInfo> _methodRegistrations;
    private readonly HashSet<string> _typeNames;

    /// <summary>
    /// Create a new instance of the <see cref="GenerationNamespace"/>
    /// </summary>
    /// <param name="name"></param>
    /// <exception cref="ArgumentException"></exception>
    public GenerationNamespace(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException($"'{nameof(name)}' cannot be null or whitespace.", nameof(name));
        }

        Name = name;

        _dependencyRegistrations = new HashSet<BindingInfo>();
        _methodRegistrations = new HashSet<BindingInfo>();
        _typeNames = new HashSet<string>();
    }

    /// <summary>
    /// The namespace name.
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// Add a binding to the namespace
    /// </summary>
    /// <exception cref="InvalidOperationException">When <paramref name="item"/> is null.</exception>
    public void Add(BindingInfo item)
    {
        if(!string.Equals(Name, item.Namespace))
        {
            throw new InvalidOperationException($"The current namespace '{Name}' is not the same as the binding: {item}");
        }

        if (item.IsMethodBinding())
        { 
            _methodRegistrations.Add(item);
        }
        else
        {
            _typeNames.Add(item.Class!);
            _dependencyRegistrations.Add(item);
        }
    }

    /// <inheritdoc/>
    public IEnumerator<BindingInfo> GetEnumerator()
    {
        return _dependencyRegistrations.Union(_methodRegistrations).GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}