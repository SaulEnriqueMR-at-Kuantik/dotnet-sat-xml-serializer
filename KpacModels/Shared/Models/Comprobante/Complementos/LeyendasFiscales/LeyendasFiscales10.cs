using System.Text.Json.Serialization;
using System.Xml.Serialization;
using KpacModels.Shared.Models.Constants;

namespace KpacModels.Shared.Models.Comprobante.Complementos.LeyendasFiscales;

[XmlRoot(ElementName = "LeyendasFiscales", Namespace = Namespaces.Leyendas10)]
public class LeyendasFiscales10
{
    [XmlAttribute(AttributeName = "version")]
    [JsonPropertyName("Version")]
    public string Version { get; set; }

    [XmlElement(ElementName = "Leyenda", Namespace = Namespaces.Leyendas10)]
    [JsonPropertyName("Leyenda")]
    public Leyenda[] Leyenda { get; set; }
}

public class Leyenda
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("DisposicionFiscal")]
    [XmlAttribute(AttributeName = "disposicionFiscal")]
    public string? DisposicionFiscal { get; set; }

    [XmlAttribute(AttributeName = "norma")]
    [JsonPropertyName("Norma")]
    public string Norma { get; set; }

    [XmlAttribute(AttributeName = "textoLeyenda")]
    [JsonPropertyName("TextoLeyenda")]
    public string TextoLeyenda { get; set; }
}