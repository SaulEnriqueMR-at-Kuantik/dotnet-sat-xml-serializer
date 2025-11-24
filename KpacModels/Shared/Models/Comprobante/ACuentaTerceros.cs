using System.Text.Json.Serialization;
using System.Xml.Serialization;

namespace KpacModels.Shared.Models.Comprobante;

public class ACuentaTerceros
{
    [XmlAttribute(AttributeName = "RfcACuentaTerceros")]
    [JsonPropertyName("Rfc")]
    public string Rfc { get; set; }

    [XmlAttribute(AttributeName = "NombreACuentaTerceros")]
    [JsonPropertyName("Nombre")]
    public string Nombre { get; set; }
    
    [XmlAttribute(AttributeName = "RegimenFiscalACuentaTerceros")]
    [JsonPropertyName("RegimenFiscal")]
    public string RegimenFiscal { get; set; }
    
    [XmlAttribute(AttributeName = "DomicilioFiscalACuentaTerceros")]
    [JsonPropertyName("DomicilioFiscal")]
    public string DomicilioFiscal { get; set; }
}