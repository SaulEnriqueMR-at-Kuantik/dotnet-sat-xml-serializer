using System.Text.Json.Serialization;
using System.Xml.Serialization;
using KpacModels.Shared.Models.Constants;

namespace KpacModels.Shared.Models.Comprobante.Complementos.Nomina;

public class OtroPago
{
    [XmlAttribute(AttributeName = "TipoOtroPago")]
    [JsonPropertyName("Tipo")]
    public string Tipo { get; set; }

    [XmlAttribute(AttributeName = "Clave")]
    [JsonPropertyName("Clave")]
    public string Clave { get; set; }

    [XmlAttribute(AttributeName = "Concepto")]
    [JsonPropertyName("Concepto")]
    public string Concepto { get; set; }

    [XmlAttribute(AttributeName = "Importe")]
    [JsonPropertyName("Importe")]
    public string Importe { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("SubsidioAlEmpleo")]
    [XmlElement(ElementName = "SubsidioAlEmpleo", Namespace = Namespaces.Nomina12)]
    public SubsidioAlEmpleo? SubsidioAlEmpleo { get; set; }

    public bool ShouldSerializeSubsidioAlEmpleo() => SubsidioAlEmpleo != null;

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("CompensacionSaldosAFavor")]
    [XmlElement(ElementName = "CompensacionSaldosAFavor", Namespace = Namespaces.Nomina12)]
    public CompensacionSaldosAFavor? CompensacionSaldosAFavor { get; set; }

    public bool ShouldSerializeCompensacionSaldosAFavor() => CompensacionSaldosAFavor != null;
}