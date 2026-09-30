using System.Text.Json.Serialization;
using System.Xml.Serialization;

namespace KpacModels.Shared.Models.Comprobante.Complementos.Pagos;

public class TrasladoDR
{

    [XmlAttribute(AttributeName = "BaseDR")]
    [JsonPropertyName("Base")]
    public string Base { get; set; }

    [XmlAttribute(AttributeName = "ImpuestoDR")]
    [JsonPropertyName("Impuesto")]
    public string Impuesto { get; set; }

    [XmlAttribute(AttributeName = "TipoFactorDR")]
    [JsonPropertyName("TipoFactor")]
    public string TipoFactor { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("TasaOCuota")]
    [XmlAttribute(AttributeName = "TasaOCuotaDR")]
    public string? TasaOCuota { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("Importe")]
    [XmlAttribute(AttributeName = "ImporteDR")]
    public string? Importe { get; set; }
    
}