using System.Text.Json.Serialization;
using System.Xml.Serialization;
using KpacModels.Shared.Models.Constants;

namespace KpacModels.Shared.Models.Comprobante;

public class Parte
{
    // [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    // [XmlIgnore]
    // public string? IdConcepto { get; set; }
    //
    // [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    // [XmlIgnore]
    // public string? ConsecutivoParte { get; set; }

    [XmlAttribute(AttributeName = "ClaveProdServ")]
    [JsonPropertyName("ClaveProdServ")]
    public string ClaveProdServ { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("NoIdentificacion")]
    [XmlAttribute(AttributeName = "NoIdentificacion")]
    public string? NoIdentificacion { get; set; }

    [XmlAttribute(AttributeName = "Cantidad")]
    [JsonPropertyName("Cantidad")]
    public string Cantidad { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("Unidad")]
    [XmlAttribute(AttributeName = "Unidad")]
    public string? Unidad { get; set; }

    [XmlAttribute(AttributeName = "Descripcion")]
    [JsonPropertyName("Descripcion")]
    public string Descripcion { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("ValorUnitario")]
    [XmlAttribute(AttributeName = "ValorUnitario")]
    public string ValorUnitario { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("Importe")]
    [XmlAttribute(AttributeName = "Importe")]
    public string Importe { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("InformacionAduanera")]
    [XmlElement(ElementName = "InformacionAduanera", Namespace = Namespaces.CfdiLocation)]
    public List<InformacionAduanera>? InformacionAduanera { set; get; }

    public bool ShouldSerializeInformacionAduanera() => InformacionAduanera != null && InformacionAduanera.Count > 0;
}