using System.Text.Json.Serialization;
using System.Xml.Serialization;

namespace KpacModels.Shared.Models.Comprobante.Complementos.Nomina;

public class EntidadSncf
{
    [XmlAttribute(AttributeName = "OrigenRecurso")]
    [JsonPropertyName("OrigenRecurso")]
    public string OrigenRecurso { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("MontoRecursoPropio")]
    [XmlAttribute(AttributeName = "MontoRecursoPropio")]
    public string? MontoRecursoPropio { get; set; }
}