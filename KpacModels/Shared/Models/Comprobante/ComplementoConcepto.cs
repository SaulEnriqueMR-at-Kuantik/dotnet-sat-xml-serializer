using System.Text.Json.Serialization;
using System.Xml.Serialization;
using KpacModels.Shared.Models.Comprobante.ComplementosConcepto;
using KpacModels.Shared.Models.Constants;
using KpacModels.Shared.Models.JsonConverter;

namespace KpacModels.Shared.Models.Comprobante;

public class ComplementoConcepto
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("HidrocarburosPetroliferos")]
    [Newtonsoft.Json.JsonConverter(typeof(HidrocarburosConverter))]
    [XmlElement(ElementName = "HidroYPetro", Namespace = Namespaces.HidrocarburosPetroliferos)]
    public List<HidrocarburosPetroliferos>? HidrocarburosPetroliferos { get; set; }
}