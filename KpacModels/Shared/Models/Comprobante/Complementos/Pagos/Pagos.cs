using System.Text.Json.Serialization;
using System.Xml.Serialization;
using KpacModels.Shared.Models.Constants;

namespace KpacModels.Shared.Models.Comprobante.Complementos.Pagos;

[XmlRoot(ElementName = "Pagos", Namespace = Namespaces.Pagos20)]
public class Pagos20
{
    [XmlAttribute(AttributeName = "Version")]
    [JsonPropertyName("Version")]
    public string Version { get; set; }

    [XmlElement(ElementName = "Totales", Namespace = Namespaces.Pagos20)]
    [JsonPropertyName("Totales")]
    public Totales Totales { get; set; }

    [XmlElement(ElementName = "Pago", Namespace = Namespaces.Pagos20)]
    [JsonPropertyName("Pago")]
    public List<Pago> Pago { get; set; }

    public bool ShouldSerializePago() => Pago != null && Pago.Count > 0;
}