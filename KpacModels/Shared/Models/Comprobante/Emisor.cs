using System.Text.Json.Serialization;
using System.Xml.Serialization;
using KpacModels.Shared.XmlProcessing.Formatter.Interface;

namespace KpacModels.Shared.Models.Comprobante;

public class Emisor
{
    [XmlAttribute(AttributeName = "Rfc")]
    [JsonPropertyName("Rfc")]
    public string Rfc { get; set; }
    
    [XmlAttribute(AttributeName = "Nombre")]
    [JsonPropertyName("Nombre")]
    public string Nombre { get; set; }
    
    [XmlAttribute(AttributeName = "RegimenFiscal")]
    [JsonPropertyName("RegimenFiscal")]
    public string? RegimenFiscal { get; set; }
    
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("FacAtrAdquirente")]
    [XmlAttribute(AttributeName = "FacAtrAdquirente")]
    public string? FacAtrAdquirent { set; get; }
    
    public void Accept(IVisitorFormatter visitor)
    {
        visitor.Visit(this);
    }
}