using System.Text.Json.Serialization;
using System.Xml.Serialization;
using KpacModels.Shared.XmlProcessing.Formatter.Interface;

namespace KpacModels.Shared.Models.Comprobante;

public class Receptor
{
    [XmlAttribute(AttributeName = "Rfc")]
    [JsonPropertyName("Rfc")]
    public string Rfc { get; set; }

    [XmlAttribute(AttributeName = "Nombre")]
    [JsonPropertyName("Nombre")]
    public string Nombre { get; set; }

    [XmlAttribute(AttributeName = "DomicilioFiscalReceptor")]
    [JsonPropertyName("DomicilioFiscal")]
    public string DomicilioFiscal { get; set; }

    [XmlAttribute(AttributeName = "RegimenFiscalReceptor")]
    [JsonPropertyName("RegimenFiscal")]
    public string RegimenFiscal { get; set; }

    [XmlAttribute(AttributeName = "UsoCFDI")]
    [JsonPropertyName("UsoCfdi")]
    public string UsoCfdi { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("ResidenciaFiscal")]
    [XmlAttribute(AttributeName = "ResidenciaFiscal")]
    public string? ResidenciaFiscal { set; get; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("NumRegIdTrib")]
    [XmlAttribute(AttributeName = "NumRegIdTrib")]
    public string? NumRegIdTrib { set; get; }
    

    public void Accept(IVisitorFormatter visitor)
    {
        visitor.Visit(this);
    }
}