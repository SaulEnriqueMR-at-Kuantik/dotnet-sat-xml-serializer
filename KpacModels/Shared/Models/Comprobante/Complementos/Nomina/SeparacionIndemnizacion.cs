using System.Text.Json.Serialization;
using System.Xml.Serialization;

namespace KpacModels.Shared.Models.Comprobante.Complementos.Nomina;

public class SeparacionIndemnizacion
{
    [XmlAttribute(AttributeName = "TotalPagado")]
    [JsonPropertyName("TotalPagado")]
    public string TotalPagado { get; set; }

    [XmlAttribute(AttributeName = "NumAñosServicio")]
    [JsonPropertyName("NumeroAniosServicio")]
    public string NumeroAniosServicio { get; set; }

    [XmlAttribute(AttributeName = "UltimoSueldoMensOrd")]
    [JsonPropertyName("UltimoSueldoMensualOrdinario")]
    public string UltimoSueldoMensualOrdinario { get; set; }

    [XmlAttribute(AttributeName = "IngresoAcumulable")]
    [JsonPropertyName("IngresoAcumulable")]
    public string IngresoAcumulable { get; set; }

    [XmlAttribute(AttributeName = "IngresoNoAcumulable")]
    [JsonPropertyName("IngresoNoAcumulable")]
    public string IngresoNoAcumulable { get; set; }
}