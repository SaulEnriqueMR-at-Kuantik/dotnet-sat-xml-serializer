using System.Text.Json.Serialization;
using System.Xml.Serialization;
using KpacModels.Shared.Models.Constants;

namespace KpacModels.Shared.Models.Comprobante.Complementos.Pagos;

public class Pago
{
    // [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    // [XmlIgnore]
    // public string? IdPago { get; set; }

    [XmlAttribute(AttributeName = "FechaPago")]
    [JsonPropertyName("FechaPago")]
    public string FechaPago { get; set; }

    [XmlAttribute(AttributeName = "FormaDePagoP")]
    [JsonPropertyName("FormaPago")]
    public string FormaPago { get; set; }

    [XmlAttribute(AttributeName = "MonedaP")]
    [JsonPropertyName("Moneda")]
    public string Moneda { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("TipoCambio")]
    [XmlAttribute(AttributeName = "TipoCambioP")]
    public string? TipoCambio { get; set; }


    [XmlAttribute(AttributeName = "Monto")]
    [JsonPropertyName("Monto")]
    public string Monto { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("NoOperacion")]
    [XmlAttribute(AttributeName = "NumOperacion")]
    public string? NoOperacion { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("RfcEmisorCuentaOrdenante")]
    [XmlAttribute(AttributeName = "RfcEmisorCtaOrd")]
    public string? RfcEmisorCuentaOrdenante { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("NombreBancoOrdenanteExtranjero")]
    [XmlAttribute(AttributeName = "NomBancoOrdExt")]
    public string? NombreBancoOrdenanteExtranjero { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("CuentaOrdenante")]
    [XmlAttribute(AttributeName = "CtaOrdenante")]
    public string? CuentaOrdenante { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("RfcEmisorCuentaBeneficiario")]
    [XmlAttribute(AttributeName = "RfcEmisorCtaBen")]
    public string? RfcEmisorCuentaBeneficiario { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("CuentaBeneficiario")]
    [XmlAttribute(AttributeName = "CtaBeneficiario")]
    public string? CuentaBeneficiario { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("TipoCadenaPago")]
    [XmlAttribute(AttributeName = "TipoCadPago")]
    public string? TipoCadenaPago { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("CertificadoPago")]
    [XmlAttribute(AttributeName = "CertPago")]
    public string? CertificadoPago { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("CadenaPago")]
    [XmlAttribute(AttributeName = "CadPago")]
    public string? CadenaPago { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("SelloPago")]
    [XmlAttribute(AttributeName = "SelloPago")]
    public string? SelloPago { get; set; }

    [XmlElement(ElementName = "DoctoRelacionado", Namespace = Namespaces.Pagos20)]
    [JsonPropertyName("DoctoRelacionado")]
    public List<DoctoRelacionado> DocumentosRelacionados { get; set; }

    public bool ShouldSerializeDocumentosRelacionados() =>
        DocumentosRelacionados != null && DocumentosRelacionados.Count > 0;

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("Impuestos")]
    [XmlElement(ElementName = "ImpuestosP", Namespace = Namespaces.Pagos20)]
    public ImpuestosP? Impuestos { get; set; }

    public bool ShouldSerializeImpuestos() => Impuestos != null;
    
}