using System.Text.Json.Serialization;
using System.Xml.Serialization;
using KpacModels.Shared.XmlProcessing.Formatter.Interface;

namespace KpacModels.Shared.Models.Comprobante.Complementos.Pagos;

public class Totales
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("TotalRetencionesIva")]
    [XmlAttribute(AttributeName = "TotalRetencionesIVA")]
    public string? TotalRetencionesIva { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("TotalRetencionesIsr")]
    [XmlAttribute(AttributeName = "TotalRetencionesISR")]
    public string? TotalRetencionesIsr { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("TotalRetencionesIeps")]
    [XmlAttribute(AttributeName = "TotalRetencionesIEPS")]
    public string? TotalRetencionesIeps { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("TotalTrasladosBaseIva16")]
    [XmlAttribute(AttributeName = "TotalTrasladosBaseIVA16")]
    public string? TotalTrasladosBaseIva16 { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("TotalTrasladosImpuestoIva16")]
    [XmlAttribute(AttributeName = "TotalTrasladosImpuestoIVA16")]
    public string? TotalTrasladosImpuestoIva16 { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("TotalTrasladosBaseIva8")]
    [XmlAttribute(AttributeName = "TotalTrasladosBaseIVA8")]
    public string? TotalTrasladosBaseIva8 { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("TotalTrasladosImpuestoIva8")]
    [XmlAttribute(AttributeName = "TotalTrasladosImpuestoIVA8")]
    public string? TotalTrasladosImpuestoIva8 { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("TotalTrasladosBaseIva0")]
    [XmlAttribute(AttributeName = "TotalTrasladosBaseIVA0")]
    public string? TotalTrasladosBaseIva0 { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("TotalTrasladosImpuestoIva0")]
    [XmlAttribute(AttributeName = "TotalTrasladosImpuestoIVA0")]
    public string? TotalTrasladosImpuestoIva0 { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("TotalTrasladosBaseIvaExento")]
    [XmlAttribute(AttributeName = "TotalTrasladosBaseIVAExento")]
    public string? TotalTrasladosBaseIvaExento { get; set; }

    [XmlAttribute(AttributeName = "MontoTotalPagos")]
    [JsonPropertyName("MontoTotalPagos")]
    public string MontoTotalPagos { get; set; }
    
    public void Accept(
        IVisitorFormatterPagos visitor,
        decimal montoTotal,
        List<RetencionP> retencionesTotales,
        List<TrasladoP> trasladosTotales)
    {
        visitor.Visit(this, montoTotal, retencionesTotales, trasladosTotales);
    }
}