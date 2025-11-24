using System.Text.Json.Serialization;
using System.Xml.Serialization;
using KPac.Domain.Mapping.JsonConverter;
using KPac.Domain.Mapping.Xml.Comprobante.Complementos.Impuestoslocales;
using KpacModels.Shared.Models.Comprobante.Complementos.Cartaporte;
using KpacModels.Shared.Models.Comprobante.Complementos.Comercioexterior;
using KpacModels.Shared.Models.Comprobante.Complementos.LeyendasFiscales;
using KpacModels.Shared.Models.Comprobante.Complementos.Nomina;
using KpacModels.Shared.Models.Comprobante.Complementos.Pagos;
using KpacModels.Shared.Models.Constants;
using KpacModels.Shared.Models.TimbreFiscalDigital;
using KpacModels.Shared.XmlProcessing.Formatter.Interface;

namespace KpacModels.Shared.Models.Comprobante;

public class Complemento
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [Newtonsoft.Json.JsonConverter(typeof(TimbreFiscalDigitalConverter))]
    [JsonPropertyName("TimbreFiscalDigital")]
    [XmlElement(ElementName = "TimbreFiscalDigital", Namespace = Namespaces.TfdLocation)]
    public TimbreFiscalDigital11? TimbreFiscalDigital { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [Newtonsoft.Json.JsonConverter(typeof(PagosConverter))]
    [JsonPropertyName("Pagos")]
    [XmlElement(ElementName = "Pagos", Namespace = Namespaces.Pagos20)]
    public List<Pagos20>? Pagos { get; set; }

    public bool ShouldSerializePagos() => Pagos != null && Pagos.Count > 0;

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [Newtonsoft.Json.JsonConverter(typeof(NominaConverter))]
    [JsonPropertyName("Nomina")]
    [XmlElement(ElementName = "Nomina", Namespace = Namespaces.Nomina12)]
    public List<Nomina12>? Nomina { get; set; }

    public bool ShouldSerializeNomina() => Nomina != null && Nomina.Count > 0;

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [Newtonsoft.Json.JsonConverter(typeof(ImpuestosLocalesConverter))]
    [JsonPropertyName("ImpuestosLocales")]
    [XmlElement(ElementName = "ImpuestosLocales", Namespace = Namespaces.ImpuestosLocales10)]
    public List<ImpuestosLocales10>? ImpuestosLocales { get; set; }

    public bool ShouldSerializeImpuestosLocales() => ImpuestosLocales != null && ImpuestosLocales.Count > 0;

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [Newtonsoft.Json.JsonConverter(typeof(ComercioExteriorConverter))]
    [JsonPropertyName("ComercioExterior")]
    [XmlElement(ElementName = "ComercioExterior", Namespace = Namespaces.ComercioExterior20)]
    public List<ComercioExterior20>? ComercioExterior { get; set; }

    public bool ShouldSerializeComercioExterior() => ComercioExterior != null && ComercioExterior.Count > 0;

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [Newtonsoft.Json.JsonConverter(typeof(CartaPorteConverter))]
    [JsonPropertyName("CartaPorte")]
    [XmlElement(ElementName = "CartaPorte", Namespace = Namespaces.CartaPorte31)]
    public List<CartaPorte31>? CartaPorte { get; set; }

    public bool ShouldSerializeCartaPorte() => CartaPorte != null && CartaPorte.Count > 0;

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [Newtonsoft.Json.JsonConverter(typeof(LeyendasFiscalesConverter))]
    [JsonPropertyName("LeyendasFiscales")]
    [XmlElement(ElementName = "LeyendasFiscales", Namespace = Namespaces.Leyendas10)]
    public List<LeyendasFiscales10>? LeyendasFiscales { get; set; }

    public bool ShouldSerializeLeyendasFiscales() => LeyendasFiscales != null && LeyendasFiscales.Count > 0;


    public async Task Format(IVisitorFormatterPagos visitor)
    {
        visitor.Visit(this);
        var pago = Pagos?.FirstOrDefault();
        if (pago != null)
        {
            await pago.Format(visitor);
        }
    }
    
    public async Task Format(IVisitorFormatterNomina visitor)
    {
        visitor.Visit(this);
        var nomina = Nomina?.FirstOrDefault();
        if (nomina != null)
        {
            await nomina.Format(visitor);
        }
    }
}