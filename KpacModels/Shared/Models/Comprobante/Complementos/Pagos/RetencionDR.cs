using System.Text.Json.Serialization;
using System.Xml.Serialization;
using KpacModels.Shared.XmlProcessing.Formatter.Interface;

namespace KpacModels.Shared.Models.Comprobante.Complementos.Pagos;

public class RetencionDR : ICloneable
{
    // [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    // [XmlIgnore]
    // public string? IdDocumento { get; set; }
    //
    // [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    // [XmlIgnore]
    // public string? IdPago { get; set; }

    [XmlAttribute(AttributeName = "BaseDR")]
    [JsonPropertyName("BaseDR")]
    public string Base { get; set; }

    [XmlAttribute(AttributeName = "ImpuestoDR")]
    [JsonPropertyName("Impuesto")]
    public string Impuesto { get; set; }

    [XmlAttribute(AttributeName = "TipoFactorDR")]
    [JsonPropertyName("TipoFactor")]
    public string TipoFactor { get; set; }

    [XmlAttribute(AttributeName = "TasaOCuotaDR")]
    [JsonPropertyName("TasaOCuota")]
    public string TasaOCuota { get; set; }

    [XmlAttribute(AttributeName = "ImporteDR")]
    [JsonPropertyName("Importe")]
    public string Importe { get; set; }
    
    public void Accept(IVisitorFormatterPagos visit, int numPago, int numDocto, int noRetencion)
    {
        visit.Visit(this, numPago, numDocto, noRetencion);
    }

    public object Clone()
    {
        return new RetencionDR
        {
            Base = Base,
            Impuesto = Impuesto,
            TipoFactor = TipoFactor,
            TasaOCuota = TasaOCuota,
            Importe = Importe,
        };
    }
}