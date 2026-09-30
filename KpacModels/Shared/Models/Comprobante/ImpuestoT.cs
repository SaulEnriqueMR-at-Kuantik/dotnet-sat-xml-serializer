using System.Text.Json.Serialization;
using System.Xml.Serialization;

namespace KpacModels.Shared.Models.Comprobante;

public class ImpuestoT
{

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("Base")]
    [XmlAttribute(AttributeName = "Base")]
    public string? Base { get; set; }

    [XmlAttribute(AttributeName = "Impuesto")]
    [JsonPropertyName("Impuesto")]
    public string Impuesto { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    [JsonPropertyName("TipoFactor")]
    [XmlAttribute(AttributeName = "TipoFactor")]
    public string? TipoFactor { get; set; }
    
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("TasaOCuota")]
    [XmlAttribute(AttributeName = "TasaOCuota")]
    public string? TasaOCuota { get; set; }


    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("Importe")]
    [XmlAttribute(AttributeName = "Importe")]
    public string? Importe { get; set; }
    
}