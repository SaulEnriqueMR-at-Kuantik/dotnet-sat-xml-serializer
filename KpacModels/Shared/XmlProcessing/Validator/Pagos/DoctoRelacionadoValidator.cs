using KPac.Application.Validator;
using KPac.Domain.CatalgosSAT;
using KpacModels.Shared.Models.Comprobante.Complementos.Pagos;
using KpacModels.Shared.XmlProcessing.Validator.Catalogos;

namespace KpacModels.Shared.XmlProcessing.Validator.Pagos;

public class DoctoRelacionadoValidator
{
    private DoctoRelacionado _doctoRelacionado = new DoctoRelacionado();
    
    private ValidatorContext _context = new ValidatorContext();
    
    private string _section = string.Empty;

    //private RepositoryValidator<cfdi40_moneda> _repository;
    
    private MontoHelper _montoHelper;

    private int _decimalesMonedaDr = 0;
    
    public async Task Validate(ValidatorContext context, DoctoRelacionado doctoRelacionado,
        //ClientValidator clientValidator,
        int numPago, int numDocto)
    {
        _doctoRelacionado = doctoRelacionado;
        _context = context;
        //_repository = new RepositoryValidator<cfdi40_moneda>(clientValidator);
        _section = $"Comprobante -> Complemento -> Pagos -> {numPago}. Pago -> {numDocto}. DoctoRelacionado";
        _montoHelper = new MontoHelper(_context);
        ValidateIdDocumento();
        ValidateSerie();
        await ValidateMoneda();
        ValidateEquivalencia();
        ValidateNumParcialidad();
        ValidateImporteSaldoAnterior();
        ValidateImportePagado();
        ValidateImporteSaldoInsoluto();
        ValidateObjetoImpuesto();
    }

    private void ValidateIdDocumento()
    {
        var idDocumento = _doctoRelacionado.IdDocumento;

        if (string.IsNullOrEmpty(idDocumento))
        {
            _context.AddError(
                code: "CRP20999",
                section: _section,
                message: "El campo IdDocumento no puede ser nulo ni vació.");
            return;
        }
        
        if (!RegexCatalog.IsIdDocumentoValid(idDocumento))
        {
            _context.AddError(
                code: "CRP20999",
                section: _section,
                message: "El campo IdDocumento tiene un formato invalido, puede ser un código con letras y números separados por guiones (como d85b1407-dd61-4a8c-b4b0-b1995dc5f6b0) o un número con el formato 123-45-678901234.");
        }
    }

    private void ValidateSerie()
    {
        var serie = _doctoRelacionado.Serie;
        if(string.IsNullOrEmpty(serie)) return;
        if (serie.Length is < 1 or > 25)
        {
            _context.AddWarning(section: _section, message: "El campo Serie debe tener desde 1 hasta 25 caracteres.");
        }
    }

    private async Task ValidateMoneda()
    {
        var monedaDr = _doctoRelacionado.Moneda;
        if (string.IsNullOrEmpty(monedaDr))
        {
            _context.AddError(
                code: "CRP20999",
                section: _section,
                message: "El campo MonedaDr no puede ser nulo ni vació.");
            return;
        }
        
        if (monedaDr == "XXX")
        {
            _context.AddError(
                code: "CRP20236",
                section: _section,
                message: "El valor del campo MonedaDR debe ser distinto de 'XXX'.");
        }
        
        // var monedaDb = await _repository.GetAsync("c_moneda", monedaDr);
        // if (monedaDb == null)
        // {
        //     _context.AddError(
        //         code: "CRP20999",
        //         section: _section,
        //         message: "El valor del campo MonedaDr no pertenece al catalogo c_Moneda.");
        //     return;
        // }
        //
        // _decimalesMonedaDr = int.Parse(monedaDb.decimales);
        // _context.AddValue("decimalesMonedaDr",  monedaDb.decimales);

    }

    private void ValidateEquivalencia()
    {
        var monedaP = _context.GetValue("monedaP") ?? string.Empty;
        var monedaDr = _doctoRelacionado.Moneda;
        var equivalencia = _doctoRelacionado.Equivalencia;
        
        var decimalesEquivalencia = ValidateHelper.CountDecimalPlaces(decimal.Parse(equivalencia ?? "0"));
        if (decimalesEquivalencia > 10)
        {
            _context.AddWarning(section: _section, message: "El campo EquivalenciaDR solo puede contener hasta 10 decimales.");
        }
        
        
        // Si MonedaP es diferente a MonedaDR se debera registrar EquivalenciaDR
        if (monedaP != monedaDr && string.IsNullOrEmpty(equivalencia))
        {
            _context.AddError(
                code: "CRP20237",
                section: _section,
                message: "Si el valor del atributo MonedaDR es diferente al valor registrado en el atributo MonedaP del nodo Pago, se debe registrar información en el atributo EquivalenciaDR.");
            return;
        }
        
        if (monedaP == monedaDr && equivalencia != "1")
        {
            _context.AddError(
                code: "CRP20238",
                section: _section,
                message: "Si el valor del atributo MonedaDR es igual al valor registrado en el atributo MonedaP del nodo Pago se debera registrar el valor '1' en el atributo EquivalenciaDR. ");
            return;
        }
        _context.AddValue("equivalenciaDr", equivalencia ?? "1");
        
        // TODO
        //  - **CRP20277**
        //  - Cuando existan operaciones con más de un Documento relacionado en donde al menos uno de ellos contenga la misma moneda que la del Pago, para la fórmula en el cálculo del margen de variación se deben considerar 10 decimales en la EquivalenciaDR cuando el valor sea 1.
        //  - El valor de EquivalenciaDR  para la fórmula del cálculo del margen de variación debe ser “1.0000000000”. 
        
    }

    private void ValidateNumParcialidad()
    {
        var noParcialidad = _doctoRelacionado.NoParcialidad;
        if (string.IsNullOrEmpty(noParcialidad))
        {
            _context.AddError(section: _section, code: "CRP20999", message: "El campo NoParcialidad no puede ser nulo ni vació.");
        }
    }

    private void ValidateImporteSaldoAnterior()
    {
        var importeSaldoAnteriorString = _doctoRelacionado.ImporteSaldoAnterior;
        if (string.IsNullOrEmpty(importeSaldoAnteriorString))
        {
            _context.AddError(
                code: "CRP20999",
                section: _section,
                message: "El valor del campo ImporteSaldoAnterior no puede ser nulo ni vació.");
            return;
        }
        var importeSaldoAnterior = decimal.Parse(importeSaldoAnteriorString);
        if (importeSaldoAnterior < 0)
        {
            _context.AddError(
                code: "CRP20239",
                section: _section,
                message: "El valor del campo ImporteSaldoAnterior debe mayor a cero.");
            return;
        }
        var countDecimals = ValidateHelper.CountDecimalPlaces(importeSaldoAnterior);
        if (_decimalesMonedaDr != 0 && countDecimals > _decimalesMonedaDr)
        {
            _context.AddError(
                code: "CRP20240",
                section: _section,
                message: $"El valor del campo ImporteSaldoAnterior debe corresponder a la moneda registrada en el campo" +
                         $" MonedaDR y ser redondeados hasta la cantidad de decimales que soporte. Valor de " +
                         $"ImporteSaldoAnterior: {importeSaldoAnterior} con {countDecimals} decimales. Decimales " +
                         $"soportados por la moneda: {_decimalesMonedaDr}.");
        }
    }

    private void ValidateImportePagado()
    {
        var importePagadoString = _doctoRelacionado.ImportePagado;
        if (string.IsNullOrEmpty(importePagadoString))
        {
            _context.AddError(
                code: "CRP20999",
                section: _section,
                message: "El valor del campo ImportePagado no puede ser nulo ni vació.");
            return;
        }
        var importePagado = decimal.Parse(importePagadoString);
        if (importePagado < 0)
        {
            _context.AddError(
                code: "CRP20241",
                section: _section,
                message: "El valor del campo ImportePagado debe mayor a cero.");
            return;
        }
        var countDecimals = ValidateHelper.CountDecimalPlaces(importePagado);
        if (_decimalesMonedaDr != 0 && countDecimals > _decimalesMonedaDr)
        {
            _context.AddError(
                code: "CRP20242",
                section: _section,
                message: $"El valor del campo ImportePagado debe corresponder a la moneda registrada en el campo" +
                         $" MonedaDR y ser redondeados hasta la cantidad de decimales que soporte. Valor de " +
                         $"ImportePagado: {importePagado} con {countDecimals} decimales. Decimales " +
                         $"soportados por la moneda: {_decimalesMonedaDr}.");
            return;
        }

        var monedaP = _context.GetValue("monedaP");
        var decimalesMonedaP = int.Parse(_context.GetValue("monedaDecimalesP") ?? "0");
        var equivalencia = decimal.Parse(_doctoRelacionado.Equivalencia ?? "1");
        var monedaDr = _doctoRelacionado.Moneda;
        if (monedaP != monedaDr)
        {
            var limiteInferiorImpPagado =
                DecimalOperatorLimites.CalcularLimiteInferiorMonto(importePagado, equivalencia, decimalesMonedaP);
            var limiteSuperiorImpPagado = 
                DecimalOperatorLimites.CalcularLimiteSuperiorMonto(importePagado, equivalencia, decimalesMonedaP);
            _montoHelper.AddImpPagadoLimiteInferior(limiteInferiorImpPagado);
            _montoHelper.AddImpPagadoLimiteSuperior(limiteSuperiorImpPagado);
        }
        else
        {
            _montoHelper.AddImpPagadoLimiteInferior(importePagado);
            _montoHelper.AddImpPagadoLimiteSuperior(importePagado);
        }
    }

    private void ValidateImporteSaldoInsoluto()
    {
        var importeSaldoInsolutoString = _doctoRelacionado.ImporteSaldoInsoluto;
        if (string.IsNullOrEmpty(importeSaldoInsolutoString))
        {
            _context.AddError(
                code: "CRP20999",
                section: _section,
                message: "El valor del campo ImporteSaldoInsoluto no puede ser nulo ni vació.");
            return;
        }
        var importeSaldoInsoluto = decimal.Parse(importeSaldoInsolutoString);
        if (importeSaldoInsoluto < 0)
        {
            _context.AddError(
                code: "CRP20244",
                section: _section,
                message: "El valor del campo ImporteSaldoInsoluto debe mayor a cero.");
            return;
        }
        var countDecimals = ValidateHelper.CountDecimalPlaces(importeSaldoInsoluto);
        if (_decimalesMonedaDr != 0 && countDecimals > _decimalesMonedaDr)
        {
            _context.AddError(
                code: "CRP20243",
                section: _section,
                message: $"El valor del campo ImporteSaldoInsoluto debe corresponder a la moneda registrada en el campo" +
                         $" MonedaDR y ser redondeados hasta la cantidad de decimales que soporte. Valor de " +
                         $"ImporteSaldoInsoluto: {importeSaldoInsoluto} con {countDecimals} decimales. Decimales " +
                         $"soportados por la moneda: {_decimalesMonedaDr}.");
            return;
        }

        var saldoAnterior = decimal.Parse(_doctoRelacionado.ImporteSaldoAnterior);
        var montoPago = decimal.Parse(_doctoRelacionado.ImportePagado);
        if (montoPago > saldoAnterior)
        {
            _context.AddError(
                code: "CRP20999",
                section: _section,
                message: "");
            return;
        }
        var importeSaldoInsolutoEsperado = saldoAnterior - montoPago;
        
        if (importeSaldoInsoluto != importeSaldoInsolutoEsperado)
        {
            _context.AddError(
                code: "CRP20244",
                message: $"El campo ImporteSaldoInsoluto debe corresponder a la diferencia entre el importe del saldo " +
                         $"anterior y el monto del pago. Valor registrado {importeSaldoInsoluto}. Valor esperado {importeSaldoInsolutoEsperado}.",
                section: _section);
        }
    }

    private void ValidateObjetoImpuesto()
    {
        var objetoImpuesto = _doctoRelacionado.ObjetoImpuesto;
        if (string.IsNullOrEmpty(objetoImpuesto))
        {
            _context.AddError(
                code: "CRP20999",
                section: _section,
                message: "El valor del campo ObjetoImpuestoDR no puede ser nulo ni vació.");
            return;
        }

        if (!CatalogosComprobante.c_ObjetoImp.Contains(objetoImpuesto))
        {
            _context.AddError(
                code: "CRP20245",
                section: _section,
                message: "El valor del campo ObjetoImpuestoDR no pertenece al catalogo c_ObjetoImp.");
            return;
        }

        var impuestos = _doctoRelacionado.Impuestos;
        if (objetoImpuesto == "02" && impuestos == null)
        {
            _context.AddError(
                code: "CRP20246",
                section: _section,
                message: "Cuando valor del campo ObjetoImpuestoDR es '02' el nodo hijo ImpuestosDR debe existir.");
            return;
        }

        if (CatalogosComprobante.c_ObjetoImpNoImpuesto.Contains(objetoImpuesto) && impuestos != null)
        {
            _context.AddError(
                code: "CRP20247",
                section: _section,
                message: "Cuando valor del campo ObjetoImpuestoDR contiene el valor '01', '03','04' o '05', el nodo hijo ImpuestosDR del nodo DoctoRelacionado, no debe existir.");
            return;
        }

        //  Si este atributo contiene el valor "06" o "08", en el nodo hijo ImpuestosDR del nodo DoctoRelacionado no
        // deben existir los nodos hijo RetencionDR y TrasladoDR con el valor "002" y/o "003" en el atributo ImpuestoDR,
        // puede existir el nodo hijo RetencionDR con el valor "001" en el atributo ImpuestoDR.
        if (objetoImpuesto is "06" or "08")
        {
            
            if (impuestos == null)
                return;
            
            var retenciones = impuestos.Retenciones;
            var traslados = impuestos.Traslados;
            
            if ((retenciones == null || retenciones.Count == 0) && (traslados == null || traslados.Count == 0))
                return;
            
            var isValid = false;
            foreach (var retencion in retenciones)
            {
                if(retencion.Impuesto == "001")
                    isValid = true;
                else
                {
                    isValid = false;
                    break;
                }
            }
            
            
            if(traslados != null && traslados?.Count > 0)
                isValid = false;
            
            if(!isValid)
                _context.AddError(
                    code: "CRP20278",
                    message: "Si el atributo ObjetoImpDR contiene el valor '06' o '08' en el nodo hijo ImpuestosDR del nodo DoctoRelacionado no " +
                             "deben existir los nodos hijo RetencionDR y TrasladoDR con el valor '002' y/o '003' en el " +
                             "atributo ImpuestoDR, puede existir el nodo hijo RetencionDR con el valor '001' en el atributo ImpuestoDR.",
                    section: _section);
            return;
        }
        //  Si este atributo contiene el valor “07", en el nodo hijo ImpuestosDR del nodo DoctoRelacionado no deben
        // existir los nodos hijo RetencionDR y TrasladoDR con el valor "002" en el atributo ImpuestoDR; puede existir
        // el nodo RetencionesDR, con al menos un nodo hijo RetencionDR con el valor "001" en el atributo ImpuestoDR;
        // debe existir el nodo hijo TrasladoDR con el valor "003" en el atributo ImpuestoDR y puede existir el nodo hijo
        // RetencionDR con el valor "003" en el atributo ImpuestoDR.
        if (objetoImpuesto is "07")
        {
            
            if (impuestos == null)
            {
                _context.AddError(
                    code: "CRP20279",
                    message: "Cuando se registre  el valor '07' en el campo ObjetoImpDR, en el nodo hijo ImpuestosDR del nodo DoctoRelacionado debe existir al menos un nodo hijo TrasladoDR con el valor '003' (IEPS).",
                    section: _section);
                return;
            }
            var retenciones = impuestos.Retenciones;
            var traslados = impuestos.Traslados;
            
            // En traslados debe existir al menos un hijo con el valor 003 
            if (traslados == null || traslados.Count == 0)
            {
                _context.AddError(
                    code: "CRP20279",
                    message: "Cuando se registre  el valor '07' en el campo ObjetoImpDR, en el nodo hijo ImpuestosDR del nodo DoctoRelacionado debe existir al menos un nodo hijo TrasladoDR con el valor '003' (IEPS).",
                    section: _section);
                return;
            }
            var isValid = false;
            // Validar RetencionesDR
            if (retenciones != null || retenciones?.Count > 0)
            {
                // No deben existir retenciones con el valor 002
                foreach (var retencion in retenciones)
                {
                    if (retencion.Impuesto == "002")
                    {
                        _context.AddError(
                            code: "CRP20279",
                            message: "Cuando se registre  el valor '07' en el campo ObjetoImpDR, en el nodo hijo ImpuestosDR del nodo DoctoRelacionado no deben existir los nodos hijo RetencionDR con el valor '002' en el atributo ImpuestoDR.",
                            section: _section);
                        return;
                    }
                    // Debe existir al menos un nodo hijo RetencionDR con el valor "001" en el atributo ImpuestoDR
                    if (retencion.Impuesto == "001")
                    {
                        isValid = true;
                    }
                    
                }
                // Si no encontro ninguna RetencionDR con el valor "001" en el atributo ImpuestoDR agregar error
                if (!isValid)
                {
                    _context.AddError(
                        code: "CRP20279",
                        message: "Cuando se registre  el valor '07' en el campo ObjetoImpDR, en el nodo hijo ImpuestosDR del nodo DoctoRelacionado el nodo RetencionesDR debe tener al menos un nodo hijo RetencionDR con el valor '001' en el atributo ImpuestoDR",
                        section: _section);
                    return;
                }
            }
            isValid = false;
            // No deben existir traslados con el valor 002 y debe existir al menos un traslado con el valor 003
            foreach (var traslado in traslados)
            {
                if (traslado.Impuesto == "002")
                {
                    _context.AddError(
                        code: "CRP20279",
                        message: "Cuando se registre  el valor '07' en el campo ObjetoImpDR, en el nodo hijo ImpuestosDR del nodo DoctoRelacionado no deben existir los nodos hijo TrasladoDR con el valor '002' en el atributo ImpuestoDR.",
                        section: _section);
                    return;
                }
                if(traslado.Impuesto == "003")
                    isValid = true;
                
            }
            if (!isValid)
            {
                _context.AddError(
                    code: "CRP20279",
                    message: "Cuando se registre  el valor '07' en el campo ObjetoImpDR, en el nodo hijo ImpuestosDR del nodo DoctoRelacionado debe existir al menos un nodo hijo TrasladoDR con el valor '003' (IEPS).",
                    section: _section);
                return;
            }
        }
    }
}