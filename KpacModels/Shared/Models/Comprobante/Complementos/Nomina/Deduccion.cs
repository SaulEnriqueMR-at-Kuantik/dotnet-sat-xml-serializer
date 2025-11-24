using System.Text.Json.Serialization;
using System.Xml.Serialization;

namespace KpacModels.Shared.Models.Comprobante.Complementos.Nomina;

public class Deduccion
{
    [XmlAttribute(AttributeName = "TipoDeduccion")]
    [JsonPropertyName("Tipo")]
    public string Tipo { get; set; }

    [XmlAttribute(AttributeName = "Clave")]
    [JsonPropertyName("Clave")]
    public string Clave { get; set; }

    [XmlAttribute(AttributeName = "Concepto")]
    [JsonPropertyName("Concepto")]
    public string Concepto { get; set; }

    [XmlAttribute(AttributeName = "Importe")]
    [JsonPropertyName("Importe")]
    public string Importe { get; set; }
}