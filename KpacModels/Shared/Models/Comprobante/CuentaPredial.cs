using System.Text.Json.Serialization;
using System.Xml.Serialization;

namespace KpacModels.Shared.Models.Comprobante;

public class CuentaPredial
{
    [XmlAttribute(AttributeName = "Numero")]
    [JsonPropertyName("Numero")]
    public string Numero { get; set; }
}