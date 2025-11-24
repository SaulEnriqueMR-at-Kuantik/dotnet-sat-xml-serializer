using System.Text.Json.Serialization;
using System.Xml.Serialization;
using KpacModels.Shared.Constants;
using Newtonsoft.Json;

namespace KPac.Domain.Mapping.Xml.Retenciones.Complementos.Dividendos;

[XmlRoot("Dividendos", Namespace = Namespaces.Dividendos10)]
public class Dividendos10
{
    [XmlAttribute(AttributeName = "Version")]
    [JsonPropertyName("Version")]
    public string Version { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    [JsonPropertyName("DividendoOUtilidad")]
    [XmlElement(ElementName = "DividOUtil", Namespace = Namespaces.Dividendos10)]
    public DividOUtilDividendos10? DividendoOUtilidad { get; set; }

    public bool ShouldSerializeDividendoOUtilidad() => DividendoOUtilidad is not null;

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    [JsonPropertyName("Remanente")]
    [XmlElement(ElementName = "Remanente", Namespace = Namespaces.Dividendos10)]
    public RemanenteDividendos10? Remanente { get; set; }

    public bool ShouldSerializeRemanente() => Remanente is not null;
}

public class DividOUtilDividendos10
{
    [XmlAttribute(AttributeName = "CveTipDivOUtil")]
    [JsonPropertyName("ClaveTipoDivendoOUtilidad")]
    public string ClaveTipoDivendoOUtilidad { get; set; }

    [XmlAttribute(AttributeName = "MontISRAcredRetMexico")]
    [JsonPropertyName("MontoIsrRetenidoMexico")]
    public string MontoIsrRetenidoMexico { get; set; }

    [XmlAttribute(AttributeName = "MontISRAcredRetExtranjero")]
    [JsonPropertyName("MontoIsrRetenidoExtranjero")]
    public string MontoIsrRetenidoExtranjero { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    [JsonPropertyName("MontoRetencionDividendoExtranjero")]
    [XmlAttribute(AttributeName = "MontRetExtDivExt")]
    public string? MontoRetencionDividendoExtranjero { get; set; }

    [XmlAttribute(AttributeName = "TipoSocDistrDiv")]
    [JsonPropertyName("TipoSociedadDistribucionDividendo")]
    public string TipoSociedadDistribucionDividendo { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    [JsonPropertyName("MontoIsrAcreditableNacional")]
    [XmlAttribute(AttributeName = "MontISRAcredNal")]
    public string? MontoIsrAcreditableNacional { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    [JsonPropertyName("MontoDividendoAcumulableNacional")]
    [XmlAttribute(AttributeName = "MontDivAcumNal")]
    public string? MontoDividendoAcumulableNacional { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    [JsonPropertyName("MontoDividendoAcumulableExtranjero")]
    [XmlAttribute(AttributeName = "MontDivAcumExt")]
    public string? MontoDividendoAcumulableExtranjero { get; set; }
}

public class RemanenteDividendos10
{
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    [JsonPropertyName("ProporcionRemanente")]
    [XmlAttribute(AttributeName = "ProporcionRem")]
    public string? ProporcionRemanente { get; set; }
}