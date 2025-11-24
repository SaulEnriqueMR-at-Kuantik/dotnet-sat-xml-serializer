using System.Text.Json.Serialization;
using System.Xml.Serialization;
using KpacModels.Shared.Models.Constants;
using KpacModels.Shared.XmlProcessing.Formatter.Interface;

namespace KpacModels.Shared.Models.Comprobante;

public class Concepto
{
    // [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    // [XmlIgnore]
    // public string? IdConcepto { get; set; }
    
    [XmlAttribute(AttributeName = "ClaveProdServ")]
    [JsonPropertyName("ClaveProdServ")]
    public string ClaveProdServ { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("NoIdentificacion")]
    [XmlAttribute(AttributeName = "NoIdentificacion")]
    public string? NoIdentificacion { get; set; }

    [XmlAttribute(AttributeName = "Cantidad")]
    [JsonPropertyName("Cantidad")]
    public string Cantidad { get; set; }
    
    [XmlIgnore]
    [JsonPropertyName("SrcCantidad")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public decimal? SrcCantidad { get; set; }
    
    [XmlAttribute(AttributeName = "ClaveUnidad")]
    [JsonPropertyName("ClaveUnidad")]
    public string ClaveUnidad { get; set; }

     [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("Unidad")]
    [XmlAttribute(AttributeName = "Unidad")]
    public string? Unidad { get; set; }

    [XmlAttribute(AttributeName = "Descripcion")]
    [JsonPropertyName("Descripcion")]
    public string Descripcion { get; set; }
    
    [XmlAttribute(AttributeName = "ValorUnitario")]
    [JsonPropertyName("ValorUnitario")]
    public string ValorUnitario { get; set; }
    
    [XmlIgnore]
    [JsonPropertyName("SrcValorUnitario")]
     [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public decimal? SrcValorUnitario { get; set; }

    [XmlAttribute(AttributeName = "Importe")]
    [JsonPropertyName("Importe")]
    public string Importe { get; set; }
    
    [XmlIgnore]
    [JsonPropertyName("SrcImporte")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public decimal SrcImporte {  get; set; }
    
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("Descuento")]
    [XmlAttribute(AttributeName = "Descuento")]
    public string? Descuento { get; set; }
    
    [XmlIgnore]
    [JsonPropertyName("SrcDescuento")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public decimal? SrcDescuento {  get; set; }
    
    [XmlAttribute(AttributeName = "ObjetoImp")]
    [JsonPropertyName("ObjetoImpuesto")]
    public string ObjetoImpuesto { get; set; }
    
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("Impuestos")]
    [XmlElement(ElementName = "Impuestos", Namespace = Namespaces.CfdiLocation)]
    public ConceptoImpuestos? Impuestos { get; set; }

    public bool ShouldSerializeImpuestos()
    {
        if(Impuestos == null)return false;
        if(Impuestos.Retenciones != null && Impuestos.Retenciones.Count > 0) return true;
        if(Impuestos.Traslados != null && Impuestos.Traslados.Count > 0) return true;
        return false;
    }
    
     [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("ACuentaTerceros")]
    [XmlElement(ElementName = "ACuentaTerceros", Namespace = Namespaces.CfdiLocation)]
    public ACuentaTerceros? ACuentaTerceros { set; get; }
    
     [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("InformacionAduanera")]
    [XmlElement(ElementName = "InformacionAduanera", Namespace = Namespaces.CfdiLocation)]
    public List<InformacionAduanera>? InformacionAduanera { set; get; }
    
    public bool ShouldSerializeInformacionAduanera() => InformacionAduanera != null && InformacionAduanera.Count > 0;
    
     [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("CuentaPredial")]
    [XmlElement(ElementName = "CuentaPredial", Namespace = Namespaces.CfdiLocation)]
    public List<CuentaPredial>? CuentaPredial { set; get; }
    
    public bool ShouldSerializeCuentaPredial() => CuentaPredial != null && CuentaPredial.Count > 0;
    
     [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("Parte")]
    [XmlElement(ElementName = "Parte", Namespace = Namespaces.CfdiLocation)]
    public List<Parte>? Parte { get; set; } 
    
    public bool ShouldSerializeParte() => Parte != null && Parte.Count > 0;
    
    
    public async Task Accept(IVisitorFormatter visitor, int numConcepto)
    {
        visitor.Visit(this, numConcepto);
    }
    
}
