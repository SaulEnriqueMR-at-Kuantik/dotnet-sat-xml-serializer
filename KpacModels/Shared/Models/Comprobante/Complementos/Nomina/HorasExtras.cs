using System.Text.Json.Serialization;
using System.Xml.Serialization;

namespace KpacModels.Shared.Models.Comprobante.Complementos.Nomina;

public class HorasExtras
{
    // [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    // [XmlIgnore]
    // public string? ClavePercepcion { get; set; }
    
    [XmlAttribute(AttributeName = "Dias")]
    [JsonPropertyName("Dias")]
    public string Dias { get; set; }

    [XmlAttribute(AttributeName = "TipoHoras")]
    [JsonPropertyName("TipoHoras")]
    public string TipoHoras { get; set; }

    [XmlAttribute(AttributeName = "HorasExtra")]
    [JsonPropertyName("HorasExtra")]
    public string HorasExtra { get; set; }

    [XmlAttribute(AttributeName = "ImportePagado")]
    [JsonPropertyName("ImportePagado")]
    public string ImportePagado { get; set; }
}