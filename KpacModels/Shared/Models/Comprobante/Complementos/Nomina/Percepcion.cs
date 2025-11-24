using System.Text.Json.Serialization;
using System.Xml.Serialization;
using KpacModels.Shared.Models.Constants;

namespace KpacModels.Shared.Models.Comprobante.Complementos.Nomina;

public class Percepcion
{
    [XmlAttribute(AttributeName = "TipoPercepcion")]
    [JsonPropertyName("Tipo")]
    public string Tipo { get; set; }

    [XmlAttribute(AttributeName = "Clave")]
    [JsonPropertyName("Clave")]
    public string Clave { get; set; }

    [XmlAttribute(AttributeName = "Concepto")]
    [JsonPropertyName("Concepto")]
    public string Concepto { get; set; }

    [XmlAttribute(AttributeName = "ImporteGravado")]
    [JsonPropertyName("ImporteGravado")]
    public string ImporteGravado { get; set; }

    [XmlAttribute(AttributeName = "ImporteExento")]
    [JsonPropertyName("ImporteExento")]
    public string ImporteExento { get; set; }

    // [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    // [JsonPropertyName("ValorMercado")]
    // [XmlIgnore]
    // public string? ValorMercado { get; set; }
    //
    // [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    // [JsonPropertyName("PrecioAlOtorgarse")]
    // [XmlIgnore]
    // public string? PrecioAlOtorgarse { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("AccionesOTitulos")]
    [XmlElement(ElementName = "AccionesOTitulos", Namespace = Namespaces.Nomina12)]
    public AccionesOTitulos? AccionesOTitulos { get; set; }

    public bool ShouldSerializeAccionesOTitulos() => AccionesOTitulos != null;

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("HorasExtra")]
    [XmlElement(ElementName = "HorasExtra", Namespace = Namespaces.Nomina12)]
    public List<HorasExtras>? HorasExtra { get; set; }

    public bool ShouldSerializeHorasExtra() => HorasExtra != null && HorasExtra.Count > 0;
}