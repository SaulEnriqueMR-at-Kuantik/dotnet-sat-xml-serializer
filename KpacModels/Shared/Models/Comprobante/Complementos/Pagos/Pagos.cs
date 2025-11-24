using System.Text.Json.Serialization;
using System.Xml.Serialization;
using KPac.Application.Validator;
using KpacModels.Shared.Constants;
using KpacModels.Shared.XmlProcessing.Formatter.Interface;
using KpacModels.Shared.XmlProcessing.Formatter.Pagos;
using KpacModels.Shared.XmlProcessing.Validator;
using KpacModels.Shared.XmlProcessing.Validator.Interface;

namespace KpacModels.Shared.Models.Comprobante.Complementos.Pagos;

[XmlRoot(ElementName = "Pagos", Namespace = Namespaces.Pagos20)]
public class Pagos20
{
    [XmlAttribute(AttributeName = "Version")]
    [JsonPropertyName("Version")]
    public string Version { get; set; }

    [XmlElement(ElementName = "Totales", Namespace = Namespaces.Pagos20)]
    [JsonPropertyName("Totales")]
    public Totales Totales { get; set; }

    [XmlElement(ElementName = "Pago", Namespace = Namespaces.Pagos20)]
    [JsonPropertyName("Pago")]
    public List<Pago> Pago { get; set; }

    public bool ShouldSerializePago() => Pago != null && Pago.Count > 0;

    public async Task Format(IVisitorFormatterPagos visitor)
    {
        visitor.Visit(this);
        var count = Pago?.Count;
        List<RetencionP> retencionesTotales = [];
        List<TrasladoP> trasladosTotales = [];
        var montoTotal = decimal.Zero;
        for (int i = 0; i < count; i++)
        {
            var pago = Pago[i];
            await pago.Accept(visitor, i + 1);
            var tipoCambio = decimal.Parse(pago.TipoCambio ?? "0");
            if (pago.Impuestos?.Retenciones is { Count: > 0 })
            {
                var retencionesOriginal = pago.Impuestos.Retenciones;
                var retenciones = retencionesOriginal
                    .Select(t => t.Clone())
                    .Cast<RetencionP>()
                    .ToArray();

                retencionesTotales.AddRange(
                    ImpuestosHelper.CalculateRetencionesP(retenciones, tipoCambio));
            }

            if (pago.Impuestos?.Traslados is { Count: > 0 })
            {
                var trasladosOriginal = pago.Impuestos.Traslados;
                var traslados = trasladosOriginal
                    .Select(t => t.Clone())
                    .Cast<TrasladoP>()
                    .ToArray();
                trasladosTotales.AddRange(
                    ImpuestosHelper.CalculateTrasladosP(traslados, tipoCambio));
            }

            montoTotal += PagosFormatHelper.CalculateMonto(pago.Monto, tipoCambio);
        }

        if (Totales == null)
            Totales = new Totales();
        Totales.Accept(visitor, montoTotal, retencionesTotales, trasladosTotales);
    }

    public async Task Accept(IVisitorPagos visitor)
    {
        visitor.Visit(this);
        if (visitor.HasErrors()) return;
        var count = Pago.Count;
        List<RetencionP> retencionesTotales = [];
        List<TrasladoP> trasladosTotales = [];
        for (int i = 0; i < count; i++)
        {
            var pago = Pago[i];
            var tipoCambio = decimal.Parse(pago.TipoCambio ?? "0");
            if (pago.Impuestos?.Retenciones is { Count: > 0 })
            {
                var retencionesOriginal = pago.Impuestos.Retenciones;
                var retenciones = retencionesOriginal
                    .Select(t => t.Clone())
                    .Cast<RetencionP>()
                    .ToArray();

                retencionesTotales.AddRange(
                    ImpuestosHelper.CalculateRetencionesP(retenciones, tipoCambio));
            }

            if (pago.Impuestos?.Traslados is { Count: > 0 })
            {
                var trasladosOriginal = pago.Impuestos.Traslados;
                var traslados = trasladosOriginal
                    .Select(t => t.Clone())
                    .Cast<TrasladoP>()
                    .ToArray();

                trasladosTotales.AddRange(
                    ImpuestosHelper.CalculateTrasladosP(traslados, tipoCambio));
            }

            await pago.Accept(visitor, i + 1);
        }

        if (visitor.HasErrors()) return;
        Totales.Accept(visitor, retencionesTotales, trasladosTotales);
    }
}