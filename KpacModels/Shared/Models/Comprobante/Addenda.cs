namespace KpacModels.Shared.Models.Comprobante;

public class Addenda
{
    public List<InformacionAdicionalAddenda>? InformacionAdicional { get; set; }
}

public class InformacionAdicionalAddenda
{
    public Cfdi? CFDI { get; set; }
    public Domicilio? EMSR { get; set; }
    public Domicilio? EXP { get; set; }
    public Rcpr? R { get; set; }
    public Ptdao? PTDAo {  get; set; }
}

public class Cfdi
{
    public string? TipoDocumento { get; set; }
    public string? NumeroDocumentos { get; set; }
    public string? Etiqueta1C { get; set; }
    public string? Valor1C { get; set; }
    public string? Valor10C { get; set; }
}

public class Domicilio
{
    public string? Calle { get; set; }
    public string? NoExterior { get; set; }
    public string? NoInterior { get; set; }
    public string? Colonia { get; set; }
    public string? Municipio { get; set; }
    public string? Estado { get; set; }
    public string? Pais { get; set; }
    public string? CodigoPostal { get; set; }
}

public class Rcpr : Domicilio
{
    public string? Conector { get; set; }
    public string? Valor13R { get; set; }
    public string? Referencia { get; set; }
}

public class Ptdao
{
    public string? Etiqueta1PO { get; set; }
    public string? Valor1PO { get; set; }
    public string? Etiqueta2PO { get; set; }
    public string? Valor2PO { get; set; }
    public string? Etiqueta3PO { get; set; }
    public string? Valor3PO { get; set; }
    public string? Etiqueta4PO { get; set; }
    public string? Valor4PO { get; set; }
    public string? Etiqueta5PO { get; set; }
}