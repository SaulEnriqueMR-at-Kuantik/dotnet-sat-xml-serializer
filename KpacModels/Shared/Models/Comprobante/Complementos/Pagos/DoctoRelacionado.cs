using System.Text.Json.Serialization;
using System.Xml.Serialization;
using KpacModels.Shared.Constants;
using KpacModels.Shared.XmlProcessing.Formatter.Interface;
using KpacModels.Shared.XmlProcessing.Validator.Interface;

namespace KpacModels.Shared.Models.Comprobante.Complementos.Pagos;

public class DoctoRelacionado
{
    // [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    // [XmlIgnore]
    // public string? IdPago { get; set; }

    [XmlAttribute(AttributeName = "IdDocumento")]
    [JsonPropertyName("IdDocumento")]
    public string IdDocumento { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("Serie")]
    [XmlAttribute(AttributeName = "Serie")]
    public string? Serie { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("Folio")]
    [XmlAttribute(AttributeName = "Folio")]
    public string? Folio { get; set; }

    [XmlAttribute(AttributeName = "MonedaDR")]
    [JsonPropertyName("Moneda")]
    public string Moneda { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("MetodoPago")]
    [XmlIgnore]
    public string? MetodoPago { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("Equivalencia")]
    [XmlAttribute(AttributeName = "EquivalenciaDR")]
    public string? Equivalencia { get; set; }

    [XmlAttribute(AttributeName = "NumParcialidad")]
    [JsonPropertyName("NoParcialidad")]
    public string NoParcialidad { get; set; }

    [XmlAttribute(AttributeName = "ImpSaldoAnt")]
    [JsonPropertyName("ImporteSaldoAnterior")]
    public string ImporteSaldoAnterior { get; set; }

    [XmlAttribute(AttributeName = "ImpPagado")]
    [JsonPropertyName("ImportePagado")]
    public string ImportePagado { get; set; }

    [XmlAttribute(AttributeName = "ImpSaldoInsoluto")]
    [JsonPropertyName("ImporteSaldoInsoluto")]
    public string ImporteSaldoInsoluto { get; set; }

    [XmlAttribute(AttributeName = "ObjetoImpDR")]
    [JsonPropertyName("ObjetoImpuesto")]
    public string ObjetoImpuesto { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("Impuestos")]
    [XmlElement(ElementName = "ImpuestosDR", Namespace = Namespaces.Pagos20)]
    public ImpuestosDR? Impuestos { get; set; }

    public bool ShouldSerializeImpuestosDr() => Impuestos != null;

    public async Task Accept(IVisitorPagos visitor, int numPago, int numDocto)
    {
        await visitor.Visit(this, numPago, numDocto);
        if (visitor.HasErrors()) return;
        Impuestos?.Accept(visitor, numPago, numDocto);
    }

    public async Task Accept(IVisitorFormatterPagos visitor, int numPago, int numDocto)
    {
        await visitor.Visit(this, numPago, numDocto);
        Impuestos?.Accept(visitor, numPago, numDocto);
        visitor.SaveImpuestosDr(Impuestos, Equivalencia);
    }
}