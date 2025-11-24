using System.Text.Json.Serialization;
using System.Xml.Serialization;
using KpacModels.Shared.Constants;

namespace KPac.Domain.Mapping.Xml.Comprobante.Complementos.Impuestoslocales;

[XmlRoot(ElementName = "ImpuestosLocales", Namespace = Namespaces.ImpuestosLocales10)]
public class ImpuestosLocales10
{
    [XmlAttribute(AttributeName = "version")]
    [JsonPropertyName("Version")]
    public string Version { get; set; }

    [XmlAttribute(AttributeName = "TotaldeRetenciones")]
    [JsonPropertyName("TotalRetenciones")]
    public string TotalRetenciones { get; set; }

    [XmlAttribute(AttributeName = "TotaldeTraslados")]
    [JsonPropertyName("TotalTraslados")]
    public string TotalTraslados { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("Retenciones")]
    [XmlElement(ElementName = "RetencionesLocales", Namespace = Namespaces.ImpuestosLocales10)]
    public RetencionesLocalesImpLocales10[]? Retenciones { get; set; }

    public bool ShouldSerializeRetenciones() => Retenciones != null && Retenciones.Length > 0;

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("Traslados")]
    [XmlElement(ElementName = "TrasladosLocales", Namespace = Namespaces.ImpuestosLocales10)]
    public TrasladosLocalesImpLocales10[]? Traslados { get; set; }

    public bool ShouldSerializeTraslados() => Traslados != null && Traslados.Length > 0;
}

public class RetencionesLocalesImpLocales10
{
    [XmlAttribute(AttributeName = "ImpLocRetenido")]
    [JsonPropertyName("Impuesto")]
    public string Impuesto { get; set; }

    [XmlAttribute(AttributeName = "TasadeRetencion")]
    [JsonPropertyName("Tasa")]
    public string Tasa { get; set; }

    [XmlAttribute(AttributeName = "Importe")]
    [JsonPropertyName("Importe")]
    public string Importe { get; set; }
}

public class TrasladosLocalesImpLocales10
{
    [XmlAttribute(AttributeName = "ImpLocTrasladado")]
    [JsonPropertyName("Impuesto")]
    public string Impuesto { get; set; }

    [XmlAttribute(AttributeName = "TasadeTraslado")]
    [JsonPropertyName("Tasa")]
    public string Tasa { get; set; }

    [XmlAttribute(AttributeName = "Importe")]
    [JsonPropertyName("Importe")]
    public string Importe { get; set; }
}