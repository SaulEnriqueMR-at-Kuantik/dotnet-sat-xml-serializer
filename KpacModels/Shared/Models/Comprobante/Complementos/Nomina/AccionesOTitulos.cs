using System.Text.Json.Serialization;
using System.Xml.Serialization;

namespace KpacModels.Shared.Models.Comprobante.Complementos.Nomina;

public class AccionesOTitulos
{
    [XmlAttribute(AttributeName = "ValorMercado")]
    [JsonPropertyName("ValorMercado")]
    public string ValorMercado { get; set; }

    [XmlAttribute(AttributeName = "PrecioAlOtorgarse")]
    [JsonPropertyName("PrecioAlOtorgarse")]
    public string PrecioAlOtorgarse { get; set; }
}