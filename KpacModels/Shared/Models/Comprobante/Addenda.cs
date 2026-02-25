using System.Xml.Serialization;

namespace KpacModels.Shared.Models.Comprobante;

public class Addenda
{
    [XmlElement(ElementName = "InformacionAdicional")]
    public List<InformacionAdicionalAddenda>? InformacionAdicional { get; set; }
}

public class InformacionAdicionalAddenda
{
    [XmlElement(ElementName = "CFDI ")]
    public Cfdi? CFDI { get; set; }
    [XmlElement(ElementName = "EMSR")]
    public Domicilio? EMSR { get; set; }
    [XmlElement(ElementName = "EXP")]
    public Domicilio? EXP { get; set; }
    [XmlElement(ElementName = "R")]
    public Rcpr? R { get; set; }
    [XmlElement(ElementName = "PTDAo")]
    public Ptdao? PTDAo {  get; set; }
}

public class Cfdi
{
    [XmlElement(ElementName = "tipoDocumento")]
    public string? TipoDocumento { get; set; }
    [XmlElement(ElementName = "numeroDocumentos")]
    public string? NumeroDocumentos { get; set; }
    [XmlElement(ElementName = "etiqueta1C")]
    public string? Etiqueta1C { get; set; }
    [XmlElement(ElementName = "valor1C")]
    public string? Valor1C { get; set; }
    [XmlElement(ElementName = "valor10C")]
    public string? Valor10C { get; set; }
}

public class Domicilio
{
    [XmlElement(ElementName = "calle")]
    public string? Calle { get; set; }
    [XmlElement(ElementName = "noExterior")]
    public string? NoExterior { get; set; }
    [XmlElement(ElementName = "noInterior")]
    public string? NoInterior { get; set; }
    [XmlElement(ElementName = "colonia")]
    public string? Colonia { get; set; }
    [XmlElement(ElementName = "municipio")]
    public string? Municipio { get; set; }
    [XmlElement(ElementName = "estado")]
    public string? Estado { get; set; }
    [XmlElement(ElementName = "pais")]
    public string? Pais { get; set; }
    [XmlElement(ElementName = "codigoPostal")]
    public string? CodigoPostal { get; set; }
}

public class Rcpr : Domicilio
{
    [XmlElement(ElementName = "conector")]
    public string? Conector { get; set; }
    [XmlElement(ElementName = "valor13R")]
    public string? Valor13R { get; set; }
    [XmlElement(ElementName = "referencia")]
    public string? Referencia { get; set; }
}

public class Ptdao
{
    [XmlElement(ElementName = "etiqueta1PO")]
    public string? Etiqueta1PO { get; set; }
    [XmlElement(ElementName = "valor1PO")]
    public string? Valor1PO { get; set; }
    [XmlElement(ElementName = "etiqueta2PO")]
    public string? Etiqueta2PO { get; set; }
    [XmlElement(ElementName = "valor2PO")]
    public string? Valor2PO { get; set; }
    [XmlElement(ElementName = "etiqueta3PO")]
    public string? Etiqueta3PO { get; set; }
    [XmlElement(ElementName = "valor3PO")]
    public string? Valor3PO { get; set; }
    [XmlElement(ElementName = "etiqueta4PO")]
    public string? Etiqueta4PO { get; set; }
    [XmlElement(ElementName = "valor4PO")]
    public string? Valor4PO { get; set; }
    [XmlElement(ElementName = "etiqueta5PO")]
    public string? Etiqueta5PO { get; set; }
}