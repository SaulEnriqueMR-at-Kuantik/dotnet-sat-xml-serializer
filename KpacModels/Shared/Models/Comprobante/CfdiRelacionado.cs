using System.Text.Json.Serialization;
using System.Xml.Serialization;
using KpacModels.Shared.Models.Constants;
using KpacModels.Shared.XmlProcessing.Formatter.Interface;

namespace KpacModels.Shared.Models.Comprobante;

public class CfdiRelacionado
{
    [XmlElement("CfdiRelacionado", Namespace = Namespaces.CfdiLocation)]
    [JsonPropertyName("UuidsRelacionados")]
    public List<UuidRelacionado> UuidsRelacionados { get; set; }
    
    public void Accept(IVisitorFormatter visitor, int noCfdi)
    {
        visitor.Visit(this, noCfdi);
    }
    
    [XmlAttribute(AttributeName = "TipoRelacion")]
    [JsonPropertyName("TipoRelacion")]
    public string TipoRelacion { get; set; }
}