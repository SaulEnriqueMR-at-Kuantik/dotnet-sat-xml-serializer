using System.Text.Json.Serialization;
using System.Xml.Serialization;
using KpacModels.Shared.Constants;
using Newtonsoft.Json;

namespace KPac.Domain.Mapping.Xml.Retenciones.Complementos.PagosAExtranjeros;

[XmlRoot(ElementName = "Pagosaextranjeros", Namespace = Namespaces.PagosAExtranjeros)]
public class PagosAExtranjeros10
{
    [XmlAttribute(AttributeName = "Version")]
    [JsonPropertyName("Version")]
    public string Version { get; set; }

    [XmlAttribute(AttributeName = "EsBenefEfectDelCobro")]
    [JsonPropertyName("EsBenefEfectDelCobro")]
    public string EsBenefEfectDelCobro { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    [JsonPropertyName("NoBeneficiario")]
    [XmlElement(ElementName = "NoBeneficiario", Namespace = Namespaces.PagosAExtranjeros)]
    public NoBeneficiario? NoBeneficiario { get; set; }

    public bool ShouldSerializeNoBeneficiario() => NoBeneficiario != null;

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    [JsonPropertyName("Beneficiario")]
    [XmlElement(ElementName = "Beneficiario", Namespace = Namespaces.PagosAExtranjeros)]
    public Beneficiario? Beneficiario { get; set; }

    public bool ShouldSerializeBeneficiario() => Beneficiario != null;
}