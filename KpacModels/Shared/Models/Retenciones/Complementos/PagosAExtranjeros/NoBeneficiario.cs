using System.Text.Json.Serialization;
using System.Xml.Serialization;

namespace KpacModels.Shared.Models.Retenciones.Complementos.PagosAExtranjeros;

public class NoBeneficiario
{
    [JsonPropertyName("ResidenciaFiscal")]
    [XmlAttribute(AttributeName = "PaisDeResidParaEfecFisc")]
    public string ResidenciaFiscal { get; set; }

    [JsonPropertyName("")]
    [XmlAttribute(AttributeName = "ConceptoPago")]
    public string ConceptoPago { get; set; }

    [JsonPropertyName("DescripcionConcepto")]
    [XmlAttribute(AttributeName = "DescripcionConcepto")]
    public string DescripcionConcepto { get; set; }
}