using System.Text.Json.Serialization;
using System.Xml.Serialization;
using KpacModels.Shared.Constants;

namespace KpacModels.Shared.Models.Comprobante.Complementos.Nomina;

public class Emisor
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("Curp")]
    [XmlAttribute(AttributeName = "Curp")]
    public string? Curp { get; set; }
    
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("RegistroPatronal")]
    [XmlAttribute(AttributeName = "RegistroPatronal")]
    public string? RegistroPatronal { get; set; }
    
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("RfcPatronOrigen")]
    [XmlAttribute(AttributeName = "RfcPatronOrigen")]
    public string? RfcPatronOrigen { get; set; }
    
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("EntidadSncf")]
    [XmlElement(ElementName = "EntidadSNCF", Namespace = Namespaces.Nomina12)]
    public EntidadSncf? EntidadSncf { get; set; }
    public bool ShouldSerializeEntidadSncf() => EntidadSncf != null;
    
    // [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    // [JsonPropertyName("OrigenRecursos")]
    // [XmlIgnore]
    // public string? OrigenRecursos { get; set; }
    //
    // [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    // [JsonPropertyName("MontoRecursoPropio")]
    // [XmlIgnore]
    // public string? MontoRecursoPropio { get; set; }
}