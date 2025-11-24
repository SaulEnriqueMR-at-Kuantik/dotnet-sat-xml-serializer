using System.Text.Json.Serialization;
using System.Xml.Serialization;
using KPac.Domain.Mapping.Xml.Comprobante;
using KpacModels.Shared.Models.Constants;

namespace KpacModels.Shared.Models.Comprobante;

public class ConceptoImpuestos
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("Traslados")]
    [XmlArray(ElementName = "Traslados", Namespace = Namespaces.CfdiLocation)]
    [XmlArrayItem(ElementName = "Traslado", Namespace = Namespaces.CfdiLocation)]
    public List<ImpuestoT>? Traslados { get; set; }
    
    public bool ShouldSerializeTraslados() => Traslados != null && Traslados.Count > 0;
    
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("Retenciones")]
    [XmlArray(ElementName = "Retenciones", Namespace = Namespaces.CfdiLocation)]
    [XmlArrayItem(ElementName = "Retencion", Namespace = Namespaces.CfdiLocation)]
    public List<ImpuestoT>? Retenciones { get; set; }
    
    public bool ShouldSerializeRetenciones() => Retenciones != null && Retenciones.Count > 0;

    public void Clean()
    {
        Traslados?.Clear();
        Retenciones?.Clear();
    }
}