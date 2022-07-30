using System;
using System.Xml;
using System.Xml.Serialization;

namespace ZarDevs.DependencyInjection.SourceGenerator;

/// <summary>
/// Binding information that contains the list of generation information.
/// </summary>
public class BindingInfo : IEquatable<BindingInfo>
{
    #region Properties

    /// <summary>
    /// Specifiy the class that contains the binding. Not Required. If not specified, the assembly
    /// will be scanned for all classes that contain <see cref="IDependencyRegistration"/>
    /// </summary>
    [XmlAttribute]
    public string Class { get; set; }

    /// <summary>
    /// Specify the method for the <see cref="Class"/>. Not Required. If the class is not specified,
    /// no source will be generated.
    /// </summary>
    [XmlAttribute]
    public string Method { get; set; }

    /// <summary>
    /// Specify the assembly that contains the binding. Required.
    /// </summary>
    [XmlAttribute]
    public string Namespace { get; set; }

    #endregion Properties

    #region Methods

    /// <summary>
    /// Indicate if this is an assembly scan.
    /// </summary>
    /// <returns></returns>
    public bool IsMethodBinding() => !string.IsNullOrWhiteSpace(Method);

    /// <inheritdoc/>
    public bool Equals(BindingInfo other)
    {
        if (other == null) return false;

        return string.Equals(Namespace, other.Namespace, StringComparison.Ordinal)
            && string.Equals(Class, other.Class, StringComparison.Ordinal)
            && string.Equals(Method, other.Method, StringComparison.Ordinal);
    }

    /// <inheritdoc/>
    public override bool Equals(object obj)
    {
        return obj is BindingInfo info ? Equals(info) : base.Equals(obj);
    }

    /// <inheritdoc/>
    public override int GetHashCode()
    {
        unchecked
        {
            int hashcode = 965267;
            hashcode = hashcode * 720697 ^ Namespace.GetHashCode();
            hashcode = hashcode * 720697 ^ Class?.GetHashCode() ?? 0;
            hashcode = hashcode * 720697 ^ Method?.GetHashCode() ?? 0;
            return hashcode;
        }
    }

    /// <inheritdoc/>
    public override string ToString()
    {
        if (IsMethodBinding()) return $"{Namespace}:{Class}:{Method}";

        return $"{Namespace}:{Class}:{nameof(IDependencyRegistration)}";
    }

    #endregion Methods
}