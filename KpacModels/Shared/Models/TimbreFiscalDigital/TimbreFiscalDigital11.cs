using System.Text.Json.Serialization;
using System.Xml.Serialization;
using KpacModels.Shared.Models.Constants;

namespace KpacModels.Shared.Models.TimbreFiscalDigital;

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
    
    [XmlAttribute(AttributeName = "SelloCFD")]
    [JsonPropertyName("SelloCfd")]
    public string SelloCfd {  set; get; }
}