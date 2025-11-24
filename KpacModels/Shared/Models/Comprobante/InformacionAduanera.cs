using System.Text.Json.Serialization;
using System.Xml.Serialization;

namespace KpacModels.Shared.Models.Comprobante;

public class InformacionAduanera
{
    // [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    // [XmlIgnore]
    // public string? IdConcepto { get; set; }
    
    // [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    // [XmlIgnore]
    // public string? ConsecutivoParte { get; set; }
    //
    // [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    // [XmlIgnore]
    // public string? ClaveProductoServicio { get; set; }
    
    [XmlAttribute(AttributeName = "NumeroPedimento")]
    [JsonPropertyName("NumeroPedimento")]
    public string NumeroPedimento { get; set; }
    
    // [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    // [JsonPropertyName("Fecha")]
    // [XmlIgnore]
    // public string? Fecha { get; set; }
    //
    // [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    // [XmlIgnore]
    // public string? Aduana { get; set; }
}