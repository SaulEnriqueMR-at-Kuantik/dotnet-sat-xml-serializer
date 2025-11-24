using System.Text.Json.Serialization;
using System.Xml.Serialization;
using KpacModels.Shared.Models.Constants;

namespace KpacModels.Shared.Models.Retenciones.Complementos.EnajenacionAcciones;

[XmlRoot(ElementName = "EnajenaciondeAcciones", Namespace = Namespaces.EnajenacionAcciones10)]
public class EnajenacionAcciones10
{
    [XmlAttribute(AttributeName = "Version")]
    [JsonPropertyName("Version")]
    public string Version { get; set; }

    [XmlAttribute(AttributeName = "ContratoIntermediacion")]
    [JsonPropertyName("ContratoIntermediacion")]
    public string ContratoIntermediacion { get; set; }

    [XmlAttribute(AttributeName = "Ganancia")]
    [JsonPropertyName("Ganancia")]
    public string Ganancia { get; set; }

    [XmlAttribute(AttributeName = "Perdida")]
    [JsonPropertyName("Perdida")]
    public string Perdida { get; set; }
}