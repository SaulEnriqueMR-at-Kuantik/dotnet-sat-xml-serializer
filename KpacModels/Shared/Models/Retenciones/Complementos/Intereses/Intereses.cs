using System.Text.Json.Serialization;
using System.Xml.Serialization;
using KpacModels.Shared.Constants;
using Newtonsoft.Json;

namespace KPac.Domain.Mapping.Xml.Retenciones.Complementos.Intereses;

[XmlRoot(ElementName = "Intereses", Namespace = Namespaces.Intereses10)]
public class Intereses10
{
    [XmlAttribute(AttributeName = "Version")]
    [JsonPropertyName("Version")]
    public string Version { get; set; }

    [XmlAttribute(AttributeName = "SistFinanciero")]
    [JsonPropertyName("SistemaFinanciero")]
    public string SistemaFinanciero { get; set; }

    [XmlAttribute(AttributeName = "RetiroAORESRetInt")]
    [JsonPropertyName("RetiroIntereses")]
    public string RetiroIntereses { get; set; }

    [XmlAttribute(AttributeName = "OperFinancDerivad")]
    [JsonPropertyName("OperacionFinancieraDerivada")]
    public string OperacionFinancieraDerivada { get; set; }

    [XmlAttribute(AttributeName = "MontIntNominal")]
    [JsonPropertyName("MontoInteresNominal")]
    public string MontoInteresNominal { get; set; }

    [XmlAttribute(AttributeName = "MontIntReal")]
    [JsonPropertyName("MontoInteresReal")]
    public string MontoInteresReal { get; set; }

    [XmlAttribute(AttributeName = "Perdida")]
    [JsonPropertyName("Perdida")]
    public string Perdida { get; set; }
}