using KPac.Application.Validator;
using KPac.Domain.CatalgosSAT;
using KpacModels.Shared.Models.Comprobante;
using KpacModels.Shared.XmlProcessing.Validator.Catalogos;

namespace KpacModels.Shared.XmlProcessing.Validator.Comprobante.ConceptoValidate;

public class ImpuestoConcepto
{
    private ValidatorContext _context;
    private ImpuestosHelper _impuestosHelper;
    
    private int _numConcepto;
    private int _decimales = 0;

    //private RepositoryValidator<cfdi40_tasaocuota> _repository;
    public ImpuestoConcepto(ValidatorContext context,  int numConcepto
        //, ClientValidator clientValidator
        )
    {
        _context = context;
        _numConcepto = numConcepto;
        //_repository = new RepositoryValidator<cfdi40_tasaocuota>(clientValidator);
        _decimales = int.Parse(_context.GetValue("monedaDecimales") ?? "2");
        _impuestosHelper = new ImpuestosHelper(_context);
    }
    public async Task Validate(ConceptoImpuestos impuestos)
    {
        ValidateBase(impuestos);
        if (impuestos.Retenciones != null)
        {
            var count = impuestos.Retenciones.Count;
            for (var i = 0; i < count; i++)
            {
                var retencion = impuestos.Retenciones[i];
                await ValidateRetencion(retencion, i + 1);
            }
        }
        
        
        if (impuestos.Traslados != null)
        {
            var count = impuestos.Traslados.Count;
            for (var i = 0; i < count; i++)
            {
                var traslado = impuestos.Traslados[i];
                await ValidateTraslado(traslado, i + 1);
            }
        }
    }

    private void ValidateBase(ConceptoImpuestos impuestos)
    {
        if (impuestos is { Retenciones: { Count: 0 }, Traslados.Count: 0 })
        {
            _context.AddError(
                code: "CFDI40173",
                section: $"Comprobante -> {_numConcepto}.-Concepto -> Impuestos",
                message: "En caso de utilizar el nodo Impuestos en un concepto, se deben incluir impuestos  de " +
                         "traslado y/o retenciones.");
        }
    }

    private async Task ValidateRetencion(ImpuestoT retencion, int index)
    {
        var section = $"Comprobante -> {_numConcepto}.-Concepto -> Impuestos -> {index}.-Retención";
        // Validar Base
        var @base = decimal.Parse(retencion.Base ?? "0");
        if (@base <= 0)
        {
            _context.AddError(
                code: "CFDI40181",
                section: section,
                message: "El valor del campo Base que corresponde a Retención debe ser mayor que cero.");
            return;
        }

        // Validar Impuesto
        if (!CatalogosComprobante.c_Impuesto.TryGetValue(retencion.Impuesto, out var impuesto))
        {
            _context.AddError(
                code: "CFDI40182",
                section: section,
                message:
                "El valor del campo Impuesto que corresponde a Retención no contiene un valor del catálogo c_Impuesto.");
            return;
        }
        var tipoFactor = retencion.TipoFactor ?? string.Empty;
        // Validar TipoFactor
        if (!CatalogosComprobante.c_TipoFactor.Contains(tipoFactor))
        {
            _context.AddError(
                code: "CFDI40183",
                section: section,
                message: "El valor del campo TipoFactor que corresponde a Retención no contiene un valor del catálogo" +
                         " c_TipoFactor.");
            return;
        }

        if (tipoFactor == "Exento")
        {
            _context.AddError(
                code: "CFDI40184",
                section: section,
                message: "El valor registrado en el campo TipoFactor que corresponde a Retención debe ser distinto de Exento.");
            return;
        }
        // Validar TasaOCuota
        var tasaOCuotaList = ValidateHelper.GetListTasaOCuota(impuesto, retencion: true);
        var tasaOCuota = decimal.Parse(retencion.TasaOCuota ?? "0");
        if (tasaOCuotaList.Count > 0 && retencion is { TasaOCuota: not null, TipoFactor: not null } &&
            !ValidateHelper.ExistTasaOCuota(
                tasaOCuotaList, 
                tasaOCuota,
                retencion.TipoFactor))
        {
            _context.AddError(
                code: "CFDI40185",
                section: section,
                message: "El valor del campo TasaOCuota que corresponde a Retención, no contiene un valor del catálogo" +
                         " c_TasaOcuota o se encuentra fuera de rango.");
            return;
        }
        var importe = decimal.Parse(retencion.Importe ?? "0");
        var decimalesImporte = ValidateHelper.CountDecimalPlaces(importe);
        if(tipoFactor != "Exento"){
            var importeInferior = DecimalOperatorLimites.CalcularLimiteInferiorImpuesto(@base, tasaOCuota, decimalesImporte);
            var importeSuperior = DecimalOperatorLimites.CalcularLimiteSuperiorImpuesto(@base, tasaOCuota, decimalesImporte);
            if (importe < importeInferior || importe > importeSuperior)
            {
                _context.AddError(
                    code: "CFDI40186", 
                    section: section, 
                    message: "El valor del campo Importe que corresponde a Retención no se encuentra entre el" +
                             $" limite inferior y superior permitido. Limite inferior: {importeInferior}. Limite" +
                             $" superior: {importeSuperior}. Valor registrado: {importe}");
            }
            _context.AddValue("totalTrasladados", "true");
        }
        _impuestosHelper.AddRetencion(impuesto: retencion.Impuesto, importe: importe);
        
    }
    
    
    private async Task ValidateTraslado(ImpuestoT traslado, int index )
    {
        var section = $"Comprobante -> {_numConcepto}.-Concepto -> Impuestos -> {index}.-Traslado";
        // Validar Base sea mayor a 0
        var @base = decimal.Parse(traslado.Base ?? "0");
        if (@base <= 0)
        {
            _context.AddError(
                code: "CFDI40174",
                section: section,
                message: $"El valor del campo Base que corresponde a Traslado debe ser mayor que cero. Valor registrado: {@base}.");
            return;
        }

        // Validar Impuesto
        if (!CatalogosComprobante.c_Impuesto.TryGetValue(traslado.Impuesto, out var impuesto))
        {
            _context.AddError(
                code: "CFDI40175",
                section: section,
                message:
                "El valor del campo Impuesto que corresponde a Traslado no contiene un valor del catálogo c_Impuesto.");
            return;
        }
        
        // Validar que Impuesto sea diferente a "ISR"
        if (impuesto == "ISR")
        {
            _context.AddError(
                code: "CFDI40999",
                section: section,
                message: "El Impuesto '001' correspondiente a 'ISR' no esta permitido en el nodo Traslado");
            return;
        }
        
        var tipoFactor = traslado.TipoFactor ?? string.Empty;
        // Validar TipoFactor
        if (!CatalogosComprobante.c_TipoFactor.Contains(tipoFactor))
        {
            _context.AddError(
                code: "CFDI40176",
                section: section,
                message: "El valor del campo TipoFactor que corresponde a Traslado no contiene un valor del catálogo" +
                         " c_TipoFactor.");
            return;
        }

        if (tipoFactor == "Exento" && !string.IsNullOrEmpty(traslado.Impuesto) && !string.IsNullOrEmpty(traslado.TasaOCuota))
        {
            _context.AddError(
                code: "CFDI40177",
                section: section,
                message: "El valor registrado en el campo TipoFactor que corresponde a Traslado es Exento, no se deben" +
                         " registrar los campos TasaOCuota ni Importe.");
            return;
        }
        
        if (tipoFactor is "Tasa" or "Cuota" && string.IsNullOrEmpty(traslado.Impuesto) && string.IsNullOrEmpty(traslado.TasaOCuota))
        {
            _context.AddError(
                code: "CFDI40178",
                section: section,
                message: "El valor registrado en el campo TipoFactor que corresponde a Traslado es Tasa o Cuota," +
                         " se deben registrar los campos TasaOCuota e Importe.");
            return;
        }
        // Validar TasaOCuota
        var tasaOCuotaList = ValidateHelper.GetListTasaOCuota(impuesto, traslado: true);
        var tasaOCuota = decimal.Parse(traslado.TasaOCuota ?? "0");
        if (tasaOCuotaList.Count > 0 && traslado is { TasaOCuota: not null, TipoFactor: not null } &&
            !ValidateHelper.ExistTasaOCuota(
                tasaOCuotaList, 
                tasaOCuota,
                traslado.TipoFactor))
        {
            _context.AddError(
                code: "CFDI40179",
                section: section,
                message: "El valor del campo TasaOCuota que corresponde a Traslado, no contiene un valor del catálogo" +
                         " c_TasaOcuota o se encuentra fuera de rango.");
            _context.GetErrors();
        }
        var importe = decimal.Parse(traslado.Importe ?? "0");
        var decimalesImporte = ValidateHelper.CountDecimalPlaces(importe);
        if(tipoFactor != "Exento"){
            var importeInferior = DecimalOperatorLimites.CalcularLimiteInferiorImpuesto(@base, tasaOCuota, decimalesImporte);
            var importeSuperior = DecimalOperatorLimites.CalcularLimiteSuperiorImpuesto(@base, tasaOCuota, decimalesImporte);
            if (importe < importeInferior || importe > importeSuperior)
            {
                _context.AddError(
                    code: "CFDI40186", 
                    section: section, 
                    message: "El valor del campo Importe que corresponde a Traslado no se encuentra entre el" +
                             $" limite inferior y superior permitido. Limite inferior: {importeInferior}. Limite" +
                             $" superior: {importeSuperior}. Valor registrado: {importe}");
                return;
            }
            _context.AddValue("soloExentos", "false");
        }
        _impuestosHelper.AddTraslado(traslado);
    }
}