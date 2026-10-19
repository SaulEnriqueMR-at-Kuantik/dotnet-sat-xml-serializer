using System.Text.Json.Serialization;
using System.Xml.Serialization;
using KpacModels.Shared.Models.Constants;

namespace KpacModels.Shared.Models.Comprobante.ComplementosConcepto;

[XmlRoot(ElementName = "HidroYPetro", Namespace = Namespaces.HidrocarburosPetroliferos)]
public class HidrocarburosPetroliferos
{
    [XmlAttribute("Version")]
    [JsonPropertyName("Version")]
    public string Version { get; set; }

    [XmlAttribute("TipoPermiso")]
    [JsonPropertyName("TipoPermiso")]
    public string TipoPermiso { get; set; }

    [XmlAttribute("NumeroPermiso")]
    [JsonPropertyName("NoPermiso")]
    public string NoPermiso { get; set; }

    [XmlAttribute("ClaveHYP")]
    [JsonPropertyName("Clave")]
    public string Clave { get; set; }

    [XmlAttribute("SubProductoHYP")]
    [JsonPropertyName("Subproducto")]
    public string Subproducto { get; set; }
}