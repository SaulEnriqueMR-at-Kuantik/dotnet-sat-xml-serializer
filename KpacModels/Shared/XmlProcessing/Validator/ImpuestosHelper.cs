using System.Globalization;
using KpacModels.Shared.Models.Comprobante;
using KpacModels.Shared.Models.Comprobante.Complementos.Pagos;
using KpacModels.Shared.Models.Core;

namespace KpacModels.Shared.XmlProcessing.Validator;

/// <summary>
/// Esta clase se usa para agrupar los impuestos de concepto.
/// Calcular el importe y base total.
/// </summary>
public class ImpuestosHelper
{
    private ValidatorContext _context;
    
    public ImpuestosHelper(ValidatorContext context)
    {
        _context = context;
    }
    
    /// <summary>
    /// Realizar la suma de un importe según su tipo de impuesto.
    /// </summary>
    /// <param name="impuesto">Tipo de impuesto (001, 002, 003)</param>
    /// <param name="importe">Importe que se sumara</param>
    public void AddRetencion(string impuesto, decimal importe)
    {
        var key = $"{impuesto}_retencion";
        if (_context.TryGetRetencion(key, out var retencion))
        {
            var result = Suma(retencion, importe);
            _context.AddRetencion(key, result);
            return;
        }
        _context.AddRetencion(key, importe);
    }

    /// <summary>
    /// Obtener la suma de importes de impuestos concepto por tipo de impuesto.
    /// </summary>
    /// <param name="impuesto">Tipo de impuesto (001, 002, 003)</param>
    /// <returns>Suma de importes de impuesto conceptos</returns>
    public string GetRetencion(string impuesto)
    {
        var key = $"{impuesto}_retencion";
        if (_context.TryGetRetencion(key, out var retencion))
        {
            _context.DeleteRetencion(key);
            return retencion.ToString("F2");
        }

        return "0";
    }

    /// <summary>
    /// Calcular el total de importe y base de traslados, agrupándolos por Impuesto, TasaOCuota y TipoFactor.
    /// </summary>
    /// <param name="traslado">Impuesto con los atributos que requerimos</param>
    public void AddTraslado(ImpuestoT traslado)
    {
        var key = $"{traslado.Impuesto}_{traslado.TasaOCuota}_{traslado.TipoFactor}";
        if (_context.TryGetTraslado(key, out var data))
        {
            var baseString = traslado.Base;
            if (!string.IsNullOrEmpty(baseString))
            {
                var @base = decimal.Parse(baseString);
                data.BaseTotal = Suma(data.BaseTotal, @base);
            }
            var importeString = traslado.Importe;
            if (!string.IsNullOrEmpty(importeString))
            {
                var importe = decimal.Parse(importeString);
                data.ImporteTotal = Suma(data.ImporteTotal, importe);
            }
            _context.AddTraslado(key, data);
            return;
        }
        _context.AddTraslado(key, new TrasladoTotales(traslado));
    }
    
    /// <summary>
    /// Calcular el total de importe y base de traslados, agrupándolos por Impuesto, TasaOCuota y TipoFactor.
    /// </summary>
    /// <param name="traslado">Impuesto con los atributos que requerimos</param>
    public void AddTrasladoDr(TrasladoDR traslado)
    {
        var key = $"{traslado.Impuesto}_{traslado.TasaOCuota}_{traslado.TipoFactor}";
        if (_context.TryGetTraslado(key, out var data))
        {
            var baseString = traslado.Base;
            if (!string.IsNullOrEmpty(baseString))
            {
                var @base = decimal.Parse(baseString);
                data.BaseTotal = Suma(data.BaseTotal, @base);
            }
            var importeString = traslado.Importe;
            if (!string.IsNullOrEmpty(importeString))
            {
                var importe = decimal.Parse(importeString);
                data.ImporteTotal = Suma(data.ImporteTotal, importe);
            }
            _context.AddTraslado(key, data);
            return;
        }
        _context.AddTraslado(key, new TrasladoTotales(traslado));
    }
    
    
    
    /// <summary>
    /// Obtener la suma de importes de impuestos concepto por Impuesto.
    /// </summary>
    /// <param name="traslado"></param>
    /// <returns>Objeto <see cref="ImpuestoT"/> con los totales</returns>
    public TrasladoTotales? GetTraslado(ImpuestoT traslado)
    {
        var key = $"{traslado.Impuesto}_{traslado.TasaOCuota}_{traslado.TipoFactor}";
        if (_context.TryGetTraslado(key, out var value))
        {
            _context.DeleteTraslado(key);
            return value;
        }
        return null;
    }
    
    /// <summary>
    /// Obtener la suma de importes de impuestos concepto por Impuesto.
    /// </summary>
    /// <param name="traslado"></param>
    /// <returns>Objeto <see cref="ImpuestoT"/> con los totales</returns>
    public TrasladoTotales? GetTraslado(TrasladoP traslado)
    {
        var key = $"{traslado.Impuesto}_{traslado.TasaOCuota}_{traslado.TipoFactor}";
        if (_context.TryGetTraslado(key, out var value))
        {
            _context.DeleteTraslado(key);
            return value;
        }
        return null;
    }
    
    
    private static decimal Suma(decimal firstValue, decimal secondValue)
    {
        decimal result = firstValue + secondValue;
        return result;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="importeSinConvertir"></param>
    public decimal CalculateImporteToMonedaP(decimal importeSinConvertir)
    {
        var equivalenciaDr = decimal.Parse(_context.GetValue("equivalenciaDr") ?? "1");
        var tipoCambioP = 1;
        var tipoCambioDr = Math.Round(tipoCambioP / equivalenciaDr, 10);
        var importe = importeSinConvertir * tipoCambioDr;
        return importe;
    }

    public static List<RetencionP> CalculateRetencionesP(RetencionP[] retenciones, decimal tipoCambio)
    {
        foreach (var retencion in retenciones)
        {
            retencion.Importe = (decimal.Parse(retencion.Importe) * tipoCambio).ToString(CultureInfo.InvariantCulture);
        }
        return retenciones.ToList();
    }
    
    public static List<TrasladoP> CalculateTrasladosP(TrasladoP[] traslados, decimal tipoCambio)
    {
        foreach (var traslado in traslados)
        {
            traslado.Base = (decimal.Parse(traslado.Base) * tipoCambio).ToString(CultureInfo.InvariantCulture);
            traslado.Importe = (decimal.Parse(traslado.Importe ?? "0") * tipoCambio).ToString(CultureInfo.InvariantCulture);
            
        }
        return traslados.ToList();
    }
    
}