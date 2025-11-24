using System.Text.Json.Serialization;
using System.Xml.Serialization;

namespace KpacModels.Shared.Models.Comprobante.Complementos.Pagos;

public class RetencionP : ICloneable
{
    [XmlAttribute(AttributeName = "ImpuestoP")]
    [JsonPropertyName("Impuesto")]
    public string Impuesto { get; set; }

    [XmlAttribute(AttributeName = "ImporteP")]
    [JsonPropertyName("Importe")]
    public string Importe { get; set; }

    /// <summary>
    /// Clona la RetencionP a un object
    /// </summary>
    /// <returns>Objeto tipo <see cref="RetencionP"/></returns>
    public object Clone()
    {
        return new RetencionP()
        {
            Importe = this.Importe,
            Impuesto = this.Impuesto,
        };
    }
}