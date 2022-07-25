using System.Xml;
using System.Xml.Serialization;

namespace ZarDevs.DependencyInjection.SourceGenerator;

/// <summary>
/// Binding information that contains the list of generation information.
/// </summary>
public class BindingInfo
{
    /// <summary>
    /// Specifiy the class that contains the binding. Not Required. If not specified, the assembly will be scanned for all classes that contain <see cref="IDependencyRegistration"/>
    /// </summary>
    [XmlAttribute]
    public string Class { get; set; }

    /// <summary>
    /// Specify the method for the <see cref="Class"/>. Not Required. If the class is not specified, no source will be generated.
    /// </summary>
    [XmlAttribute]
    public string Method { get; set; }

    /// <summary>
    /// Indicate if this is an assembly scan.
    /// </summary>
    /// <returns></returns>
    public bool IsAssemblyScan() => string.IsNullOrWhiteSpace(Class) && string.IsNullOrWhiteSpace(Method);

    /// <summary>
    /// Indicate if this is an assembly scan.
    /// </summary>
    /// <returns></returns>
    public bool IsMethodBinding() => !string.IsNullOrWhiteSpace(Method);
}


/// <summary>
/// Binding information that contains the list of generation information for assemblies.
/// </summary>
public class AssemblyBindingInfo : BindingInfo
{
    /// <summary>
    /// Specify the assembly that contains the binding. Required.
    /// </summary>
    [XmlAttribute]
    public string Assembly { get; set; }
    
    /// <inheritdoc/>
    public override string ToString()
    {
        if(IsAssemblyScan()) return $"{Assembly}:Scan";

        if(IsMethodBinding()) return $"{Assembly}:{Class}:{Method}";

        return $"{Assembly}:{Class}:{nameof(IDependencyRegistration)}";
    }
}