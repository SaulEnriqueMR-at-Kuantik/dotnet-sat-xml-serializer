using System.Text.Json.Serialization;
using System.Xml.Serialization;
using KpacModels.Shared.Models.Constants;

namespace KpacModels.Shared.Models.Comprobante.Complementos.Pagos;

public class ImpuestosP
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("Retenciones")]
    [XmlArray("RetencionesP", Namespace = Namespaces.Pagos20)]
    [XmlArrayItem("RetencionP", Namespace = Namespaces.Pagos20)]
    public List<RetencionP>? Retenciones { get; set; }

    public bool ShouldSerializeRetenciones() => Retenciones != null && Retenciones.Count > 0;


    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("Traslados")]
    [XmlArray("TrasladosP", Namespace = Namespaces.Pagos20)]
    [XmlArrayItem(ElementName = "TrasladoP", Namespace = Namespaces.Pagos20)]
    public List<TrasladoP>? Traslados { get; set; }

    public bool ShouldSerializeTraslados() => Traslados != null && Traslados.Count > 0;

}