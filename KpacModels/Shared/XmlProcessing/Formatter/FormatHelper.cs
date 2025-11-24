using System.Globalization;
using KpacModels.Shared.Models.Comprobante;
using KpacModels.Shared.Models.Comprobante.Complementos.Pagos;

namespace KpacModels.Shared.XmlProcessing.Formatter;

public class FormatHelper
{
    public static string ConvertStringToDecimalSatString(string valueString, string errorMessage)
    {
        if(!decimal.TryParse(valueString, out var value))
            throw new FormatException(errorMessage);
        return Math.Round(value, 6).ToString("F6");
    }
    
    public static bool TryConvertStringToDecimalSat(string? valueString, out decimal result)
    {
        if (!decimal.TryParse(valueString, out var value))
        {
            result = decimal.Zero;
            return false;
        }
        result = Math.Round(value, 6);
        return true;
    }
    
    
    public static List<ImpuestoT> ImpuestosSummaryTraslados(List<ImpuestoT> items)
    {
        return items
            .GroupBy(i => new { i.Impuesto, i.TasaOCuota, i.TipoFactor })
            .Select(g => new ImpuestoT
            {
                Impuesto = g.Key.Impuesto,
                TasaOCuota = g.Key.TasaOCuota,
                TipoFactor = g.Key.TipoFactor,
                Base = g.Sum(x => decimal.Parse(x.Base ?? "0")).ToString("F2"),
                Importe = g.All(x => x.Importe == null) 
                    ? null 
                    : g.Sum(x => decimal.Parse(x.Importe)).ToString("F2")
            })
            .ToList();
    }
    
    public static List<TrasladoP> ImpuestosSummaryTraslados(List<TrasladoP>? items)
    {
        if(items == null || items.Count == 0) 
            return [];
        return items
            .GroupBy(i => new { i.Impuesto, i.TasaOCuota, i.TipoFactor })
            .Select(g => new TrasladoP
            {
                Impuesto = g.Key.Impuesto,
                TasaOCuota = g.Key.TasaOCuota,
                TipoFactor = g.Key.TipoFactor,
                Base = g.Sum(x => decimal.Parse(x.Base)).ToString(CultureInfo.InvariantCulture),
                Importe = g.All(x => x.Importe == null) 
                    ? null 
                    : g.Sum(x => decimal.Parse(x.Importe ?? "0")).ToString(CultureInfo.InvariantCulture)
            })
            .ToList();
    }
    
    public static List<ImpuestoR> ImpuestosSummaryRetenciones(List<ImpuestoT> items)
    {
        return items
            .GroupBy(i => i.Impuesto)
            .Select(g => new ImpuestoR
            {
                Impuesto = g.Key,
                Importe = g.Sum(x => decimal.Parse(x.Importe ?? "0")).ToString("F2")
            })
            .ToList();
    }
    
    public static List<RetencionP> ImpuestosSummaryRetenciones(List<RetencionP>? items)
    {
        if(items == null || items.Count == 0) return [];
        return items
            .GroupBy(i => i.Impuesto)
            .Select(g => new RetencionP
            {
                Impuesto = g.Key,
                Importe = g.Sum(x => decimal.Parse(x.Importe)).ToString(CultureInfo.InvariantCulture)
            })
            .ToList();
    }
    
    public static string? FormatStringToImporteSat(string? valueString)
    {
        if (!decimal.TryParse(valueString, out var value))
        {
            return null;
        }
        return Math.Round(value, 2).ToString("F2");
    }
    
    public static string? FormatDecimalToImporteSat(decimal? value)
    {
        if (value == null)
            return null;
        return Math.Round(value ?? 0, 2).ToString("F2");
    }

    
}