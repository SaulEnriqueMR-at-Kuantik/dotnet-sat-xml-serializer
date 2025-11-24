using System.Text.Json.Serialization;
using System.Xml.Serialization;
using KpacModels.Shared.Models.Constants;

namespace KpacModels.Shared.Models.Comprobante.Complementos.Nomina;

public class Deducciones
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("TotalOtrasDeducciones")]
    [XmlAttribute(AttributeName = "TotalOtrasDeducciones")]
    public string? TotalOtrasDeducciones { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("TotalImpuestosRetenidos")]
    [XmlAttribute(AttributeName = "TotalImpuestosRetenidos")]
    public string? TotalImpuestosRetenidos { get; set; }

    [XmlElement(ElementName = "Deduccion", Namespace = Namespaces.Nomina12)]
    [JsonPropertyName("Deduccion")]
    public List<Deduccion> Deduccion { get; set; }

    public bool ShouldSerializeDeduccion() => Deduccion != null && Deduccion.Count > 0;
}