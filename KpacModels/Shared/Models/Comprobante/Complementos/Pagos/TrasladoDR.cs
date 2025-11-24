using System.Text.Json.Serialization;
using System.Xml.Serialization;
using KpacModels.Shared.XmlProcessing.Formatter.Interface;
using KpacModels.Shared.XmlProcessing.Validator.Interface;

namespace KpacModels.Shared.Models.Comprobante.Complementos.Pagos;

public class TrasladoDR : ICloneable
{
    // [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    // [XmlIgnore]
    // public string? IdDocumento { get; set; }
    //
    // [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    // [XmlIgnore]
    // public string? IdPago { get; set; }

    [XmlAttribute(AttributeName = "BaseDR")]
    [JsonPropertyName("Base")]
    public string Base { get; set; }

    [XmlAttribute(AttributeName = "ImpuestoDR")]
    [JsonPropertyName("Impuesto")]
    public string Impuesto { get; set; }

    [XmlAttribute(AttributeName = "TipoFactorDR")]
    [JsonPropertyName("TipoFactor")]
    public string TipoFactor { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("TasaOCuota")]
    [XmlAttribute(AttributeName = "TasaOCuotaDR")]
    public string? TasaOCuota { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("Importe")]
    [XmlAttribute(AttributeName = "ImporteDR")]
    public string? Importe { get; set; }

    public void Accept(IVisitorPagos visit, int numPago, int numDocto, int numTraslado)
    {
        visit.Visit(this, numPago, numDocto, numTraslado);
    }

    public void Accept(IVisitorFormatterPagos visit, int numPago, int numDocto, int numTraslado)
    {
        visit.Visit(this, numPago, numDocto, numTraslado);
    }

    public object Clone()
    {
        return new TrasladoDR
        {
            Impuesto = this.Impuesto,
            Base = this.Base,
            Importe = this.Importe,
            TipoFactor = this.TipoFactor,
            TasaOCuota = this.TasaOCuota,
        };
    }
}