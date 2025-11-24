using System.Text.Json.Serialization;
using System.Xml.Serialization;
using KpacModels.Shared.Models.Constants;

namespace KpacModels.Shared.Models.Comprobante.Complementos.Nomina;

public class Percepciones
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("TotalSueldos")]
    [XmlAttribute(AttributeName = "TotalSueldos")]
    public string? TotalSueldos { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("AttributeName")]
    [XmlAttribute(AttributeName = "TotalSeparacionIndemnizacion")]
    public string? TotalSeparacionIndemnizacion { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("TotalJubilacionPensionRetiro")]
    [XmlAttribute(AttributeName = "TotalJubilacionPensionRetiro")]
    public string? TotalJubilacionPensionRetiro { get; set; }

    [XmlAttribute(AttributeName = "TotalGravado")]
    [JsonPropertyName("TotalGravado")]
    public string TotalGravado { get; set; }

    [XmlAttribute(AttributeName = "TotalExento")]
    [JsonPropertyName("TotalExento")]
    public string TotalExento { get; set; }

    [XmlElement(ElementName = "Percepcion", Namespace = Namespaces.Nomina12)]
    [JsonPropertyName("Percepcion")]
    public List<Percepcion>? Percepcion { get; set; }

    public bool ShouldSerializePercepcion() => Percepcion != null && Percepcion.Count > 0;

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("JubilacionPensionRetiro")]
    [XmlElement(ElementName = "JubilacionPensionRetiro", Namespace = Namespaces.Nomina12)]
    public JubilacionPensionRetiro? JubilacionPensionRetiro { get; set; }

    public bool ShouldSerializeJubilacionPensionRetiro() => JubilacionPensionRetiro != null;

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("SeparacionIndemnizacion")]
    [XmlElement(ElementName = "SeparacionIndemnizacion", Namespace = Namespaces.Nomina12)]
    public SeparacionIndemnizacion? SeparacionIndemnizacion { get; set; }

    public bool ShouldSerializeSeparacionIndemnizacion() => SeparacionIndemnizacion != null;
}