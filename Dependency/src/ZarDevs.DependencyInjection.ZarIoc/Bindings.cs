using System.Collections.Generic;
using System.ComponentModel;
using System.Xml;
using System.Xml.Serialization;

namespace ZarDevs.DependencyInjection.SourceGenerator;

/// <summary>
/// Bindings Root, contains a list of <see cref="BindingInfo"/>
/// </summary>
public class Bindings : BindingInfo
{
    /// <summary>
    /// List of binding information.
    /// </summary>
    [XmlElement("Binding", typeof(BindingInfo))]
    public List<BindingInfo>? BindingInfoList { get; set; }

    /// <summary>
    /// Specify that the current assembly should also be enabled
    /// </summary>
    [DefaultValue(true)]
    public bool EnableThis { get; set; } = true;
}
