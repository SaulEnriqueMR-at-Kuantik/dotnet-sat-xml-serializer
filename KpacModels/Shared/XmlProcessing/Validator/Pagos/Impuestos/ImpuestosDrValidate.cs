using System.Globalization;
using KPac.Application.Validator;
using KpacModels.Shared.Models.Comprobante.Complementos.Pagos;
using KpacModels.Shared.XmlProcessing.Validator.Catalogos;

namespace KpacModels.Shared.XmlProcessing.Validator.Pagos.Impuestos;

public class ImpuestosDrValidate
{
    public void ValidateRetencion(RetencionDR retencion, ValidatorContext context, string section)
    {
        var impuestosHelper = new ImpuestosHelper(context: context);
        var decimalesMoneda = int.Parse(context.GetValue("monedaDecimales") ?? "0");
        // Validar Base
        var baseString = retencion.Base;
        if (string.IsNullOrEmpty(baseString))
        {
            context.AddError(
                code: "CRP20999",
                section: section,
                message: "El campo BaseDR es obligatorio, no puede ser nulo ni vació.");
            return;
        }
        var @base = decimal.Parse(baseString);
        if (@base <= 0)
        {
            context.AddError(
                code: "CRP20249",
                section: section,
                message: "El valor del campo BaseDR que corresponde a Retención debe ser mayor que cero.");
            return;
        }

        // Validar Impuesto
        if (!CatalogosComprobante.c_Impuesto.TryGetValue(retencion.Impuesto, out var impuesto))
        {
            context.AddError(
                code: "CRP20250",
                section: section,
                message:
                "El valor del campo ImpuestoDR que corresponde a Retención no contiene un valor del catálogo c_Impuesto.");
            return;
        }
        var tipoFactor = retencion.TipoFactor;
        // Validar TipoFactor
        if (!CatalogosComprobante.c_TipoFactor.Contains(tipoFactor))
        {
            context.AddError(
                code: "CRP20251",
                section: section,
                message: "El valor del campo TipoFactorDR que corresponde a Retención no contiene un valor del catálogo" +
                         " c_TipoFactor.");
            return;
        }

        if (tipoFactor == "Exento")
        {
            context.AddError(
                code: "CRP20252",
                section: section,
                message: "El valor registrado en el campo TipoFactorDR que corresponde a Retención debe ser distinto de Exento.");
            return;
        }
        // Validar TasaOCuota
        var tasaOCuotaList = ValidateHelper.GetListTasaOCuota(impuesto, retencion: true);
        var tasaOCuota = decimal.Parse(retencion.TasaOCuota, CultureInfo.InvariantCulture);
        if (tasaOCuotaList.Count > 0 && !ValidateHelper.ExistTasaOCuota(
                tasaOCuotaList, 
                tasaOCuota,
                retencion.TipoFactor))
        {
            context.AddError(
                code: "CRP20253",
                section: section,
                message: "El valor del campo TasaOCuotaDR que corresponde a Retención, no contiene un valor del catálogo" +
                         " c_TasaOcuota o se encuentra fuera de rango.");
            return;
        }
        var importe = decimal.Parse(retencion.Importe);
        if(tipoFactor != "Exento"){
            var importeInferior = DecimalOperatorLimites.CalcularLimiteInferiorImporteDr(@base, tasaOCuota, importe);
            var importeSuperior = DecimalOperatorLimites.CalcularLimiteSuperiorImporteDr(@base, tasaOCuota, importe);
            if (importe < importeInferior || importe > importeSuperior)
            {
                context.AddError(
                    code: "CFDI40186", 
                    section: section, 
                    message: "El valor del campo Importe que corresponde a Retención no se encuentra entre el" +
                             $" limite inferior y superior permitido. Limite inferior: {importeInferior}. Limite" +
                             $" superior: {importeSuperior}. Valor registrado: {importe}");
            }
        }
        impuestosHelper.AddRetencion(impuesto: retencion.Impuesto, importe: importe);
    }

    public void ValidateTraslado(TrasladoDR traslado, ValidatorContext context, string section)
    {
        var impuestosHelper = new ImpuestosHelper(context: context);
        var decimalesMonedaDr = int.Parse(context.GetValue("decimalesMonedaDr") ?? "0");
        // Validar Base sea mayor a 0
        var @base = decimal.Parse(traslado.Base);
        if (@base <= 0)
        {
            context.AddError(
                code: "CRP20255",
                section: section,
                message: $"El valor del campo BaseDR que corresponde a Traslado debe ser mayor que cero. Valor registrado: {@base}.");
            return;
        }

        // Validar Impuesto
        if (!CatalogosComprobante.c_Impuesto.TryGetValue(traslado.Impuesto, out var impuesto))
        {
            context.AddError(
                code: "CRP20256",
                section: section,
                message:
                "El valor del campo ImpuestoDR que corresponde a Traslado no contiene un valor del catálogo c_Impuesto.");
            return;
        }
        
        // Validar que Impuesto sea diferente a "ISR"
        if (impuesto == "ISR")
        {
            context.AddError(
                code: "CRP20999",
                section: section,
                message: "El Impuesto '001' correspondiente a 'ISR' no esta permitido en el nodo Traslado");
            return;
        }
        
        var tipoFactor = traslado.TipoFactor;
        // Validar TipoFactor
        if (!CatalogosComprobante.c_TipoFactor.Contains(tipoFactor))
        {
            context.AddError(
                code: "CRP20257",
                section: section,
                message: "El valor del campo TipoFactorDR que corresponde a Traslado no contiene un valor del catálogo" +
                         " c_TipoFactor.");
            return;
        }

        if (tipoFactor == "Exento" && !string.IsNullOrEmpty(traslado.Impuesto) && !string.IsNullOrEmpty(traslado.TasaOCuota))
        {
            context.AddError(
                code: "CRP20258",
                section: section,
                message: "El valor registrado en el campo TipoFactorDR que corresponde a Traslado es Exento, no se deben" +
                         " registrar los campos TasaOCuotaDR ni ImporteDR.");
            return;
        }
        
        if (tipoFactor is "Tasa" or "Cuota" && string.IsNullOrEmpty(traslado.Impuesto) && string.IsNullOrEmpty(traslado.TasaOCuota))
        {
            context.AddError(
                code: "CRP20259",
                section: section,
                message: "El valor registrado en el campo TipoFactorDR que corresponde a Traslado es Tasa o Cuota," +
                         " se deben registrar los campos TasaOCuotaDR e ImporteDR.");
            return;
        }
        // Validar TasaOCuota
        var tasaOCuotaList = ValidateHelper.GetListTasaOCuota(impuesto, traslado: true);
        var tasaOCuota = decimal.Parse(traslado.TasaOCuota ?? "0", CultureInfo.InvariantCulture);
        if (tasaOCuotaList.Count > 0 && traslado is { TasaOCuota: not null } &&
            !ValidateHelper.ExistTasaOCuota(
                tasaOCuotaList, 
                tasaOCuota,
                traslado.TipoFactor))
        {
            context.AddError(
                code: "CRP20260",
                section: section,
                message: "El valor del campo TasaOCuotaDR que corresponde a Traslado, no contiene un valor del catálogo" +
                         " c_TasaOcuota o se encuentra fuera de rango.");
            return;
        }
        var importe = decimal.Parse(traslado.Importe ?? "0");
        if(tipoFactor != "Exento"){
            var importeInferior = DecimalOperatorLimites.CalcularLimiteInferiorImporteDr(@base, tasaOCuota, importe);
            var importeSuperior = DecimalOperatorLimites.CalcularLimiteSuperiorImporteDr(@base, tasaOCuota, importe);
            if (importe < importeInferior || importe > importeSuperior)
            {
                context.AddError(
                    code: "CRP20261", 
                    section: section, 
                    message: "El valor del campo ImporteDR que corresponde a Traslado no se encuentra entre el" +
                             $" limite inferior y superior permitido. Limite inferior: {importeInferior}. Limite" +
                             $" superior: {importeSuperior}. Valor registrado: {importe}");
                return;
            }
            context.AddValue("soloExentos", "false");
        }
        var baseConvertida = impuestosHelper.CalculateImporteToMonedaP(@base);
        var importeConvertido = impuestosHelper.CalculateImporteToMonedaP(importe);
        traslado.Base = baseConvertida.ToString(CultureInfo.InvariantCulture);
        traslado.Importe = importeConvertido.ToString(CultureInfo.InvariantCulture);
        impuestosHelper.AddTrasladoDr(traslado);
    }
}