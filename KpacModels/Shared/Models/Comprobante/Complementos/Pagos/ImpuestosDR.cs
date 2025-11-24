using System.Text.Json.Serialization;
using System.Xml.Serialization;
using KpacModels.Shared.Models.Constants;
using KpacModels.Shared.XmlProcessing.Formatter.Interface;

namespace KpacModels.Shared.Models.Comprobante.Complementos.Pagos;

public class ImpuestosDR : ICloneable
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("Retenciones")]
    [XmlArray("RetencionesDR", Namespace = Namespaces.Pagos20)]
    [XmlArrayItem(ElementName = "RetencionDR", Namespace = Namespaces.Pagos20)]
    public List<RetencionDR>? Retenciones { get; set; }

    public bool ShouldSerializeRetenciones() => Retenciones is { Count: > 0 };

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("Traslados")]
    [XmlArray("TrasladosDR", Namespace = Namespaces.Pagos20)]
    [XmlArrayItem(ElementName = "TrasladoDR", Namespace = Namespaces.Pagos20)]
    public List<TrasladoDR>? Traslados { get; set; }

    public bool ShouldSerializeTraslados() => Traslados is { Count: > 0 };

    public void Accept(IVisitorFormatterPagos visitor, int numPago, int numDocto)
    {
        var countR = Retenciones?.Count;
        for (int i = 0; i < countR; i++)
        {
            var retencion = Retenciones[i];
            retencion.Accept(visitor, numPago, numDocto, i + 1);
        }

        var countT = Traslados?.Count;
        for (int i = 0; i < countT; i++)
        {
            var traslado = Traslados[i];
            traslado.Accept(visitor, numPago, numDocto, i + 1);
        }
    }

    public object Clone()
    {
        List<RetencionDR>? retenciones = [];
        List<TrasladoDR>? traslados = [];
        if (Retenciones != null && Retenciones.Count > 0)
        {
            foreach (var retencion in Retenciones)
            {
                var retencionClone = retencion.Clone() as RetencionDR;
                if (retencionClone != null)
                    retenciones.Add(retencionClone);
            }
        }

        if (Traslados != null && Traslados.Count > 0)
        {
            foreach (var traslado in Traslados)
            {
                var trasladoClone = traslado.Clone() as TrasladoDR;
                if (trasladoClone != null)
                    traslados.Add(trasladoClone);
            }
        }

        return new ImpuestosDR
        {
            Retenciones = retenciones,
            Traslados = traslados
        };
    }
}