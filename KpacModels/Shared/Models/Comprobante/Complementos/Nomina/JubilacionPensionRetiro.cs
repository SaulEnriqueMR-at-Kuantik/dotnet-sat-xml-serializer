using System.Text.Json.Serialization;
using System.Xml.Serialization;

namespace KpacModels.Shared.Models.Comprobante.Complementos.Nomina;

public class JubilacionPensionRetiro
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("TotalUnaExhibicion")]
    [XmlAttribute(AttributeName = "TotalUnaExhibicion")]
    public string? TotalUnaExhibicion { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("TotalParcialidad")]
    [XmlAttribute(AttributeName = "TotalParcialidad")]
    public string? TotalParcialidad { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("MontoDiario")]
    [XmlAttribute(AttributeName = "MontoDiario")]
    public string? MontoDiario { get; set; }

    [XmlAttribute(AttributeName = "IngresoAcumulable")]
    [JsonPropertyName("IngresoAcumulable")]
    public string IngresoAcumulable { get; set; }

    [XmlAttribute(AttributeName = "IngresoNoAcumulable")]
    [JsonPropertyName("IngresoNoAcumulable")]
    public string IngresoNoAcumulable { get; set; }
}