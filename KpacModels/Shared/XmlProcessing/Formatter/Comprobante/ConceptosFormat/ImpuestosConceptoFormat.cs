using KPac.Application.Formatter;
using KPac.Application.Validator;
using KPac.Application.Validator.Catalogos;
using KpacModels.Shared.Models.Comprobante;
using KpacModels.Shared.XmlProcessing.Validator;
using KpacModels.Shared.XmlProcessing.Validator.Catalogos;

namespace KpacModels.Shared.XmlProcessing.Formatter.Comprobante.ConceptosFormat;

public class ImpuestosConceptoFormat
{
    private FormatContext _context;
    public ImpuestosConceptoFormat(FormatContext context)
    {
        _context = context;
    }
    public void FormatImpuesto(ImpuestoT impuesto, string section)
    {
        var tasaOCuota = Math.Round(impuesto.SrcTasaOCuota ?? decimal.Zero, 6);
        impuesto.TasaOCuota = tasaOCuota.ToString("F6");
        
        // Si se registra TipoFactor con el valor "Exento", TasaOCuota e Importe no deben existir
        if (impuesto.TipoFactor == "Exento")
        {
            impuesto.TasaOCuota = null;
            impuesto.Importe = null;
            return;
        }

        if (impuesto.TipoFactor is "Tasa" or "Cuota")
        {
            if (string.IsNullOrEmpty(impuesto.TasaOCuota))
            {
                _context.AddError(
                    section: section,
                    message: "El atributo TasaOCuota es obligatorio cuando TipoFactor es Tasa o Cuota.");
                return;
            }
            if (string.IsNullOrEmpty(impuesto.TasaOCuota))
            {
                _context.AddError(
                    section: section,
                    message: "El atributo Base es obligatorio cuando TipoFactor es Tasa o Cuota.");
                return;
            }
            
        }

        var @base = impuesto.SrcBase ?? decimal.Zero;
        if (@base < decimal.Zero)
        {
            _context.AddError(
                section: section,
                message: "El atributo Base registrado tiene un valor que no es permitido, debe ser un número mayor a 0 (cero).");
            return;
        }
        
        
        if (ValidateHelper.CountDecimalPlaces(@base) > 6)
        {
            DecimalOperatorLimites.TruncarDecimal(@base, 6);
            return;
        }
        impuesto.Base = @base.ToString("F6");
        
        var importe = @base * tasaOCuota;
        impuesto.Importe = importe.ToString("F6");
    }

    /// <summary>
    /// Validar la TasaOCuota de un Impuesto
    /// </summary>
    /// <param name="impuesto">Objeto Impuesto</param>
    /// <param name="section">Si hay error la seccion donde se mando a llamar la función</param>
    /// <param name="message">Si hay error el mensaje</param>
    public void ValidateTasaOCuota(ImpuestoT impuesto, string section, string message)
    {
        var listTasaOCuota = ValidateHelper.GetListTasaOCuota(CatalogosComprobante.c_Impuesto[impuesto.Impuesto]);
        if (listTasaOCuota.Count > 0 && impuesto is { TasaOCuota: not null, TipoFactor: not null })
        {
            var tasaOCuota = decimal.Parse(impuesto.TasaOCuota);
            if (!ValidateHelper.ExistTasaOCuota(listTasaOCuota, tasaOCuota, impuesto.TipoFactor))
            {
                _context.AddError(
                    section: section,
                    message: message );
            }
        }
    }
}