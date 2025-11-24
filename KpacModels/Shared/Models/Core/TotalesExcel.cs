using System.Globalization;
using KPac.Application.Validator;

namespace KpacModels.Shared.Models.Core;

public class TotalesExcel
{
    
    public bool SoloExentosPagosRetenciones = true;
    
    public bool SoloExentosPagosTraslados = true;
    public decimal _tipoCambio { get; set; }
    public decimal TotalRetencionesIva { get; set; } = decimal.Zero;
    public decimal TotalRetencionesIsr { get; set; } = decimal.Zero;
    public decimal TotalRetencionesIeps { get; set; } = decimal.Zero;
    public decimal TotalTrasladosBaseIva16 { get; set; } = decimal.Zero;
    public decimal TotalTrasladosImpuestoIva16 { get; set; } = decimal.Zero;
    public decimal TotalTrasladosBaseIva8 { get; set; } = decimal.Zero;
    public decimal TotalTrasladosImpuestoIva8 { get; set; } = decimal.Zero;
    public decimal TotalTrasladosBaseIva0 { get; set; } = decimal.Zero;
    public decimal TotalTrasladosImpuestoIva0 { get; set; } = decimal.Zero;
    
    public decimal TotalTrasladosBaseIvaExento { get; set; } = decimal.Zero;

    public Dictionary<string, object?> ToDictionary()
    {
        return new Dictionary<string, object?>()
        {
            { "TotalRetencionesIva", GetTotal(TotalRetencionesIva) },
            { "TotalRetencionesIsr", GetTotal(TotalRetencionesIsr) },
            { "TotalRetencionesIeps", GetTotal(TotalRetencionesIeps) },
            { "TotalTrasladosBaseIva16", GetTotal(TotalTrasladosBaseIva16) },
            { "TotalTrasladosImpuestoIva16", GetTotal(TotalTrasladosImpuestoIva16) },
            { "TotalTrasladosBaseIva8", GetTotal(TotalTrasladosBaseIva8) },
            { "TotalTrasladosImpuestoIva8", GetTotal(TotalTrasladosImpuestoIva8) },
            { "TotalTrasladosBaseIva0", GetTotal(TotalTrasladosBaseIva0) },
            { "TotalTrasladosImpuestoIva0", GetTotal(TotalTrasladosImpuestoIva0) },
        };
    }

    public Dictionary<string, string?> BuildTotales(decimal totalPagos)
    {
        Dictionary<string, string?> totales = new();
        if (!SoloExentosPagosRetenciones)
        {
            totales["TotalRetencionesIva"] = BuildTotal(TotalRetencionesIva);
            totales["TotalRetencionesIsr"] = BuildTotal(TotalRetencionesIsr);
            totales["TotalRetencionesIeps"] = BuildTotal(TotalRetencionesIeps);
        }
        if (!SoloExentosPagosTraslados)
        {
            totales["TotalTrasladosBaseIva16"] = BuildTotal(TotalTrasladosBaseIva16);
            totales["TotalTrasladosImpuestoIva16"] = BuildTotal(TotalTrasladosImpuestoIva16);
            totales["TotalTrasladosBaseIva8"] = BuildTotal(TotalTrasladosBaseIva8);
            totales["TotalTrasladosImpuestoIva8"] = BuildTotal(TotalTrasladosImpuestoIva8);
            totales["TotalTrasladosBaseIva0"] = BuildTotal(TotalTrasladosBaseIva0);
            totales["TotalTrasladosImpuestoIva0"] = BuildTotal(TotalTrasladosImpuestoIva0);
        }
        totales["MontoTotalPagos"] = Math.Round(totalPagos, 2).ToString(CultureInfo.InvariantCulture);
        totales["TotalTrasladosBaseIvaExento"] = BuildTotal(TotalTrasladosBaseIvaExento);
        return totales;
    }


    public object? GetTotal(decimal total)
    {
        if (total == decimal.Zero)
            return null;
        Console.WriteLine($"total: {total}");
        Console.WriteLine($"tipo cambio: {_tipoCambio}");
        var totalConvertido = total * _tipoCambio;
        Console.WriteLine($"Total convertido: {totalConvertido}");
        Console.WriteLine($"Total redondeado: {Math.Round(totalConvertido, 2)}");
        return Math.Round(totalConvertido, 2);
    }

    public string? BuildTotal(decimal total)
    {
        if(total == decimal.Zero)
            return null;
        return DecimalOperatorLimites.TruncarDecimal(total, 2).ToString(CultureInfo.InvariantCulture);
    }
};