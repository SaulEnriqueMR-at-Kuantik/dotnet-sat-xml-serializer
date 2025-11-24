using System.Text.Json.Serialization;
using System.Xml.Serialization;
using KpacModels.Shared.Models.Constants;

namespace KpacModels.Shared.Models.Comprobante.Complementos.Nomina;

public class Receptor
{
    [XmlAttribute(AttributeName = "Curp")]
    [JsonPropertyName("Curp")]
    public string Curp { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("NoSeguridadSocial")]
    [XmlAttribute(AttributeName = "NumSeguridadSocial")]
    public string? NoSeguridadSocial { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("FechaInicioRelacionLaboral")]
    [XmlAttribute(AttributeName = "FechaInicioRelLaboral")]
    public string? FechaInicioRelacionLaboral { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("Antiguedad")]
    [XmlAttribute(AttributeName = "Antigüedad")]
    public string? Antiguedad { get; set; }

    [XmlAttribute(AttributeName = "TipoContrato")]
    [JsonPropertyName("TipoContrato")]
    public string TipoContrato { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("Sindicalizado")]
    [XmlAttribute(AttributeName = "Sindicalizado")]
    public string? Sindicalizado { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("TipoJornada")]
    [XmlAttribute(AttributeName = "TipoJornada")]
    public string? TipoJornada { get; set; }

    [XmlAttribute(AttributeName = "TipoRegimen")]
    [JsonPropertyName("TipoRegimen")]
    public string TipoRegimen { get; set; }

    [XmlAttribute(AttributeName = "NumEmpleado")]
    [JsonPropertyName("NoEmpleado")]
    public string NoEmpleado { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("Departamento")]
    [XmlAttribute(AttributeName = "Departamento")]
    public string? Departamento { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("Puesto")]
    [XmlAttribute(AttributeName = "Puesto")]
    public string? Puesto { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("RiesgoPuesto")]
    [XmlAttribute(AttributeName = "RiesgoPuesto")]
    public string? RiesgoPuesto { get; set; }

    [XmlAttribute(AttributeName = "PeriodicidadPago")]
    [JsonPropertyName("PeriodicidadPago")]
    public string PeriodicidadPago { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("Banco")]
    [XmlAttribute(AttributeName = "Banco")]
    public string? Banco { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("CuentaBancaria")]
    [XmlAttribute(AttributeName = "CuentaBancaria")]
    public string? CuentaBancaria { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("SalarioBaseCotizacion")]
    [XmlAttribute(AttributeName = "SalarioBaseCotApor")]
    public string? SalarioBaseCotizacion { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("SalarioDiarioIntegrado")]
    [XmlAttribute(AttributeName = "SalarioDiarioIntegrado")]
    public string? SalarioDiarioIntegrado { get; set; }

    [XmlAttribute(AttributeName = "ClaveEntFed")]
    [JsonPropertyName("ClaveEntidadFederativa")]
    public string ClaveEntidadFederativa { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("Subcontratacion")]
    [XmlElement(ElementName = "SubContratacion", Namespace = Namespaces.Nomina12)]
    public List<SubContratacion>? Subcontratacion { get; set; }

    public bool ShouldSerializeSubcontratacion() => Subcontratacion != null && Subcontratacion.Count > 0;
}