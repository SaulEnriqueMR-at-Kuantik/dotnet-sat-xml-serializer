using System.Text.Json.Serialization;
using System.Xml.Serialization;
using KPac.Domain.Mapping.Xml.Comprobante;
using KpacModels.Shared.Constants;
using KpacModels.Shared.XmlProcessing.Formatter.Interface;
using KpacModels.Shared.XmlProcessing.Validator.Interface;

namespace KpacModels.Shared.Models.Comprobante;

public class CfdiRelacionado
{
    [XmlElement("CfdiRelacionado", Namespace = Namespaces.CfdiLocation)]
    [JsonPropertyName("UuidsRelacionados")]
    public List<UuidRelacionado> UuidsRelacionados { get; set; }
    
    public async Task Accept(IVisitor visitor, int noCfdi)
    {
        await visitor.Visit(this, noCfdi);
    }
    
    public void Accept(IVisitorFormatter visitor, int noCfdi)
    {
        visitor.Visit(this, noCfdi);
    }
    
    [XmlAttribute(AttributeName = "TipoRelacion")]
    [JsonPropertyName("TipoRelacion")]
    public string TipoRelacion { get; set; }
}