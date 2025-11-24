using System.Globalization;
using KPac.Application.Formatter;
using KPac.Application.Validator;
using KPac.Application.Validator.Catalogos;
using KpacModels.Shared.Models.Comprobante.Complementos.Pagos;
using KpacModels.Shared.XmlProcessing.Validator;
using KpacModels.Shared.XmlProcessing.Validator.Catalogos;
using Newtonsoft.Json;

namespace KpacModels.Shared.XmlProcessing.Formatter.Pagos;

public class ImpuestosDrFormatter
{
    private FormatContext _context;
    
    private string _section;
    
    public ImpuestosDrFormatter(FormatContext context)
    {
        _context = context;
    }

    public void Format(RetencionDR retencion, int numPago, int numDocto, int numRetencion)
    {
        _section = $"Comprobante -> Complemento -> Pagos -> {numPago}. Pago -> {numDocto}. Documento Relacionado -> {numRetencion} Retención";
        
        var baseDr = decimal.Parse(retencion.Base);
        if (baseDr < 0)
        {
            _context.AddError(_section, "El valor del campo BaseDR que corresponde a Retención debe ser mayor que cero.");
        }
        
        if (!CatalogosComprobante.c_Impuesto.TryGetValue(retencion.Impuesto,  out var impuesto))
        {
            _context.AddError(
                _section, 
                "El atributo ImpuestoDR debe contener un valor del catálogo c_Impuesto.");
            return;
        }

        if (!CatalogosComprobante.c_TipoFactor.Contains(retencion.TipoFactor))
        {
            _context.AddError(
                _section, 
                "El atributo TipoFactorDR debe contener un valor del catálogo c_TipoFactor.");
            return;
        }

        if (retencion.TipoFactor == "Exento")
        {
            _context.AddError(_section, "El valor de TipoFactorDR que corresponde a retención debe ser distinto de Exento.");
            return;
        }
        
        if (!ValidarTasaOCuota(impuesto, retencion.TasaOCuota, retencion.TipoFactor))
        {
            _context.AddError(
                _section, 
                "El valor del campo TasaOCuota que corresponde a Retención, no contiene un valor del catálogo c_TasaOcuota o se encuentra fuera de rango.");
            return;
        }

        if (retencion.TipoFactor is "Tasa" or "Cuota")
        {
            var tasaOCuota = decimal.Parse(retencion.TasaOCuota);
            var importe = baseDr *  tasaOCuota;
            retencion.Importe = importe.ToString("F6");
        }

    }

    public void Format(TrasladoDR traslado, int numPago, int numDocto, int numTraslado)
    {
        _section = $"Comprobante -> Complemento -> Pagos -> {numPago}. Pago -> {numDocto}. Documento Relacionado -> {numTraslado} Traslado";
        
        var baseDr = decimal.Parse(traslado.Base);
        if (baseDr < 0)
        {
            _context.AddError(_section, "El valor del campo BaseDR que corresponde a Traslado debe ser mayor que cero.");
        }
        
        if (!CatalogosComprobante.c_Impuesto.TryGetValue(traslado.Impuesto,  out var impuesto))
        {
            _context.AddError(
                _section, 
                "El atributo ImpuestoDR debe contener un valor del catálogo c_Impuesto.");
            return;
        }
        
        if (!CatalogosComprobante.c_TipoFactor.Contains(traslado.TipoFactor))
        {
            _context.AddError(
                _section, 
                "El atributo TipoFactorDR debe contener un valor del catálogo c_TipoFactor.");
            return;
        }

        if (traslado.TipoFactor == "Exento")
        {
            traslado.TasaOCuota = null;
            traslado.Importe = null;
            return;
        }

        if (traslado.TasaOCuota == null)
        {
            _context.AddError(_section, "Cuando el TipoFactorDR sea diferente a Exento, es obligatorio registrar la TasaOCuotaDR.");
            return;
        }
        
        if (!ValidarTasaOCuota(impuesto, traslado.TasaOCuota, traslado.TipoFactor))
        {
            _context.AddError(
                _section, 
                "El valor del campo TasaOCuota que corresponde a Retención, no contiene un valor del catálogo c_TasaOcuota o se encuentra fuera de rango.");
            return;
        }
        
        var tasaOCuota = decimal.Parse(traslado.TasaOCuota);
        var importe = baseDr *  tasaOCuota;
        traslado.Importe = importe.ToString("F6");
    }

    private bool ValidarTasaOCuota(string impuesto, string tasaOCuotaString, string tipoFactor)
    {
        var tasaOCuotaList = ValidateHelper.GetListTasaOCuota(impuesto, retencion: true);
        var tasaOCuota = decimal.Parse(tasaOCuotaString, CultureInfo.InvariantCulture);
        return ValidateHelper.ExistTasaOCuota(tasaOCuotaList, tasaOCuota, tipoFactor);
    }
    
}