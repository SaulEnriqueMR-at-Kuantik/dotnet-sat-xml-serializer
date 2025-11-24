using System.Text.Json.Serialization;
using System.Xml.Serialization;
using KpacModels.Shared.Constants;

namespace KPac.Domain.Mapping.Xml.TimbreFiscalDigital;

[XmlRoot(ElementName = "TimbreFiscalDigital", Namespace = Namespaces.TfdLocation)]
public class TimbreFiscalDigital11
{
    [XmlAttribute(AttributeName = "Version")]
    [JsonPropertyName("Version")]
    public string Version { get; set; }
    
    [XmlAttribute(AttributeName = "UUID")]
    [JsonPropertyName("Uuid")]
    public string Uuid { set; get; }
    
    [XmlAttribute(AttributeName = "FechaTimbrado")]
    [JsonPropertyName("FechaTimbrado")]
    public DateTime FechaTimbrado { get; set; }
    
    [XmlAttribute(AttributeName = "RfcProvCertif")]
    [JsonPropertyName("RfcProvCertif")]
    public string RfcProvCertif { set; get; }
    
    [XmlAttribute(AttributeName = "NoCertificadoSAT")]
    [JsonPropertyName("NoCertificadoSat")]
    public string NoCertificadoSat { set; get; }
    
    [XmlAttribute(AttributeName = "SelloSAT")]
    [JsonPropertyName("SelloSat")]
    public string SelloSat { set; get; }
}