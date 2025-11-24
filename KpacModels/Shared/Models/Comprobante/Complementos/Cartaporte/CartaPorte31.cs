using System.Text.Json.Serialization;
using System.Xml.Serialization;
using KpacModels.Shared.Models.Constants;

namespace KpacModels.Shared.Models.Comprobante.Complementos.Cartaporte;

[XmlRoot(ElementName = "CartaPorte", Namespace = Namespaces.CartaPorte31)]
public class CartaPorte31
{
    [XmlAttribute(AttributeName = "Version")]
    [JsonPropertyName("Version")]
    public string Version { get; set; }

    [XmlAttribute(AttributeName = "IdCCP")]
    [JsonPropertyName("IdCcp")]
    public string IdCcp { get; set; }

    [XmlAttribute(AttributeName = "TranspInternac")]
    [JsonPropertyName("TransporteInternacional")]
    public string TransporteInternacional { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("EntradaSalidaMercancia")]
    [XmlAttribute(AttributeName = "EntradaSalidaMerc")]
    public string? EntradaSalidaMercancia { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("PaisOrigenDestino")]
    [XmlAttribute(AttributeName = "PaisOrigenDestino")]
    public string? PaisOrigenDestino { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("ViaEntradaSalida")]
    [XmlAttribute(AttributeName = "ViaEntradaSalida")]
    public string? ViaEntradaSalida { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("TotalDistanciaRecorrida")]
    [XmlAttribute(AttributeName = "TotalDistRec")]
    public string? TotalDistanciaRecorrida { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("RegistroIstmo")]
    [XmlAttribute(AttributeName = "RegistroISTMO")]
    public string? RegistroIstmo { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("UbicacionPoloOrigen")]
    [XmlAttribute(AttributeName = "UbicacionPoloOrigen")]
    public string? UbicacionPoloOrigen { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("UbicacionPoloDestino")]
    [XmlAttribute(AttributeName = "UbicacionPoloDestino")]
    public string? UbicacionPoloDestino { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("RegimenesAduaneros")]
    [XmlArray(ElementName = "RegimenesAduaneros", Namespace = Namespaces.CartaPorte31)]
    [XmlArrayItem(ElementName = "RegimenAduaneroCCP", Namespace = Namespaces.CartaPorte31)]
    public List<RegimenAduaneroCartaPorte31>? RegimenesAduaneros { get; set; }

    public bool ShouldSerializeRegimenesAduaneros() => RegimenesAduaneros != null && RegimenesAduaneros.Count > 0;

    [XmlArray(ElementName = "Ubicaciones", Namespace = Namespaces.CartaPorte31)]
    [XmlArrayItem(ElementName = "Ubicacion", Namespace = Namespaces.CartaPorte31)]
    [JsonPropertyName("Ubicaciones")]
    public List<UbicacionCartaPorte31> Ubicaciones { get; set; }

    public bool ShouldSerializeUbicaciones() => Ubicaciones != null && Ubicaciones.Count > 0;

    [XmlElement(ElementName = "Mercancias", Namespace = Namespaces.CartaPorte31)]
    [JsonPropertyName("Mercancias")]
    public MercanciasCartaPorte31? Mercancias { get; set; }

    public bool ShouldSerializeMercancias() => Mercancias != null && Mercancias.PesoBrutoTotal != null;

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("FiguraTransporte")]
    [XmlElement(ElementName = "FiguraTransporte", Namespace = Namespaces.CartaPorte31)]
    public FiguraCartaPorte31? FiguraTransporte { get; set; }

    public bool ShouldSerializeFiguraTransporte() => FiguraTransporte != null;
}

public class RegimenAduaneroCartaPorte31
{
    [XmlAttribute(AttributeName = "RegimenAduanero")]
    [JsonPropertyName("RegimenAduanero")]
    public string RegimenAduanero { get; set; }
}

public class UbicacionCartaPorte31
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [XmlIgnore]
    public string? ConsecutivoUbicacion { set; get; }

    [XmlAttribute(AttributeName = "TipoUbicacion")]
    [JsonPropertyName("TipoUbicacion")]
    public string TipoUbicacion { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("IdUbicacion")]
    [XmlAttribute(AttributeName = "IDUbicacion")]
    public string? IdUbicacion { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("RfcRemitenteDestinatario")]
    [XmlAttribute(AttributeName = "RFCRemitenteDestinatario")]
    public string RfcRemitenteDestinatario { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("NombreRemitenteDestinatario")]
    [XmlAttribute(AttributeName = "NombreRemitenteDestinatario")]
    public string? NombreRemitenteDestinatario { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("NumRegIdTrib")]
    [XmlAttribute(AttributeName = "NumRegIdTrib")]
    public string? NumRegIdTrib { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("ResidenciaFiscal")]
    [XmlAttribute(AttributeName = "ResidenciaFiscal")]
    public string? ResidenciaFiscal { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("NoEstacion")]
    [XmlAttribute(AttributeName = "NumEstacion")]
    public string? NoEstacion { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("NombreEstacion")]
    [XmlAttribute(AttributeName = "NombreEstacion")]
    public string? NombreEstacion { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("NavegacionTrafico")]
    [XmlAttribute(AttributeName = "NavegacionTrafico")]
    public string? NavegacionTrafico { get; set; }
    
    [JsonPropertyName("FechaHoraSalidaLlegada")]
    [XmlAttribute(AttributeName = "FechaHoraSalidaLlegada")]
    public string FechaHoraSalidaLlegada { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("TipoEstacion")]
    [XmlAttribute(AttributeName = "TipoEstacion")]
    public string? TipoEstacion { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("DistanciaRecorrida")]
    [XmlAttribute(AttributeName = "DistanciaRecorrida")]
    public string? DistanciaRecorrida { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("Domicilio")]
    [XmlElement(ElementName = "Domicilio", Namespace = Namespaces.CartaPorte31)]
    public DomicilioCartaPorte31? Domicilio { get; set; }
}

public class DomicilioCartaPorte31
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [XmlIgnore]
    public string? IdTipoFigura { set; get; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [XmlIgnore]
    public string? IdUbicacion { set; get; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("Calle")]
    [XmlAttribute(AttributeName = "Calle")]
    public string? Calle { get; set; }

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

public class MercanciasCartaPorte31
{
    [JsonPropertyName("PesoBrutoTotal")]
    [XmlAttribute(AttributeName = "PesoBrutoTotal")]
    public string PesoBrutoTotal { get; set; }

    [XmlAttribute(AttributeName = "UnidadPeso")]
    [JsonPropertyName("UnidadPeso")]
    public string UnidadPeso { get; set; }
    
    [JsonPropertyName("PesoNetoTotal")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [XmlAttribute(AttributeName = "PesoNetoTotal")]
    public string? PesoNetoTotal { get; set; }

    [XmlAttribute(AttributeName = "NumTotalMercancias")]
    [JsonPropertyName("NumeroTotalMercancias")]
    public string NumeroTotalMercancias { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("CargoPorTasacion")]
    [XmlAttribute(AttributeName = "CargoPorTasacion")]
    public string? CargoPorTasacion { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("LogisticaInversaRecoleccionDevolucion")]
    [XmlAttribute(AttributeName = "LogisticaInversaRecoleccionDevolucion")]
    public string? LogisticaInversaRecoleccionDevolucion { get; set; }

    [XmlElement(ElementName = "Mercancia", Namespace = Namespaces.CartaPorte31)]
    [JsonPropertyName("Mercancia")]
    public List<MercanciaCartaPorte31> Mercancia { get; set; }

    public bool ShouldSerializeMercancia() => Mercancia != null && Mercancia.Count > 0;

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("Autotransporte")]
    [XmlElement(ElementName = "Autotransporte", Namespace = Namespaces.CartaPorte31)]
    public AutoTransporteCartaPorte31? Autotransporte { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("TransporteMaritimo")]
    [XmlElement(ElementName = "TransporteMaritimo", Namespace = Namespaces.CartaPorte31)]
    public TransporteMaritimoCartaPorte31? TransporteMaritimo { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("TransporteAereo")]
    [XmlElement(ElementName = "TransporteAereo", Namespace = Namespaces.CartaPorte31)]
    public TransporteAereoCartaPorte31? TransporteAereo { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("TransporteFerroviario")]
    [XmlElement(ElementName = "TransporteFerroviario", Namespace = Namespaces.CartaPorte31)]
    public TransporteFerroviarioCartaPorte31? TransporteFerroviario { get; set; }
}

public class MercanciaCartaPorte31
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [XmlIgnore]
    public string? IdMercancia { set; get; }

    [XmlAttribute(AttributeName = "BienesTransp")]
    [JsonPropertyName("BienesTransportados")]
    public string BienesTransportados { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("ClaveStcc")]
    [XmlAttribute(AttributeName = "ClaveSTCC")]
    public string? ClaveStcc { get; set; }

    [XmlAttribute(AttributeName = "Descripcion")]
    [JsonPropertyName("Descripcion")]
    public string Descripcion { get; set; }
    
    [JsonPropertyName("Cantidad")]
    [XmlAttribute(AttributeName = "Cantidad")]
    public string Cantidad { get; set; }

    [XmlAttribute(AttributeName = "ClaveUnidad")]
    [JsonPropertyName("ClaveUnidad")]
    public string ClaveUnidad { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("Unidad")]
    [XmlAttribute(AttributeName = "Unidad")]
    public string? Unidad { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("Dimensiones")]
    [XmlAttribute(AttributeName = "Dimensiones")]
    public string? Dimensiones { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("MaterialPeligroso")]
    [XmlAttribute(AttributeName = "MaterialPeligroso")]
    public string? MaterialPeligroso { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("ClaveMaterialPeligroso")]
    [XmlAttribute(AttributeName = "CveMaterialPeligroso")]
    public string? ClaveMaterialPeligroso { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("Embalaje")]
    [XmlAttribute(AttributeName = "Embalaje")]
    public string? Embalaje { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("DescripcionEmbalaje")]
    [XmlAttribute(AttributeName = "DescripEmbalaje")]
    public string? DescripcionEmbalaje { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("SectorCofepris")]
    [XmlAttribute(AttributeName = "SectorCOFEPRIS")]
    public string? SectorCofepris { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("NombreIngredienteActivo")]
    [XmlAttribute(AttributeName = "NombreIngredienteActivo")]
    public string? NombreIngredienteActivo { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("NombreQuimico")]
    [XmlAttribute(AttributeName = "NomQuimico")]
    public string? NombreQuimico { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("DenominacionGenericaProducto")]
    [XmlAttribute(AttributeName = "DenominacionGenericaProd")]
    public string? DenominacionGenericaProducto { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("DenominacionDistintivaProducto")]
    [XmlAttribute(AttributeName = "DenominacionDistintivaProd")]
    public string? DenominacionDistintivaProducto { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("Fabricante")]
    [XmlAttribute(AttributeName = "Fabricante")]
    public string? Fabricante { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("FechaCaducidad")]
    [XmlAttribute(AttributeName = "FechaCaducidad")]
    public string? FechaCaducidad { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("LoteMedicamento")]
    [XmlAttribute(AttributeName = "LoteMedicamento")]
    public string? LoteMedicamento { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("FormaFarmaceutica")]
    [XmlAttribute(AttributeName = "FormaFarmaceutica")]
    public string? FormaFarmaceutica { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("CondicionesEspecialesTransporte")]
    [XmlAttribute(AttributeName = "CondicionesEspTransp")]
    public string? CondicionesEspecialesTransporte { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("RegistroSanitarioFolioAutorizacion")]
    [XmlAttribute(AttributeName = "RegistroSanitarioFolioAutorizacion")]
    public string? RegistroSanitarioFolioAutorizacion { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("PermisoImportacion")]
    [XmlAttribute(AttributeName = "PermisoImportacion")]
    public string? PermisoImportacion { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("FolioImportacionVucem")]
    [XmlAttribute(AttributeName = "FolioImpoVUCEM")]
    public string? FolioImportacionVucem { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("NoCas")]
    [XmlAttribute(AttributeName = "NumCAS")]
    public string? NoCas { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("RazonSocialEmpresaImportadora")]
    [XmlAttribute(AttributeName = "RazonSocialEmpImp")]
    public string? RazonSocialEmpresaImportadora { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("NoRegistroSanitarioPlaguicidasCofepris")]
    [XmlAttribute(AttributeName = "NumRegSanPlagCOFEPRIS")]
    public string? NoRegistroSanitarioPlaguicidasCofepris { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("DatosFabricante")]
    [XmlAttribute(AttributeName = "DatosFabricante")]
    public string? DatosFabricante { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("DatosFormulador")]
    [XmlAttribute(AttributeName = "DatosFormulador")]
    public string? DatosFormulador { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("DatosMaquilador")]
    [XmlAttribute(AttributeName = "DatosMaquilador")]
    public string? DatosMaquilador { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("UsoAutorizado")]
    [XmlAttribute(AttributeName = "UsoAutorizado")]
    public string? UsoAutorizado { get; set; }

    [XmlAttribute(AttributeName = "PesoEnKg")]
    [JsonPropertyName("PesoKg")]
    public string PesoKg { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("ValorMercancia")]
    [XmlAttribute(AttributeName = "ValorMercancia")]
    public string? ValorMercancia { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("Moneda")]
    [XmlAttribute(AttributeName = "Moneda")]
    public string? Moneda { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("FraccionArancelaria")]
    [XmlAttribute(AttributeName = "FraccionArancelaria")]
    public string? FraccionArancelaria { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("UuidComercioExterior")]
    [XmlAttribute(AttributeName = "UUIDComercioExt")]
    public string? UuidComercioExterior { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("TipoMateria")]
    [XmlAttribute(AttributeName = "TipoMateria")]
    public string? TipoMateria { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("DescripcionMateria")]
    [XmlAttribute(AttributeName = "DescripcionMateria")]
    public string? DescripcionMateria { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("DocumentacionAduanera")]
    [XmlElement(ElementName = "DocumentacionAduanera", Namespace = Namespaces.CartaPorte31)]
    public List<DocumentacionAduaneraCartaPorte31>? DocumentacionAduanera { get; set; }

    public bool ShouldSerializeDocumentacionAduanera() =>
        DocumentacionAduanera != null && DocumentacionAduanera.Count > 0;

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("GuiasIdentificacion")]
    [XmlElement(ElementName = "GuiasIdentificacion", Namespace = Namespaces.CartaPorte31)]
    public List<GuiasIdentificacionCartaPorte31>? GuiasIdentificacion { get; set; }

    public bool ShouldSerializeGuiasIdentificacion() => GuiasIdentificacion != null && GuiasIdentificacion.Count > 0;

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("CantidadTransporta")]
    [XmlElement(ElementName = "CantidadTransporta", Namespace = Namespaces.CartaPorte31)]
    public List<CantidadTransporteCartaPorte31>? CantidadTransporta { get; set; }

    public bool ShouldSerializeCantidadTransporta() => CantidadTransporta != null && CantidadTransporta.Count > 0;

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("DetalleMercancia")]
    [XmlElement(ElementName = "DetalleMercancia", Namespace = Namespaces.CartaPorte31)]
    public DetalleMercanciaCartaPorte31? DetalleMercancia { get; set; }

    public bool ShouldSerializeDetalleMercancia() => DetalleMercancia != null;
}

public class DocumentacionAduaneraCartaPorte31
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [XmlIgnore]
    public string? IdMercancia { set; get; }

    [XmlAttribute(AttributeName = "TipoDocumento")]
    [JsonPropertyName("Tipo")]
    public string Tipo { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("NumeroPedimento")]
    [XmlAttribute(AttributeName = "NumPedimento")]
    public string? NumeroPedimento { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("IdentificadorDocumento")]
    [XmlAttribute(AttributeName = "IdentDocAduanero")]
    public string? IdentificadorDocumento { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("RfcImportador")]
    [XmlAttribute(AttributeName = "RFCImpo")]
    public string? RfcImportador { get; set; }
}

public class GuiasIdentificacionCartaPorte31
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [XmlIgnore]
    public string? IdMercancia { get; set; }

    [XmlAttribute(AttributeName = "NumeroGuiaIdentificacion")]
    [JsonPropertyName("Numero")]
    public string Numero { get; set; }

    [XmlAttribute(AttributeName = "DescripGuiaIdentificacion")]
    [JsonPropertyName("Descripcion")]
    public string Descripcion { get; set; }

    [XmlAttribute(AttributeName = "PesoGuiaIdentificacion")]
    [JsonPropertyName("Peso")]
    public string Peso { get; set; }
}

public class CantidadTransporteCartaPorte31
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [XmlIgnore]
    public string? IdMercancia { set; get; }

    [XmlAttribute(AttributeName = "Cantidad")]
    [JsonPropertyName("Cantidad")]
    public string Cantidad { get; set; }

    [XmlAttribute(AttributeName = "IDOrigen")]
    [JsonPropertyName("IdOrigen")]
    public string IdOrigen { get; set; }

    [XmlAttribute(AttributeName = "IDDestino")]
    [JsonPropertyName("IdDestino")]
    public string IdDestino { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("ClavesTransporte")]
    [XmlAttribute(AttributeName = "CvesTransporte")]
    public string? ClavesTransporte { get; set; }
}

public class DetalleMercanciaCartaPorte31
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [XmlIgnore]
    public string? IdMercancia { set; get; }

    [XmlAttribute(AttributeName = "UnidadPesoMerc")]
    [JsonPropertyName("UnidadPeso")]
    public string UnidadPeso { get; set; }

    [XmlAttribute(AttributeName = "PesoBruto")]
    [JsonPropertyName("PesoBruto")]
    public string PesoBruto { get; set; }

    [XmlAttribute(AttributeName = "PesoNeto")]
    [JsonPropertyName("PesoNeto")]
    public string PesoNeto { get; set; }

    [XmlAttribute(AttributeName = "PesoTara")]
    [JsonPropertyName("PesoTara")]
    public string PesoTara { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("NumeroPiezas")]
    [XmlAttribute(AttributeName = "NumPiezas")]
    public string? NumeroPiezas { get; set; }
}

public class AutoTransporteCartaPorte31
{
    [XmlAttribute(AttributeName = "PermSCT")]
    [JsonPropertyName("PermisoSct")]
    public string PermisoSct { get; set; }

    [XmlAttribute(AttributeName = "NumPermisoSCT")]
    [JsonPropertyName("NoPermisoSct")]
    public string NoPermisoSct { get; set; }

    [XmlElement(ElementName = "IdentificacionVehicular", Namespace = Namespaces.CartaPorte31)]
    [JsonPropertyName("IdentificacionVehicular")]
    public IdentificacionVehicularCartaPorte31 IdentificacionVehicular { get; set; }

    [XmlElement(ElementName = "Seguros", Namespace = Namespaces.CartaPorte31)]
    [JsonPropertyName("Seguros")]
    public SegurosCartaPorte31 Seguros { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("Remolques")]
    [XmlArray(ElementName = "Remolques", Namespace = Namespaces.CartaPorte31)]
    [XmlArrayItem(ElementName = "Remolque", Namespace = Namespaces.CartaPorte31)]
    public List<RemolqueCartaPorte31>? Remolques { get; set; }

    public bool ShouldSerializeRemolques() => Remolques != null && Remolques.Count > 0;
}

public class IdentificacionVehicularCartaPorte31
{
    [XmlAttribute(AttributeName = "ConfigVehicular")]
    [JsonPropertyName("Configuracion")]
    public string Configuracion { get; set; }

    [XmlAttribute(AttributeName = "PesoBrutoVehicular")]
    [JsonPropertyName("PesoBruto")]
    public string PesoBruto { get; set; }

    [XmlAttribute(AttributeName = "PlacaVM")]
    [JsonPropertyName("PlacaVm")]
    public string PlacaVm { get; set; }

    [XmlAttribute(AttributeName = "AnioModeloVM")]
    [JsonPropertyName("AnioModeloVm")]
    public string AnioModeloVm { get; set; }
}

public class SegurosCartaPorte31
{
    [XmlAttribute(AttributeName = "AseguraRespCivil")]
    [JsonPropertyName("AseguradoraResponsabilidadCivil")]
    public string AseguradoraResponsabilidadCivil { get; set; }

    [XmlAttribute(AttributeName = "PolizaRespCivil")]
    [JsonPropertyName("PolizaResponsabilidadCivil")]
    public string PolizaResponsabilidadCivil { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("AseguradoraMedioAmbiente")]
    [XmlAttribute(AttributeName = "AseguraMedAmbiente")]
    public string? AseguradoraMedioAmbiente { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("PolizaMedioAmbiente")]
    [XmlAttribute(AttributeName = "PolizaMedAmbiente")]
    public string? PolizaMedioAmbiente { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("AseguradoraCarga")]
    [XmlAttribute(AttributeName = "AseguraCarga")]
    public string? AseguradoraCarga { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("PolizaCarga")]
    [XmlAttribute(AttributeName = "PolizaCarga")]
    public string? PolizaCarga { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("PrimaSeguro")]
    [XmlAttribute(AttributeName = "PrimaSeguro")]
    public string? PrimaSeguro { get; set; }
}

public class RemolqueCartaPorte31
{
    [XmlAttribute(AttributeName = "SubTipoRem")]
    [JsonPropertyName("Subtipo")]
    public string Subtipo { get; set; }

    [XmlAttribute(AttributeName = "Placa")]
    [JsonPropertyName("Placa")]
    public string Placa { get; set; }
}

public class TransporteMaritimoCartaPorte31
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("PermisoSct")]
    [XmlAttribute(AttributeName = "PermSCT")]
    public string? PermisoSct { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("NoPermisoSct")]
    [XmlAttribute(AttributeName = "NumPermisoSCT")]
    public string? NoPermisoSct { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("NombreAseguradora")]
    [XmlAttribute(AttributeName = "NombreAseg")]
    public string? NombreAseguradora { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("NoPolizaSeguro")]
    [XmlAttribute(AttributeName = "NumPolizaSeguro")]
    public string? NoPolizaSeguro { get; set; }

    [XmlAttribute(AttributeName = "TipoEmbarcacion")]
    [JsonPropertyName("TipoEmbarcacion")]
    public string TipoEmbarcacion { get; set; }

    [XmlAttribute(AttributeName = "Matricula")]
    [JsonPropertyName("Matricula")]
    public string Matricula { get; set; }

    [XmlAttribute(AttributeName = "NumeroOMI")]
    [JsonPropertyName("NoOmi")]
    public string NoOmi { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("AnioEmbarcacion")]
    [XmlAttribute(AttributeName = "AnioEmbarcacion")]
    public string? AnioEmbarcacion { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("NombreEmbarcacion")]
    [XmlAttribute(AttributeName = "NombreEmbarc")]
    public string? NombreEmbarcacion { get; set; }

    [XmlAttribute(AttributeName = "NacionalidadEmbarc")]
    [JsonPropertyName("NacionalidadEmbarcacion")]
    public string NacionalidadEmbarcacion { get; set; }

    [XmlAttribute(AttributeName = "UnidadesDeArqBruto")]
    [JsonPropertyName("UnidadesArqueoBruto")]
    public string UnidadesArqueoBruto { get; set; }

    [XmlAttribute(AttributeName = "TipoCarga")]
    [JsonPropertyName("TipoCarga")]
    public string TipoCarga { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("Eslora")]
    [XmlAttribute(AttributeName = "Eslora")]
    public string? Eslora { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("Manga")]
    [XmlAttribute(AttributeName = "Manga")]
    public string? Manga { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("Calado")]
    [XmlAttribute(AttributeName = "Calado")]
    public string? Calado { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("Puntal")]
    [XmlAttribute(AttributeName = "Puntal")]
    public string? Puntal { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("LineaNaviera")]
    [XmlAttribute(AttributeName = "LineaNaviera")]
    public string? LineaNaviera { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("NombreAgenteNaviero")]
    [XmlAttribute(AttributeName = "NombreAgenteNaviero")]
    public string NombreAgenteNaviero { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("NoAutorizacionNaviero")]
    [XmlAttribute(AttributeName = "NumAutorizacionNaviero")]
    public string NoAutorizacionNaviero { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("NoViaje")]
    [XmlAttribute(AttributeName = "NumViaje")]
    public string? NoViaje { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("NoConocimientoEmbarque")]
    [XmlAttribute(AttributeName = "NumConocEmbarc")]
    public string? NoConocimientoEmbarque { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("PermisoTemporalNavegacion")]
    [XmlAttribute(AttributeName = "PermisoTempNavegacion")]
    public string? PermisoTemporalNavegacion { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("Contenedor")]
    [XmlElement(ElementName = "Contenedor", Namespace = Namespaces.CartaPorte31)]
    public List<ContenedorCartaPorte31>? Contenedor { get; set; }

    public bool ShouldSerializeContenedor() => Contenedor != null && Contenedor.Count > 0;
}

public class ContenedorCartaPorte31
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [XmlIgnore]
    public string? IdentificadorContenedor { set; get; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("Tipo")]
    [XmlAttribute(AttributeName = "TipoContenedor")]
    public string Tipo { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("Matricula")]
    [XmlAttribute(AttributeName = "MatriculaContenedor")]
    public string? Matricula { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("NoPrecinto")]
    [XmlAttribute(AttributeName = "NumPrecinto")]
    public string? NoPrecinto { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("IdCcpRelacionado")]
    [XmlAttribute(AttributeName = "IdCCPRelacionado")]
    public string? IdCcpRelacionado { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("PlacaVmCcp")]
    [XmlAttribute(AttributeName = "PlacaVMCCP")]
    public string? PlacaVmCcp { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("FechaCertificacionCcp")]
    [XmlAttribute(AttributeName = "FechaCertificacionCCP")]
    public string? FechaCertificacionCcp { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("RemolquesCcp")]
    [XmlArray(ElementName = "RemolquesCCP", Namespace = Namespaces.CartaPorte31)]
    [XmlArrayItem(ElementName = "RemolqueCCP", Namespace = Namespaces.CartaPorte31)]
    public List<RemolquesCcpCartaPorte31>? RemolquesCcp { get; set; }

    public bool ShouldSerializeRemolquesCcp() => RemolquesCcp != null && RemolquesCcp?.Count > 0;
}

public class RemolquesCcpCartaPorte31
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [XmlIgnore]
    public string? IdentificadorContenedor { set; get; }

    [XmlAttribute(AttributeName = "SubTipoRemCCP")]
    [JsonPropertyName("Subtipo")]
    public string Subtipo { get; set; }

    [XmlAttribute(AttributeName = "PlacaCCP")]
    [JsonPropertyName("PlacaCCP")]
    public string Placa { get; set; }
}

public class TransporteAereoCartaPorte31
{
    [XmlAttribute(AttributeName = "PermSCT")]
    [JsonPropertyName("PermisoSct")]
    public string PermisoSct { get; set; }

    [XmlAttribute(AttributeName = "NumPermisoSCT")]
    [JsonPropertyName("NoPermisoSct")]
    public string NoPermisoSct { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("MatriculaAeronave")]
    [XmlAttribute(AttributeName = "MatriculaAeronave")]
    public string MatriculaAeronave { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("NombreAseguradora")]
    [XmlAttribute(AttributeName = "NombreAseg")]
    public string NombreAseguradora { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("NoPolizaSeguro")]
    [XmlAttribute(AttributeName = "NumPolizaSeguro")]
    public string NoPolizaSeguro { get; set; }

    [XmlAttribute(AttributeName = "NumeroGuia")]
    [JsonPropertyName("NoGuia")]
    public string NoGuia { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("LugarContrato")]
    [XmlAttribute(AttributeName = "LugarContrato")]
    public string? LugarContrato { get; set; }

    [XmlAttribute(AttributeName = "CodigoTransportista")]
    [JsonPropertyName("CodigoTransportista")]
    public string CodigoTransportista { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("RfcEmbarcador")]
    [XmlAttribute(AttributeName = "RFCEmbarcador")]
    public string? RfcEmbarcador { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("NumRegIdTribEmbarcador")]
    [XmlAttribute(AttributeName = "NumRegIdTribEmbarc")]
    public string? NumRegIdTribEmbarcador { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("ResidenciaFiscalEmbarcador")]
    [XmlAttribute(AttributeName = "ResidenciaFiscalEmbarc")]
    public string? ResidenciaFiscalEmbarcador { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("NombreEmbarcador")]
    [XmlAttribute(AttributeName = "NombreEmbarcador")]
    public string? NombreEmbarcador { get; set; }
}

public class TransporteFerroviarioCartaPorte31
{
    [XmlAttribute(AttributeName = "TipoDeServicio")]
    [JsonPropertyName("TipoServicio")]
    public string TipoServicio { get; set; }

    [XmlAttribute(AttributeName = "TipoDeTrafico")]
    [JsonPropertyName("TipoTrafico")]
    public string TipoTrafico { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("NombreAseguradora")]
    [XmlAttribute(AttributeName = "NombreAseg")]
    public string? NombreAseguradora { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("NoPolizaSeguro")]
    [XmlAttribute(AttributeName = "NumPolizaSeguro")]
    public string? NoPolizaSeguro { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("DerechosDePaso")]
    [XmlElement(ElementName = "DerechosDePaso", Namespace = Namespaces.CartaPorte31)]
    public List<DerechosDePasoCartaPorte31>? DerechosDePaso { get; set; }

    public bool ShouldSerializeDerechosDePaso() => DerechosDePaso != null && DerechosDePaso.Count > 0;

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("Carro")]
    [XmlElement(ElementName = "Carro", Namespace = Namespaces.CartaPorte31)]
    public List<CarroCartaPorte31> Carro { get; set; }

    public bool ShouldSerializeCarro() => Carro != null && Carro.Count > 0;
}

public class DerechosDePasoCartaPorte31
{
    [XmlAttribute(AttributeName = "TipoDerechoDePaso")]
    [JsonPropertyName("Tipo")]
    public string Tipo { get; set; }

    [XmlAttribute(AttributeName = "KilometrajePagado")]
    [JsonPropertyName("KilometrajePagado")]
    public string KilometrajePagado { get; set; }
}

public class CarroCartaPorte31
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [XmlIgnore]
    public string? IdCarro { set; get; }

    [XmlAttribute(AttributeName = "TipoCarro")]
    [JsonPropertyName("Tipo")]
    public string Tipo { get; set; }

    [XmlAttribute(AttributeName = "MatriculaCarro")]
    [JsonPropertyName("Matricula")]
    public string Matricula { get; set; }

    [XmlAttribute(AttributeName = "GuiaCarro")]
    [JsonPropertyName("Guia")]
    public string Guia { get; set; }

    [XmlAttribute(AttributeName = "ToneladasNetasCarro")]
    [JsonPropertyName("ToneladasNetasCarro")]
    public string ToneladasNetas { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("Contenedor")]
    [XmlElement(ElementName = "Contenedor", Namespace = Namespaces.CartaPorte31)]
    public List<ContenedorCarroCataPorte31>? Contenedor { get; set; }

    public bool ShouldSerializeContenedor() => Contenedor != null && Contenedor.Count > 0;
}

public class ContenedorCarroCataPorte31
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [XmlIgnore]
    public string? IdCarro { set; get; }

    [XmlAttribute(AttributeName = "TipoContenedor")]
    [JsonPropertyName("Tipo")]
    public string Tipo { get; set; }

    [XmlAttribute(AttributeName = "PesoContenedorVacio")]
    [JsonPropertyName("PesoContenedorVacio")]
    public string PesoContenedorVacio { get; set; }

    [XmlAttribute(AttributeName = "PesoNetoMercancia")]
    [JsonPropertyName("PesoNetoMercancia")]
    public string PesoNetoMercancia { get; set; }
}

public class FiguraCartaPorte31
{
    [XmlElement(ElementName = "TiposFigura", Namespace = Namespaces.CartaPorte31)]
    [JsonPropertyName("TiposFigura")]
    public List<TiposFiguraCartaPorte31>? TiposFigura { get; set; }

    public bool ShouldSerializeTiposFigura() => TiposFigura != null && TiposFigura.Count > 0;
}

public class TiposFiguraCartaPorte31
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [XmlIgnore]
    public string? IdFigura { set; get; }

    [XmlAttribute(AttributeName = "TipoFigura")]
    [JsonPropertyName("Tipo")]
    public string Tipo { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("Rfc")]
    [XmlAttribute(AttributeName = "RFCFigura")]
    public string? Rfc { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("NoLicencia")]
    [XmlAttribute(AttributeName = "NumLicencia")]
    public string? NoLicencia { get; set; }

    [XmlAttribute(AttributeName = "NombreFigura")]
    [JsonPropertyName("Nombre")]
    public string Nombre { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("NumRegIdTrib")]
    [XmlAttribute(AttributeName = "NumRegIdTribFigura")]
    public string? NumRegIdTrib { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("ResidenciaFiscal")]
    [XmlAttribute(AttributeName = "ResidenciaFiscalFigura")]
    public string? ResidenciaFiscal { get; set; }

    [XmlElement("PartesTransporte", Namespace = Namespaces.CartaPorte31)]
    [JsonPropertyName("PartesTransporte")]
    public List<PartesTransporteCartaPorte31>? PartesTransporte { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("Domicilio")]
    [XmlElement(ElementName = "Domicilio", Namespace = Namespaces.CartaPorte31)]
    public DomicilioCartaPorte31? Domicilio { get; set; }

    public bool ShouldSerializeDomicilio() => Domicilio != null;
}

public class PartesTransporteCartaPorte31
{
    [XmlAttribute("ParteTransporte")] public string Parte { get; set; } = string.Empty;
}