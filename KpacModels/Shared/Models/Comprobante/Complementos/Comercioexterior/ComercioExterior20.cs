using System.Text.Json.Serialization;
using System.Xml.Serialization;
using KpacModels.Shared.Models.Constants;

namespace KpacModels.Shared.Models.Comprobante.Complementos.Comercioexterior;

[XmlRoot(ElementName = "ComercioExterior", Namespace = Namespaces.ComercioExterior20)]
public class ComercioExterior20
{
    [XmlAttribute(AttributeName = "Version")]
    [JsonPropertyName("Version")]
    public string Version { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("MotivoTraslado")]
    [XmlAttribute(AttributeName = "MotivoTraslado")]
    public string? MotivoTraslado { get; set; }

    [XmlAttribute(AttributeName = "ClaveDePedimento")]
    [JsonPropertyName("ClavePedimento")]
    public string ClavePedimento { get; set; }

    [XmlAttribute(AttributeName = "CertificadoOrigen")]
    [JsonPropertyName("CertificadoOrigen")]
    public string CertificadoOrigen { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("NoCertificadoOrigen")]
    [XmlAttribute(AttributeName = "NumCertificadoOrigen")]
    public string? NoCertificadoOrigen { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("NoExportadorConfiable")]
    [XmlAttribute(AttributeName = "NumeroExportadorConfiable")]
    public string? NoExportadorConfiable { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("Incoterm")]
    [XmlAttribute(AttributeName = "Incoterm")]
    public string? Incoterm { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("Observaciones")]
    [XmlAttribute(AttributeName = "Observaciones")]
    public string? Observaciones { get; set; } // No está mal escrito, asi está en los modelos de Kore Models

    [XmlAttribute(AttributeName = "TipoCambioUSD")]
    [JsonPropertyName("TipoCambioUsd")]
    public string TipoCambioUsd { get; set; }

    [XmlAttribute(AttributeName = "TotalUSD")]
    [JsonPropertyName("TotalUsd")]
    public string TotalUsd { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("Emisor")]
    [XmlElement(ElementName = "Emisor", Namespace = Namespaces.ComercioExterior20)]
    public EmisorComercioExterior20? Emisor { get; set; }

    public bool ShouldSerializeEmisor() => Emisor != null;

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("Propietario")]
    [XmlElement(ElementName = "Propietario", Namespace = Namespaces.ComercioExterior20)]
    public List<PropietarioComercioExterior20>? Propietario { get; set; }

    public bool ShouldSerializePropietario() => Propietario != null && Propietario.Count > 0;

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("Receptor")]
    [XmlElement(ElementName = "Receptor", Namespace = Namespaces.ComercioExterior20)]
    public ReceptorComercioExterior? Receptor { get; set; }

    public bool ShouldSerializeReceptor() => Receptor != null;

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("Destinatario")]
    [XmlElement(ElementName = "Destinatario", Namespace = Namespaces.ComercioExterior20)]
    public List<DestinatarioComercioExterior20>? Destinatario { get; set; }

    public bool ShouldSerializeDestinatario() => Destinatario != null && Destinatario.Count > 0;

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("Mercancias")]
    [XmlArray(ElementName = "Mercancias", Namespace = Namespaces.ComercioExterior20)]
    [XmlArrayItem(ElementName = "Mercancia", Namespace = Namespaces.ComercioExterior20)]
    public List<MercanciaComercioExterior20>? Mercancias { get; set; }
}

public class EmisorComercioExterior20
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("Curp")]
    [XmlAttribute(AttributeName = "Curp")]
    public string? Curp { get; set; }

    [XmlElement(ElementName = "Domicilio", Namespace = Namespaces.ComercioExterior20)]
    [JsonPropertyName("Domicilio")]
    public DomicilioComercioExterior20 Domicilio { get; set; }
}

public class PropietarioComercioExterior20
{
    [XmlAttribute(AttributeName = "NumRegIdTrib")]
    [JsonPropertyName("NumRegIdTrib")]
    public string NumRegIdTrib { get; set; }

    [XmlAttribute(AttributeName = "ResidenciaFiscal")]
    [JsonPropertyName("ResidenciaFiscal")]
    public string ResidenciaFiscal { get; set; }
}

public class ReceptorComercioExterior
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("NumRegIdTrib")]
    [XmlAttribute(AttributeName = "NumRegIdTrib")]
    public string? NumRegIdTrib { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("Domicilio")]
    [XmlElement(ElementName = "Domicilio", Namespace = Namespaces.ComercioExterior20)]
    public DomicilioComercioExterior20? Domicilio { get; set; }

    public bool ShouldSerializeDomicilio() => Domicilio != null;
}

public class DestinatarioComercioExterior20
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("NumRegIdTrib")]
    [XmlAttribute(AttributeName = "NumRegIdTrib")]
    public string? NumRegIdTrib { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("Nombre")]
    [XmlAttribute(AttributeName = "Nombre")]
    public string? Nombre { get; set; }

    [XmlElement(ElementName = "Domicilio", Namespace = Namespaces.ComercioExterior20)]
    [JsonPropertyName("Domicilio")]
    public List<DomicilioComercioExterior20> Domicilio { get; set; }

    public bool ShouldSerializeDomicilio() => Domicilio != null && Domicilio.Count > 0;
}

public class MercanciaComercioExterior20
{
    [XmlAttribute(AttributeName = "NoIdentificacion")]
    [JsonPropertyName("NoIdentificacion")]
    public string NoIdentificacion { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("FraccionArancelario")]
    [XmlAttribute(AttributeName = "FraccionArancelaria")]
    public string? FraccionArancelario { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("CantidadAduana")]
    [XmlAttribute(AttributeName = "CantidadAduana")]
    public string? CantidadAduana { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("UnidadAduana")]
    [XmlAttribute(AttributeName = "UnidadAduana")]
    public string? UnidadAduana { get; set; }
    
    [JsonPropertyName("ValorUnitarioAduana")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [XmlAttribute(AttributeName = "ValorUnitarioAduana")]
    public string? ValorUnitarioAduana { get; set; }

    [JsonPropertyName("ValorDolares")]
    [XmlAttribute(AttributeName = "ValorDolares")]
    public string ValorDolares { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("DescripcionesEspecificas")]
    [XmlElement(ElementName = "DescripcionesEspecificas", Namespace = Namespaces.ComercioExterior20)]
    public List<DescripcionesEspecificasComercioExterior20>? DescripcionesEspecificas { get; set; }

    public bool ShouldSerializeDescripcionesEspecificas() =>
        DescripcionesEspecificas != null && DescripcionesEspecificas.Count > 0;
}

public class DomicilioComercioExterior20
{
    [XmlAttribute(AttributeName = "Calle")]
    [JsonPropertyName("Calle")]
    public string Calle { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("NoExterior")]
    [XmlAttribute(AttributeName = "NumeroExterior")]
    public string? NoExterior { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("NoInterior")]
    [XmlAttribute(AttributeName = "NumeroInterior")]
    public string? NoInterior { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("Colonia")]
    [XmlAttribute(AttributeName = "Colonia")]
    public string? Colonia { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("Localidad")]
    [XmlAttribute(AttributeName = "Localidad")]
    public string? Localidad { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("Referencia")]
    [XmlAttribute(AttributeName = "Referencia")]
    public string? Referencia { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("Municipio")]
    [XmlAttribute(AttributeName = "Municipio")]
    public string? Municipio { get; set; }

    [XmlAttribute(AttributeName = "Estado")]
    [JsonPropertyName("Estado")]
    public string Estado { get; set; }

    [XmlAttribute(AttributeName = "Pais")]
    [JsonPropertyName("Pais")]
    public string Pais { get; set; }

    [XmlAttribute(AttributeName = "CodigoPostal")]
    [JsonPropertyName("CodigoPostal")]
    public string CodigoPostal { get; set; }
}

public class DescripcionesEspecificasComercioExterior20
{
    [XmlAttribute(AttributeName = "Marca")]
    [JsonPropertyName("Marca")]
    public string Marca { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("Modelo")]
    [XmlAttribute(AttributeName = "Modelo")]
    public string? Modelo { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("Submodelo")]
    [XmlAttribute(AttributeName = "SubModelo")]
    public string? Submodelo { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("NoSerie")]
    [XmlAttribute(AttributeName = "NumeroSerie")]
    public string? NoSerie { get; set; }
}