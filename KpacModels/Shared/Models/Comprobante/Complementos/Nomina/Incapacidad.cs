using System.Text.Json.Serialization;
using System.Xml.Serialization;

namespace KpacModels.Shared.Models.Comprobante.Complementos.Nomina;

public class Incapacidad
{
    [XmlAttribute(AttributeName = "DiasIncapacidad")]
    [JsonPropertyName("Dias")]
    public string Dias { get; set; }

    [XmlAttribute(AttributeName = "TipoIncapacidad")]
    [JsonPropertyName("Tipo")]
    public string Tipo { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("Importe")]
    [XmlAttribute(AttributeName = "ImporteMonetario")]
    public string? Importe { get; set; }
}