using System.Text.Json.Serialization;
using System.Xml.Serialization;
using KpacModels.Shared.Models.Constants;
using KpacModels.Shared.XmlProcessing.Formatter;
using KpacModels.Shared.XmlProcessing.Formatter.Interface;

namespace KpacModels.Shared.Models.Comprobante;

[XmlRoot(ElementName = "Comprobante", Namespace = Namespaces.CfdiLocation)]
public class Comprobante40
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("Factura")]
    [XmlIgnore]
    public string? Factura {  get; set; }
    
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("Comentario")]
    [XmlIgnore]
    public string? Comentario { get; set; }
    
    [XmlAttribute(AttributeName = "Version")]
    [JsonPropertyName("Version")]
    public string Version { get; set; }
    
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("Serie")]
    [XmlAttribute(AttributeName = "Serie")]
    public string? Serie { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("Folio")]
    [XmlAttribute(AttributeName = "Folio")]
    public string? Folio { get; set; }
    
    [JsonPropertyName("Fecha")]
    [XmlAttribute(AttributeName = "Fecha")]
    public string? Fecha { get; set; }

    [XmlAttribute(AttributeName = "Sello")]
    [JsonPropertyName("Sello")]
    public string? Sello { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("FormaPago")]
    [XmlAttribute(AttributeName = "FormaPago")]
    public string? FormaPago { get; set; }
    
    [XmlAttribute(AttributeName = "Certificado")]
    [JsonPropertyName("Certificado")]
    public string? Certificado { get; set; }
    
    [XmlAttribute(AttributeName = "NoCertificado")]
    [JsonPropertyName("NoCertificado")]
    public string? NoCertificado { get; set; }
    
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("CondicionesPago")]
    [XmlAttribute(AttributeName = "CondicionesDePago")]
    public string? CondicionesPago { get; set; }
    
    [XmlAttribute(AttributeName = "SubTotal")]
    [JsonPropertyName("Subtotal")]
    public string Subtotal { get; set; }
    
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("Descuento")]
    [XmlAttribute(AttributeName = "Descuento")]
    public string? Descuento { get; set; }

    [XmlAttribute(AttributeName = "Moneda")]
    [JsonPropertyName("Moneda")]
    public string? Moneda { get; set; }
    
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("TipoCambio")]
    [XmlAttribute(AttributeName = "TipoCambio")]
    public string? TipoCambio { get; set; }
    
    [XmlAttribute(AttributeName = "Total")]
    [JsonPropertyName("Total")]
    public string Total { get; set; }
    
    [XmlAttribute(AttributeName = "TipoDeComprobante")]
    [JsonPropertyName("TipoComprobante")]
    public string? TipoComprobante { get; set; }

    [XmlAttribute(AttributeName = "Exportacion")]
    [JsonPropertyName("Exportacion")]
    public string Exportacion { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("MetodoPago")]
    [XmlAttribute(AttributeName = "MetodoPago")]
    public string? MetodoPago { get; set; }
    
    [XmlAttribute(AttributeName = "LugarExpedicion")]
    [JsonPropertyName("LugarExpedicion")]
    public string LugarExpedicion { get; set; }
    
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("Confirmacion")]
    [XmlAttribute(AttributeName = "Confirmacion")]
    public string? Confirmacion { get; set; }
    
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("InformacionGlobal")]
    [XmlElement(ElementName = "InformacionGlobal", Namespace = Namespaces.CfdiLocation)]
    public InformacionGlobal? InformacionGlobal { get; set; }
    
    
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("CfdiRelacionados")]
    [XmlElement(ElementName = "CfdiRelacionados", Namespace = Namespaces.CfdiLocation)]
    public List<CfdiRelacionado>? CfdisRelacionados { get; set; }
    
    public bool ShouldSerializeCfdisRelacionados() => CfdisRelacionados is { Count: > 0 };
    
    [XmlElement(ElementName = "Emisor", Namespace = Namespaces.CfdiLocation)]
    [JsonPropertyName("Emisor")]
    public Emisor Emisor { get; set; }
    
    [XmlElement(ElementName = "Receptor", Namespace = Namespaces.CfdiLocation)]
    [JsonPropertyName("Receptor")]
    public Receptor Receptor { get; set; }
    
    [XmlArray(ElementName = "Conceptos", Namespace = Namespaces.CfdiLocation)]
    [XmlArrayItem(ElementName = "Concepto", Namespace = Namespaces.CfdiLocation)]
    [JsonPropertyName("Conceptos")]
    public List<Concepto> Conceptos { get; set; }
    
    public bool ShouldSerializeConceptos() => Conceptos?.Count > 0;

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("Impuestos")]
    [XmlElement(ElementName = "Impuestos", Namespace = Namespaces.CfdiLocation)]
    public Impuestos? Impuestos { get; set; }
    
    private string? _addendaXml;
    
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [XmlElement(ElementName = "Addenda", Namespace = Namespaces.CfdiLocation)]
    public Addenda? Addenda { get; set; }

    [JsonPropertyName("AddendaString")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [XmlIgnore]
    public string? AddendaXml 
    { 
        get => _addendaXml;
        set => _addendaXml = value;
    }
    
    [XmlIgnore]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("InformacionAdicional")]
    public InformacionAdicional? InformacionAdicional {  get; set; }

    public bool ShouldSerializeImpuestos()
    { 
        if(Impuestos == null) return false;
        if(Impuestos.Retenciones != null && Impuestos.Retenciones.Count > 0) return true;
        if(Impuestos.Traslados != null && Impuestos.Traslados.Count > 0) return true;
        return false;
    }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("Complemento")]
    [XmlElement(ElementName = "Complemento", Namespace = Namespaces.CfdiLocation)]
    public Complemento? Complemento { get; set; }
    
    public async Task Format(IVisitorFormatter visitor)
    {
        visitor.SaveAttributeBase(this);
        if (Conceptos != null)
        {
            var conceptosLenght = Conceptos.Count;
            for (var i = 0; i < conceptosLenght; i++)
            {
                var concepto = Conceptos[i];
                await concepto.Accept(visitor, i + 1);
            }
        }
        if (CfdisRelacionados != null)
        {
            var cfdiRelacionadosCount = CfdisRelacionados.Count;
            for (var i = 0; i < cfdiRelacionadosCount; i++)
            {
                var cfdiRelacionado = CfdisRelacionados[i];
                cfdiRelacionado.Accept(visitor, i + 1);
            }
        }
        Emisor?.Accept(visitor);
        InformacionGlobal?.Accept(visitor);
        Receptor?.Accept(visitor);
        if(Impuestos == null) Impuestos = new Impuestos();
        Impuestos.Accept(visitor);
        visitor.Visit(this);
        visitor.Clean();
    }

    /// <summary>
    /// Formatear un Comprobante 4.0 con complemento Pagos 2.0, se debe enviar como parametro el visitante que visitara cada nodo.
    /// Realiza los calculos de ImpuestosP y Totales.
    /// </summary>
    /// <param name="visitor">Interfaz del visitante de pagos 2.0</param>
    public async Task Format(IVisitorFormatterPagos visitor)
    {
        visitor.Visit(this);
        visitor.Visit(Complemento);
        if(Complemento != null)
            await Complemento.Format(visitor);
    }

    /// <summary>
    /// Formatear un Comprobante 4.0 con complemento Pagos 2.0, se debe enviar como parametro el visitante que visitara cada nodo.
    /// Realiza los calculos de ImpuestosP y Totales.
    /// </summary>
    /// <param name="visitor">Interfaz del visitante de pagos 2.0</param>
    /// <param name="configuracion"></param>
    public async Task Format(IVisitorFormatterNomina visitor, SettingsFormatter? configuracion)
    {
        visitor.Visit(this, configuracion);
        visitor.Visit(Complemento);
        if(Complemento != null)
            await Complemento.Format(visitor);
        visitor.Visit(this);
    }

    public bool IsPagos20()
    {
        if (Complemento?.Pagos == null) 
            return false;
        return Complemento.Pagos.Count != 0;
    }

    public bool IsNomina12()
    {
        if (Complemento?.Nomina == null) 
            return false;
        return Complemento.Nomina.Count != 0;
    }
}