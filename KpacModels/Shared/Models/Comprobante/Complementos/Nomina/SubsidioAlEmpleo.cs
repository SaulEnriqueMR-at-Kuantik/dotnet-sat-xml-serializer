using System.Text.Json.Serialization;
using System.Xml.Serialization;

namespace KpacModels.Shared.Models.Comprobante.Complementos.Nomina;

public class SubsidioAlEmpleo
{
    [XmlAttribute(AttributeName = "SubsidioCausado")]
    [JsonPropertyName("SubsidioCausado")]
    public string SubsidioCausado { get; set; }
    
}