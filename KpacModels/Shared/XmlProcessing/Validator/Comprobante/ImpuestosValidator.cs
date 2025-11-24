using KPac.Application.Validator;
using KpacModels.Shared.Models.Comprobante;
using KpacModels.Shared.XmlProcessing.Validator.Comprobante.ImpuestosValidate;

namespace KpacModels.Shared.XmlProcessing.Validator.Comprobante;

public class ImpuestosValidator
{
    private string _tipoComprobante;
    private int _monedaDecimales;
    private ValidatorContext _context;
    private TrasladosValidator _trasladosValidator;
    private RetencionesValidator _retencionesValidator;
    
    public void Validate(Impuestos impuestos, ValidatorContext comprobanteContext)
    {
        _context = comprobanteContext;
        _tipoComprobante = _context.GetValue("tipoComprobante") ?? string.Empty;
        var monedaDecimalesString = _context.GetValue("monedaDecimales");
        _monedaDecimales = int.Parse(monedaDecimalesString ?? "0");
        _trasladosValidator = new TrasladosValidator(comprobanteContext);
        _retencionesValidator = new RetencionesValidator(comprobanteContext);
        if (!ValidateBase(impuestos)) return;
        _retencionesValidator.ValidateRetenciones(impuestos.Retenciones, impuestos.TotalImpuestosRetenidos);
        _trasladosValidator.ValidateTraslados(impuestos.Traslados, impuestos.TotalImpuestosTrasladados);
    }

    private bool ValidateBase(Impuestos impuestos)
    {
        if (_tipoComprobante is "T" or "P")
        {
            _context.AddError(
                code: "CFDI40201",
                section: "Comprobante -> Impuestos",
                message: "Cuando el TipoDeComprobante sea T o P, el elemento Impuestos no debe existir.");
            return false;
        }

        if (impuestos.TotalImpuestosRetenidos != null)
        {
            var totalRetenidos = decimal.Parse(impuestos.TotalImpuestosRetenidos);
            var numDecimalestotalRetenidos = ValidateHelper.CountDecimalPlaces(totalRetenidos);
            if (numDecimalestotalRetenidos > _monedaDecimales)
            {
                _context.AddError(
                    code: "CFDI40202",
                    section: "Comprobante -> Impuestos",
                    message: $"El valor del campo TotalImpuestosRetenidos debe tener hasta la cantidad de decimales" +
                             $" que soporte la moneda. Valor {totalRetenidos} con {numDecimalestotalRetenidos} decimales. " +
                             $"Número de decimales permitidos en la moneda registrada: {_monedaDecimales}");
                return false;
            }
        }

        if (impuestos.TotalImpuestosTrasladados != null)
        {
            var totalTraslados = decimal.Parse(impuestos.TotalImpuestosTrasladados);
            var numDecimalesTotalTraslados = ValidateHelper.CountDecimalPlaces(totalTraslados);
            if (numDecimalesTotalTraslados > _monedaDecimales)
            {
                _context.AddError(
                    code: "CFDI40202",
                    section: "Comprobante -> Impuestos",
                    message: $"El valor del campo TotalImpuestosRetenidos debe tener hasta la cantidad de decimales" +
                             $" que soporte la moneda. Valor {totalTraslados} con {numDecimalesTotalTraslados} decimales. " +
                             $"Número de decimales permitidos en la moneda registrada: {_monedaDecimales}");
                return false;
            }
        }

        return true;
        
    }
}