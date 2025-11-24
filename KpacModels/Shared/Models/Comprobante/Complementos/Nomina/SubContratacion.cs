using System.Text.Json.Serialization;
using System.Xml.Serialization;

namespace KpacModels.Shared.Models.Comprobante.Complementos.Nomina;

public class SubContratacion
{
    [XmlAttribute(AttributeName = "RfcLabora")]
    [JsonPropertyName("RfcLabora")]
    public string RfcLabora { get; set; }

    [XmlAttribute(AttributeName = "PorcentajeTiempo")]
    [JsonPropertyName("PorcentajeTiempo")]
    public string PorcentajeTiempo { get; set; }
}