using System.Text.Json.Serialization;
using System.Xml.Serialization;
using KpacModels.Shared.Models.Constants;
using KpacModels.Shared.Models.Retenciones.Complementos.Dividendos;
using KpacModels.Shared.Models.Retenciones.Complementos.EnajenacionAcciones;
using KpacModels.Shared.Models.Retenciones.Complementos.Intereses;
using KpacModels.Shared.Models.Retenciones.Complementos.PagosAExtranjeros;
using KpacModels.Shared.Models.TimbreFiscalDigital;
using Newtonsoft.Json;

namespace KpacModels.Shared.Models.Retenciones;

[XmlRoot(ElementName = "Retenciones", Namespace = Namespaces.Retenciones20)]
public class Retenciones20
{
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    [XmlIgnore]
    public string? Retencion { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    [XmlIgnore]
    public string? Serie { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    [XmlIgnore]
    public string? Comentario { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    [JsonPropertyName("Version")]
    [XmlAttribute(AttributeName = "Version")]
    public string Version { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    [JsonPropertyName("Folio")]
    [XmlAttribute(AttributeName = "FolioInt")]
    public string? Folio { get; set; }

    [XmlAttribute(AttributeName = "Sello")]
    [JsonPropertyName("Sello")]
    public string? Sello { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    [JsonPropertyName("NoCertificado")]
    [XmlAttribute(AttributeName = "NoCertificado")]
    public string? NoCertificado { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    [JsonPropertyName("Certificado")]
    [XmlAttribute(AttributeName = "Certificado")]
    public string? Certificado { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    [JsonPropertyName("Fecha")]
    [XmlAttribute(AttributeName = "FechaExp")]
    public string Fecha { get; set; }

    [JsonPropertyName("LugarExpedicion")]
    [XmlAttribute(AttributeName = "LugarExpRetenc")]
    public string LugarExpedicion { get; set; }

    [JsonPropertyName("ClaveRetencion")]
    [XmlAttribute(AttributeName = "CveRetenc")]
    public string ClaveRetencion { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    [JsonPropertyName("DescripcionRetencion")]
    [XmlAttribute(AttributeName = "DescRetenc")]
    public string? DescripcionRetencion { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    [JsonPropertyName("RetencionRelacionada")]
    [XmlElement(ElementName = "CfdiRetenRelacionados", Namespace = Namespaces.Retenciones20)]
    public RetencionRelacionada20? RetencionRelacionada { get; set; }

    public bool ShouldSerializeRetencionRelacionada() => RetencionRelacionada != null;

    [XmlElement(ElementName = "Emisor", Namespace = Namespaces.Retenciones20)]
    [JsonPropertyName("Emisor")]
    public Emisor20 Emisor { get; set; }

    [XmlElement(ElementName = "Receptor", Namespace = Namespaces.Retenciones20)]
    [JsonPropertyName("Receptor")]
    public Receptor20 Receptor { get; set; }

    [XmlElement(ElementName = "Periodo", Namespace = Namespaces.Retenciones20)]
    [JsonPropertyName("Periodo")]
    public Periodo20 Periodo { get; set; }

    [XmlElement(ElementName = "Totales", Namespace = Namespaces.Retenciones20)]
    [JsonPropertyName("Totales")]
    public Totales20 Totales { get; set; }

    [XmlElement(ElementName = "Complemento", Namespace = Namespaces.Retenciones20)]
    [JsonPropertyName("Complemento")]
    public Complementos20? Complemento { get; set; }
}

public class RetencionRelacionada20
{
    [XmlAttribute(AttributeName = "TipoRelacion")]
    [JsonPropertyName("TipoRelacion")]
    public string TipoRelacion { get; set; }

    [XmlAttribute(AttributeName = "UUID")]
    [JsonPropertyName("Uuid")]
    public string Uuid { get; set; }
}

public class Emisor20
{
    [XmlAttribute(AttributeName = "RfcE")]
    [JsonPropertyName("Rfc")]
    public string Rfc { get; set; }

    [XmlAttribute(AttributeName = "NomDenRazSocE")]
    [JsonPropertyName("Nombre")]
    public string Nombre { get; set; }

    [XmlAttribute(AttributeName = "RegimenFiscalE")]
    [JsonPropertyName("RegimenFiscal")]
    public string RegimenFiscal { get; set; }
}

public class Receptor20
{
    [XmlAttribute(AttributeName = "NacionalidadR")]
    [JsonPropertyName("Nacionalidad")]
    public string Nacionalidad { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    [XmlElement(ElementName = "Nacional", Namespace = Namespaces.Retenciones20)]
    [JsonPropertyName("Nacional")]
    public Nacional20? Nacional { get; set; }

    public bool ShouldSerializeNacional() => Nacional is not null;

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    [XmlElement(ElementName = "Extranjero", Namespace = Namespaces.Retenciones20)]
    [JsonPropertyName("Extranjero")]
    public Extranjero20? Extranjero { get; set; }

    public bool ShouldSerializeExtranjero() => Extranjero is not null;
}

public class Nacional20
{
    [XmlAttribute(AttributeName = "RfcR")]
    [JsonPropertyName("Rfc")]
    public string Rfc { get; set; }

    [XmlAttribute(AttributeName = "NomDenRazSocR")]
    [JsonPropertyName("Nombre")]
    public string Nombre { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    [XmlAttribute(AttributeName = "CurpR")]
    [JsonPropertyName("Curp")]
    public string? Curp { get; set; }

    [XmlAttribute(AttributeName = "DomicilioFiscalR")]
    [JsonPropertyName("DomicilioFiscal")]
    public string DomicilioFiscal { get; set; }
}

public class Extranjero20
{
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    [XmlAttribute(AttributeName = "NumRegIdTribR")]
    [JsonPropertyName("NumRegIdTrib")]
    public string? NumRegIdTrib { get; set; }

    [XmlAttribute(AttributeName = "NomDenRazSocR")]
    [JsonPropertyName("Nombre")]
    public string Nombre { get; set; }
}

public class Periodo20
{
    [XmlAttribute(AttributeName = "MesIni")]
    [JsonPropertyName("MesInicio")]
    public string MesInicio { get; set; }

    [XmlAttribute(AttributeName = "MesFin")]
    [JsonPropertyName("MesFin")]
    public string MesFin { get; set; }

    [XmlAttribute(AttributeName = "Ejercicio")]
    [JsonPropertyName("Ejercicio")]
    public string Ejercicio { get; set; }
}

public class Totales20
{
    [XmlAttribute(AttributeName = "MontoTotOperacion")]
    [JsonPropertyName("MontoTotalOperacion")]
    public string MontoTotalOperacion { get; set; }

    [XmlAttribute(AttributeName = "MontoTotGrav")]
    [JsonPropertyName("MontoTotalGravado")]
    public string MontoTotalGravado { get; set; }

    [XmlAttribute(AttributeName = "MontoTotExent")]
    [JsonPropertyName("MontoTotalExento")]
    public string MontoTotalExento { get; set; }

    [XmlAttribute(AttributeName = "MontoTotRet")]
    [JsonPropertyName("MontoTotalRetenido")]
    public string MontoTotalRetenido { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    [JsonPropertyName("UtilidadBimestral")]
    [XmlAttribute(AttributeName = "UtilidadBimestral")]
    public string? UtilidadBimestral { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    [JsonPropertyName("IsrCorrespondiente")]
    [XmlAttribute(AttributeName = "ISRCorrespondiente")]
    public string? IsrCorrespondiente { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    [JsonPropertyName("Impuestos")]
    [XmlElement(ElementName = "ImpRetenidos", Namespace = Namespaces.Retenciones20)]
    public ImpuestosRetenido20[]? Impuestos { get; set; }

    public bool ShouldSerializeImpuestos() => Impuestos != null && Impuestos.Length > 0;
}

public class ImpuestosRetenido20
{
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    [JsonPropertyName("Base")]
    [XmlAttribute(AttributeName = "BaseRet")]
    public string? Base { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    [JsonPropertyName("Impuesto")]
    [XmlAttribute(AttributeName = "ImpuestoRet")]
    public string? Impuesto { get; set; }

    [XmlAttribute(AttributeName = "MontoRet")]
    [JsonPropertyName("Monto")]
    public string Monto { get; set; }

    [XmlAttribute(AttributeName = "TipoPagoRet")]
    [JsonPropertyName("TipoPago")]
    public string TipoPago { get; set; }
}

public class Complementos20
{
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    [JsonPropertyName("TimbreFiscalDigital")]
    [XmlElement(ElementName = "TimbreFiscalDigital", Namespace = Namespaces.TfdLocation)]
    public TimbreFiscalDigital11? TimbreFiscalDigital { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    [JsonPropertyName("Dividendos")]
    [XmlElement(ElementName = "Dividendos", Namespace = Namespaces.Dividendos10)]
    public List<Dividendos10>? Dividendos { get; set; }

    public bool ShouldSerializeDividendos() => Dividendos?.Count > 0;

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    [JsonPropertyName("EnajenacionDeAcciones")]
    [XmlElement(ElementName = "EnajenaciondeAcciones", Namespace = Namespaces.EnajenacionAcciones10)]
    public List<EnajenacionAcciones10>? EnajenacionDeAcciones { get; set; }

    public bool ShouldSerializeEnajenacionDeAcciones() => EnajenacionDeAcciones?.Count > 0;

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    [JsonPropertyName("Intereses")]
    [XmlElement(ElementName = "Intereses", Namespace = Namespaces.Intereses10)]
    public List<Intereses10>? Intereses { get; set; }

    public bool ShouldSerializeIntereses() => Intereses?.Count > 0;

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    [JsonPropertyName("PagosAExtranjeros")]
    [XmlElement(ElementName = "Pagosaextranjeros", Namespace = Namespaces.PagosAExtranjeros)]
    public List<PagosAExtranjeros10>? PagosAExtranjeros { get; set; }

    public bool ShouldSerializePagosAExtranjeros() => PagosAExtranjeros?.Count > 0;
}