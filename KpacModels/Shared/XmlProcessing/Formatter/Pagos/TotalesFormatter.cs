using System.Globalization;
using KpacModels.Shared.Models.Comprobante.Complementos.Pagos;
using KpacModels.Shared.XmlProcessing.Formatter.Common.Models;

namespace KpacModels.Shared.XmlProcessing.Formatter.Pagos;

public class TotalesFormatter
{

    private TotalesDecimal _totalesDecimal = new();

    private Totales _totales;
    
    private List<RetencionP> _retenciones;

    private List<TrasladoP> _traslados;
    
    public void Format(Totales totales, decimal montoTotal, List<RetencionP> retencionesTotales,
        List<TrasladoP> trasladosTotales)
    {
        _retenciones  = retencionesTotales;
        _traslados = trasladosTotales;
        _totales = totales;
        CalculateTotalesRetenciones();
        CalculateTotalesTraslados();
        _totales.MontoTotalPagos = Math.Round(montoTotal, 2).ToString(CultureInfo.InvariantCulture);
        RondTotals();
        _totalesDecimal.Clear();
    }

    private void CalculateTotalesRetenciones()
    {
        foreach (var retencion in _retenciones)
        {
            var tipoImpuesto = retencion.Impuesto;
            var importe = decimal.Parse(retencion.Importe);
            switch (tipoImpuesto)
            {
                case "001":
                    _totalesDecimal.TotalRetencionesIsr += importe;
                    break;
                case "002":
                    _totalesDecimal.TotalRetencionesIva += importe;
                    break;
                case "003":
                    _totalesDecimal.TotalRetencionesIeps += importe;
                    break;
            }
        }
    }

    private void CalculateTotalesTraslados()
    {
        foreach (var traslado in _traslados)
        {
            var tasaOCuota = decimal.Parse(traslado.TasaOCuota ?? "0");
            var tipoFactor = traslado.TipoFactor;
            var impuesto = traslado.Impuesto;
            if(impuesto != "002") continue;
            var @base = decimal.Parse(traslado.Base);
            var importe = decimal.Parse(traslado.Importe ?? "0");
            if (tipoFactor is "Exento")
            {
                _totalesDecimal.TotalTrasladosBaseIvaExento += @base;
                continue;
            }

            switch (tasaOCuota)
            {
                case decimal.Zero:
                    _totalesDecimal.TotalTrasladosBaseIva0 += @base;
                    _totalesDecimal.TotalTrasladosImpuestoIva0 += importe;
                    break;
                case 0.08m:
                    _totalesDecimal.TotalTrasladosBaseIva8 += @base;
                    _totalesDecimal.TotalTrasladosImpuestoIva8 += importe;
                    break;
                case 0.16m:
                    _totalesDecimal.TotalTrasladosBaseIva16 += @base;
                    _totalesDecimal.TotalTrasladosImpuestoIva16 += importe;
                    break;
            }
        }
    }
    private void RondTotals()
    {
        _totales.TotalRetencionesIva = FormatTotal(_totalesDecimal.TotalRetencionesIva);
        _totales.TotalRetencionesIsr = FormatTotal(_totalesDecimal.TotalRetencionesIsr);
        _totales.TotalRetencionesIeps = FormatTotal(_totalesDecimal.TotalRetencionesIeps);
        _totales.TotalTrasladosImpuestoIva16 = FormatTotal(_totalesDecimal.TotalTrasladosImpuestoIva16);
        _totales.TotalTrasladosBaseIva16 = FormatTotal(_totalesDecimal.TotalTrasladosBaseIva16);
        _totales.TotalTrasladosImpuestoIva8 = FormatTotal(_totalesDecimal.TotalTrasladosImpuestoIva8);
        _totales.TotalTrasladosBaseIva8 = FormatTotal(_totalesDecimal.TotalTrasladosBaseIva8);
        _totales.TotalTrasladosBaseIva0 =  FormatTotal(_totalesDecimal.TotalTrasladosBaseIva0);
        _totales.TotalTrasladosImpuestoIva0 = FormatTotal(_totalesDecimal.TotalTrasladosImpuestoIva0);
        _totales.TotalTrasladosBaseIvaExento = FormatTotal(_totalesDecimal.TotalTrasladosBaseIvaExento);
    }

    private static string? FormatTotal(decimal total)
    {
        if(total is decimal.Zero) return null;
        var result = Math.Round(total, 2);
        return result.ToString(CultureInfo.InvariantCulture);
    }
}