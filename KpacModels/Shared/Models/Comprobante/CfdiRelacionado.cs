using System.Text.Json.Serialization;
using System.Xml.Serialization;
using KpacModels.Shared.Models.Constants;

namespace KpacModels.Shared.Models.Comprobante;

public class CfdiRelacionado
{
    [XmlElement("CfdiRelacionado", Namespace = Namespaces.CfdiLocation)]
    [JsonPropertyName("UuidsRelacionados")]
    public List<UuidRelacionado> UuidsRelacionados { get; set; }
    
    [XmlAttribute(AttributeName = "TipoRelacion")]
    [JsonPropertyName("TipoRelacion")]
    public string TipoRelacion { get; set; }
}