using System.Text.Json.Serialization;
using System.Xml.Serialization;
using KpacModels.Shared.Models.Constants;

namespace KpacModels.Shared.Models.Comprobante.Complementos.Pagos;

public class ImpuestosDR
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("Retenciones")]
    [XmlArray("RetencionesDR", Namespace = Namespaces.Pagos20)]
    [XmlArrayItem(ElementName = "RetencionDR", Namespace = Namespaces.Pagos20)]
    public List<RetencionDR>? Retenciones { get; set; }

    public bool ShouldSerializeRetenciones() => Retenciones is { Count: > 0 };

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("Traslados")]
    [XmlArray("TrasladosDR", Namespace = Namespaces.Pagos20)]
    [XmlArrayItem(ElementName = "TrasladoDR", Namespace = Namespaces.Pagos20)]
    public List<TrasladoDR>? Traslados { get; set; }

    public bool ShouldSerializeTraslados() => Traslados is { Count: > 0 };
}