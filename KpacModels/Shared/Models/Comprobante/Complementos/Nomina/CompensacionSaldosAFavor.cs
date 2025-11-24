using System.Text.Json.Serialization;
using System.Xml.Serialization;

namespace KpacModels.Shared.Models.Comprobante.Complementos.Nomina;

public class CompensacionSaldosAFavor
{
    [XmlAttribute(AttributeName = "SaldoAFavor")]
    [JsonPropertyName("SaldoAFavor")]
    public string SaldoAFavor { get; set; }

    [XmlAttribute(AttributeName = "Año")]
    [JsonPropertyName("Anio")]
    public string Anio { get; set; }

    [XmlAttribute(AttributeName = "RemanenteSalFav")]
    [JsonPropertyName("RemanenteSaldoAFavor")]
    public string RemanenteSaldoAFavor { get; set; }
}