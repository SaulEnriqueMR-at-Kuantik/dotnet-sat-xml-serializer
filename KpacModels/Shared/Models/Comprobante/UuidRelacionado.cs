using System.Text.Json.Serialization;
using System.Xml.Serialization;

namespace KpacModels.Shared.Models.Comprobante;

public class UuidRelacionado
{
    [XmlAttribute(AttributeName = "UUID")]
    [JsonPropertyName("Uuid")]
    public string Uuid { get; set; }
}