using System.Text.Json.Serialization;
using System.Xml.Serialization;
using KPac.Domain.Mapping.Xml.Comprobante;
using KpacModels.Shared.Constants;
using KpacModels.Shared.XmlProcessing.Formatter.Interface;
using KpacModels.Shared.XmlProcessing.Validator.Interface;

namespace KpacModels.Shared.Models.Comprobante;

public class Impuestos
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("Retenciones")]
    [XmlArray(ElementName = "Retenciones", Namespace = Namespaces.CfdiLocation)]
    [XmlArrayItem(ElementName = "Retencion", Namespace = Namespaces.CfdiLocation)]
    public List<ImpuestoR>? Retenciones { get; set; }

    public bool ShouldSerializeRetenciones() => Retenciones != null && Retenciones.Count > 0;

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("Traslados")]
    [XmlArray(ElementName = "Traslados", Namespace = Namespaces.CfdiLocation)]
    [XmlArrayItem(ElementName = "Traslado", Namespace = Namespaces.CfdiLocation)]
    public List<ImpuestoT>? Traslados { get; set; }

    public bool ShouldSerializeTraslados() => Traslados != null && Traslados.Count > 0;

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("TotalImpuestosTrasladados")]
    [XmlAttribute(AttributeName = "TotalImpuestosTrasladados")]
    public string? TotalImpuestosTrasladados { get; set; }

    public bool ShouldSerializeTotalImpuestosTrasladados()
    {
        return !string.IsNullOrWhiteSpace(TotalImpuestosTrasladados);
    }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("TotalImpuestosRetenidos")]
    [XmlAttribute(AttributeName = "TotalImpuestosRetenidos")]
    public string? TotalImpuestosRetenidos { get; set; }

    public bool ShouldSerializeTotalImpuestosRetenidos()
    {
        return !string.IsNullOrWhiteSpace(TotalImpuestosRetenidos);
    }

    public void Accept(IVisitor visitor)
    {
        visitor.Visit(this);
    }

    public void Accept(IVisitorFormatter visitor)
    {
        visitor.Visit(this);
    }

    public void Clear()
    {
        Retenciones?.Clear();
        Traslados?.Clear();
        TotalImpuestosRetenidos = null;
        TotalImpuestosTrasladados = null;
    }
}