using System.Text.Json.Serialization;
using System.Xml.Serialization;

namespace KpacModels.Shared.Models.Retenciones.Complementos.PagosAExtranjeros;

public class Beneficiario
{
    [JsonPropertyName("Rfc")]
    [XmlAttribute(AttributeName = "RFC")]
    public string Rfc { get; set; }

    [JsonPropertyName("Curp")]
    [XmlAttribute(AttributeName = "CURP")]
    public string Curp { get; set; }

    [JsonPropertyName("Nombre")]
    [XmlAttribute(AttributeName = "NomDenRazSocB")]
    public string Nombre { get; set; }

    [JsonPropertyName("ConceptoPago")]
    [XmlAttribute(AttributeName = "ConceptoPago")]
    public string ConceptoPago { get; set; }

    [JsonPropertyName("DescripcionConcepto")]
    [XmlAttribute(AttributeName = "DescripcionConcepto")]
    public string DescripcionConcepto { get; set; }
}