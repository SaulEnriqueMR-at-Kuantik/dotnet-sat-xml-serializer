using System.Text.Json.Serialization;
using System.Xml.Serialization;

namespace KpacModels.Shared.Models.Comprobante;

public class ImpuestoR
{
    [XmlAttribute(AttributeName = "Impuesto")]
    [JsonPropertyName("Impuesto")]
    public string Impuesto { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("Importe")]
    [XmlAttribute(AttributeName = "Importe")]
    public string? Importe { get; set; }
}