using System.Text.Json.Serialization;
using System.Xml.Serialization;
using KpacModels.Shared.Constants;
using KpacModels.Shared.XmlProcessing.Formatter.Interface;
using KpacModels.Shared.XmlProcessing.Validator.Interface;

namespace KpacModels.Shared.Models.Comprobante.Complementos.Nomina;

[XmlRoot(ElementName = "Nomina", Namespace = Namespaces.Nomina12)]
public class Nomina12
{
    [XmlAttribute(AttributeName = "Version")] 
    [JsonPropertyName("Version")]
    public string Version = "1.2";

    [XmlAttribute(AttributeName = "TipoNomina")]
    [JsonPropertyName("TipoNomina")]
    public string TipoNomina { get; set; }

    [XmlAttribute(AttributeName = "FechaPago")]
    [JsonPropertyName("FechaPago")]
    public string FechaPago { get; set; }

    [XmlAttribute(AttributeName = "FechaInicialPago")]
    [JsonPropertyName("FechaInicialPago")]
    public string FechaInicialPago { get; set; }

    [XmlAttribute(AttributeName = "FechaFinalPago")]
    [JsonPropertyName("FechaFinalPago")]
    public string FechaFinalPago { get; set; }

    [XmlAttribute(AttributeName = "NumDiasPagados")]
    [JsonPropertyName("NumeroDiasPagados")]
    public string NumeroDiasPagados { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("TotalPercepciones")]
    [XmlAttribute(AttributeName = "TotalPercepciones")]
    public string? TotalPercepciones { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("TotalDeducciones")]
    [XmlAttribute(AttributeName = "TotalDeducciones")]
    public string? TotalDeducciones { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("TotalOtrosPagos")]
    [XmlAttribute(AttributeName = "TotalOtrosPagos")]
    public string? TotalOtrosPagos { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("Emisor")]
    [XmlElement(ElementName = "Emisor", Namespace = Namespaces.Nomina12)]
    public Emisor? Emisor { get; set; }

    public bool ShouldSerializeEmisor() => Emisor != null;

    [XmlElement(ElementName = "Receptor", Namespace = Namespaces.Nomina12)]
    [JsonPropertyName("Receptor")]
    public Receptor Receptor { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("Percepciones")]
    [XmlElement(ElementName = "Percepciones", Namespace = Namespaces.Nomina12)]
    public Percepciones? Percepciones { get; set; }

    public bool ShouldSerializePercepciones() => Percepciones != null;

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("Deducciones")]
    [XmlElement(ElementName = "Deducciones", Namespace = Namespaces.Nomina12)]
    public Deducciones? Deducciones { get; set; }

    public bool ShouldSerializeDeducciones() => Deducciones != null;

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("OtrosPagos")]
    [XmlArray(ElementName = "OtrosPagos", Namespace = Namespaces.Nomina12)]
    [XmlArrayItem(ElementName = "OtroPago", Namespace = Namespaces.Nomina12)]
    public List<OtroPago>? OtrosPagos { get; set; }

    public bool ShouldSerializeOtrosPagos() => OtrosPagos != null && OtrosPagos.Count > 0;

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("Incapacidades")]
    [XmlArray(ElementName = "Incapacidades", Namespace = Namespaces.Nomina12)]
    [XmlArrayItem(ElementName = "Incapacidad", Namespace = Namespaces.Nomina12)]
    public List<Incapacidad>? Incapacidades { get; set; }

    public bool ShouldSerializeIncapacidades() => Incapacidades != null && Incapacidades.Count > 0;
    
    public async Task Format(IVisitorFormatterNomina visitor)
    {
        visitor.Visit(this);
        visitor.Visit(Emisor);
        await visitor.Visit(Receptor);
        visitor.Visit(Percepciones);
        var countIncapacidades = Incapacidades?.Count ?? 0;
        for (var i = 0; i < countIncapacidades; i++)
        {
            var incapacidad = Incapacidades?[i];
            if (incapacidad != null)
                visitor.Visit(incapacidad, i + 1);
        }
        
        visitor.Visit(Incapacidades, Percepciones?.Percepcion);
        
        visitor.Visit(Deducciones);
        
        visitor.Visit(OtrosPagos);
        var countOtrosPagos = OtrosPagos?.Count ?? 0;
        for (var i = 0; i < countOtrosPagos; i++)
        {
            var otroPago = OtrosPagos?[i];
            if (otroPago != null)
                visitor.Visit(otroPago, i + 1);
        }
        visitor.VisitTotales(this);
    }

    public async Task Accept(IVisitorNomina visitor)
    {
        visitor.Visit(this);
        
        visitor.Visit(Emisor);
        
        visitor.Visit(Receptor);
        
        visitor.Visit(Percepciones);
        
        visitor.Visit(Deducciones);
        
        visitor.Visit(OtrosPagos);
        
        var countOtros =  OtrosPagos?.Count ?? 0;
        for (var i = 0; i < countOtros; i++)
        {
            var otroPago = OtrosPagos?[i];
            if (otroPago != null)
                visitor.Visit(otroPago, i + 1);
        }
        
        visitor.VisitTotales(this);
        
    }
}