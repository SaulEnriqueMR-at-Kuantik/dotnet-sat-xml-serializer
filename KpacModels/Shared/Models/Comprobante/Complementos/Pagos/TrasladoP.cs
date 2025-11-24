using System.Text.Json.Serialization;
using System.Xml.Serialization;

namespace KpacModels.Shared.Models.Comprobante.Complementos.Pagos;

public class TrasladoP : ICloneable
{
    // [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    // [XmlIgnore]
    // public string? IdPago { get; set; }

    [XmlAttribute(AttributeName = "BaseP")]
    [JsonPropertyName("Base")]
    public string Base { get; set; }

    [XmlAttribute(AttributeName = "ImpuestoP")]
    [JsonPropertyName("Impuesto")]
    public string Impuesto { get; set; }

    [XmlAttribute(AttributeName = "TipoFactorP")]
    [JsonPropertyName("TipoFactor")]
    public string TipoFactor { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("TasaOCuota")]
    [XmlAttribute(AttributeName = "TasaOCuotaP")]
    public string? TasaOCuota { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("Importe")]
    [XmlAttribute(AttributeName = "ImporteP")]
    public string? Importe { get; set; }


    /// <summary>
    /// Clona la TrasladoP a un object
    /// </summary>
    /// <returns>Objeto tipo <see cref="TrasladoP"/></returns>
    public object Clone()
    {
        return new TrasladoP
        {
            Importe = this.Importe,
            Base = this.Base,
            TasaOCuota = this.TasaOCuota,
            Impuesto = this.Impuesto,
            TipoFactor = this.TipoFactor,
        };
    }
}