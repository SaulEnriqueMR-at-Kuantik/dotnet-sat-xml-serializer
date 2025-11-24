using System.Text.Json.Serialization;
using System.Xml.Serialization;
using KpacModels.Shared.XmlProcessing.Formatter.Interface;

namespace KpacModels.Shared.Models.Comprobante;

public class InformacionGlobal
{
    [JsonPropertyName("Periodicidad")]
    [XmlAttribute(AttributeName = "Periodicidad")]
    public string Periodicidad { get; set; }

    [JsonPropertyName("Meses")]
    [XmlAttribute(AttributeName = "Meses")]
    public string Meses { get; set; }

    [JsonPropertyName("Anio")]
    [XmlAttribute(AttributeName = "Año")]
    public string Anio { get; set; }
    
    [XmlIgnore]
    [JsonPropertyName("SrcAnio")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? SrcAnio {  get; set; }

    public void Accept(IVisitorFormatter visitor)
    {
        visitor.Visit(this);
    }
}