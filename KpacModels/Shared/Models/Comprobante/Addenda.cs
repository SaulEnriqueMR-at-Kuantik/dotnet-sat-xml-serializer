using System.Xml.Serialization;

namespace KpacModels.Shared.Models.Comprobante;

public class Addenda
{
    [XmlElement(ElementName = "InformacionAdicional", Namespace = "")]
    public List<InformacionAdicionalAddenda>? InformacionAdicional { get; set; }
}

public class InformacionAdicionalAddenda
{
    [XmlElement(ElementName = "CFDI", Namespace = "")]
    public Cfdi? CFDI { get; set; }
    [XmlElement(ElementName = "EMSR", Namespace = "")]
    public Domicilio? EMSR { get; set; }
    [XmlElement(ElementName = "EXP", Namespace = "")]
    public Domicilio? EXP { get; set; }
    [XmlElement(ElementName = "R", Namespace = "")]
    public Rcpr? R { get; set; }
    [XmlElement(ElementName = "PTDAo", Namespace = "")]
    public Ptdao? PTDAo {  get; set; }
}

public class Cfdi
{
    [XmlAttribute(AttributeName = "tipoDocumento")]
    public string? TipoDocumento { get; set; }
    [XmlAttribute(AttributeName = "numeroDocumentos")]
    public string? NumeroDocumentos { get; set; }
    [XmlAttribute(AttributeName = "etiqueta1C")]
    public string? Etiqueta1C { get; set; }
    [XmlAttribute(AttributeName = "valor1C")]
    public string? Valor1C { get; set; }
    [XmlAttribute(AttributeName = "valor10C")]
    public string? Valor10C { get; set; }
}

public class Domicilio
{
    [XmlAttribute(AttributeName = "calle")]
    public string? Calle { get; set; }
    [XmlAttribute(AttributeName = "noExterior")]
    public string? NoExterior { get; set; }
    [XmlAttribute(AttributeName = "noInterior")]
    public string? NoInterior { get; set; }
    [XmlAttribute(AttributeName = "colonia")]
    public string? Colonia { get; set; }
    [XmlAttribute(AttributeName = "municipio")]
    public string? Municipio { get; set; }
    [XmlAttribute(AttributeName = "estado")]
    public string? Estado { get; set; }
    [XmlAttribute(AttributeName = "pais")]
    public string? Pais { get; set; }
    [XmlAttribute(AttributeName = "codigoPostal")]
    public string? CodigoPostal { get; set; }
}

public class Rcpr : Domicilio
{
    [XmlAttribute(AttributeName = "conector")]
    public string? Conector { get; set; }
    [XmlAttribute(AttributeName = "valor13R")]
    public string? Valor13R { get; set; }
    [XmlAttribute(AttributeName = "referencia")]
    public string? Referencia { get; set; }
}

public class Ptdao
{
    [XmlAttribute(AttributeName = "etiqueta1PO")]
    public string? Etiqueta1PO { get; set; }
    [XmlAttribute(AttributeName = "valor1PO")]
    public string? Valor1PO { get; set; }
    [XmlAttribute(AttributeName = "etiqueta2PO")]
    public string? Etiqueta2PO { get; set; }
    [XmlAttribute(AttributeName = "valor2PO")]
    public string? Valor2PO { get; set; }
    [XmlAttribute(AttributeName = "etiqueta3PO")]
    public string? Etiqueta3PO { get; set; }
    [XmlAttribute(AttributeName = "valor3PO")]
    public string? Valor3PO { get; set; }
    [XmlAttribute(AttributeName = "etiqueta4PO")]
    public string? Etiqueta4PO { get; set; }
    [XmlAttribute(AttributeName = "valor4PO")]
    public string? Valor4PO { get; set; }
    [XmlAttribute(AttributeName = "etiqueta5PO")]
    public string? Etiqueta5PO { get; set; }
}