using KPac.Application.Validator;
using KpacModels.Shared.Models.Comprobante;
using KpacModels.Shared.Services.Interfaces;
using KpacModels.Shared.XmlProcessing.Validator.Catalogos;
using KpacModels.Shared.XmlProcessing.Validator.Comprobante.Interface;

namespace KpacModels.Shared.XmlProcessing.Validator.Comprobante;

public class CfdiRelacionadoValidator : INumElementValidatorAsync<CfdiRelacionado>
{
    private readonly IKoreService _koreService;
    
    private ValidatorContext _context;

    private string _rfcEmisor;

    private string _tipoComprobante;
    public CfdiRelacionadoValidator(IKoreService koreService)
    {
        _koreService = koreService;
        _context = new ValidatorContext();
        _rfcEmisor = string.Empty;
    }
    
    public async Task Validate(CfdiRelacionado cfdiRelacionado, int numCfdi, ValidatorContext comprobanteContext)
    {
        _context = comprobanteContext;
        _rfcEmisor = _context.GetValue("rfcEmisor") ?? string.Empty;
        _tipoComprobante = _context.GetValue("tipoComprobante") ?? string.Empty;
        var tipoRelacion = cfdiRelacionado.TipoRelacion;
        if (!CatalogosComprobante.c_TipoRelacion.Contains(tipoRelacion))
        {
            _context.AddError(
                code: "CFDI40137",
                section: $"Comprobante -> {numCfdi}.- CfdiRelacionados",
                message: $"El campo TipoRelacion, no contiene un valor del catálogo c_TipoRelacion. Valor registrado {tipoRelacion}");
            return;
        }
        var count = cfdiRelacionado.UuidsRelacionados.Count;
        for (var i = 0; i < count; i++)
        {
            var section = $"Comprobante -> {numCfdi}.- CfdiRelacionados -> {i + 1}.- Uuid";
            var uuid = cfdiRelacionado.UuidsRelacionados[i].Uuid;
            if (!Guid.TryParse(uuid, out _))
            {
                _context.AddError(
                    code: "CFDI40999",
                    section: section,
                    message: $"El UUID = {uuid} no cumple con el patrón establecido.");
            }

            await SearchAndValidateUuidInKore(uuid, tipoRelacion, section);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="uuid"></param>
    /// <param name="tipoRelacion"></param>
    private async Task SearchAndValidateUuidInKore(string uuid, string tipoRelacion, string section)
    {
        var comprobante = await _koreService.GetComprobanteFromKoreByUuid(uuid, _rfcEmisor);
        
        if (comprobante == null)
        {
            _context.AddWarning(
                section: section,
                message: $"No se pudo validar el UUID = {uuid}: no se encontró ningún comprobante asociado en el sistema."
                );
            return;
        }

        var tipoComprobante = comprobante.TipoComprobante;
        ValidateRelationship(tipoComprobante, tipoRelacion, section);
    }

    private void ValidateRelationship(string tipoComprobanteRelacionado, string tipoRelacion, string section)
    {
        switch (tipoRelacion)
        {
            case "01":
                // Cuando es una sustitución 1:1 el Cfdi actual debe ser E (Egreso) y el Cfdi relacionado debe ser de I (Ingreso)
                if(_tipoComprobante == "E" && tipoComprobanteRelacionado == "I")
                    return;
                // En otro caso debe ser del mismo tipo de comprobante
                if(_tipoComprobante == tipoComprobanteRelacionado)
                    return;
                // Si no se cumplen las condiciones agregar warning
                _context.AddWarning(
                    section: section,
                    message: "Cuando se utiliza un tipo de relacion 01 y es una sustitución 1:1 el tipo de comprobante" +
                             " actual debe ser E (Egreso) y el Cfdi relacionado debe ser I (Ingreso), en otro caso deben" +
                             $" ser del mismo tipo de comprobante. Tipo de comprobante actual {_tipoComprobante}. Tipo de comprobante relacionado {tipoComprobanteRelacionado}.");
                break;
            case "02":
                // Si se utiliza un tipo de relación 02 el comprobante debe ser tipo E (Egreso) y puede relacionarse con Cfdi's de tipo I (Ingreso) o P (Pago)
                if(_tipoComprobante == "E" && tipoComprobanteRelacionado is "I" or "P")
                    return;
                _context.AddWarning(
                    section: section,
                    message: $"Cuando se utiliza un tipo de relación 02 el comprobante debe ser tipo E (Egreso) y el comprobante relacionado debe ser del tipo I (Ingreso) o P (Pago). Tipo de comprobante actual {_tipoComprobante}. Tipo de comprobante relacionado {tipoComprobanteRelacionado}.");
                break;
            case "03":
                // Cuando el tipo de relación tenga la clave 03, no se deben registrar devoluciones de mercancías
                // sobre comprobantes de tipo E (Egreso), P (Pago) o N (Nómina)
                // Aplica a Cfdi de tipo E (Egreso) y el Cfdi relacionado debe ser I (Ingreso) o T (Traslado)
                if (_tipoComprobante is "E" && tipoComprobanteRelacionado is "I" or "T")
                    return;
                
                _context.AddWarning(
                    section: section,
                    message: $"Cuando el tipo de relación tenga la clave 03, el comprobante debe ser tipo E (Egreso) y el comprobante relacionado debe ser I (Ingreso) o T (Traslado). Tipo de comprobante actual {_tipoComprobante}. Tipo de comprobante relacionado {tipoComprobanteRelacionado}.");
                
                break;
            case "04":
                // Cuando el tipo de relación tenga la clave 04, si este documento que se está generando es de 
                // tipo I (Ingreso) o E (Egreso), puede sustituir a un comprobante de tipo I (Ingreso) o E (Egreso),
                // en otro caso debe de sustituir a un comprobante del mismo tipo.
                if(tipoComprobanteRelacionado is "I" or "E" && _tipoComprobante is "I" or "E")
                    return;
                if(tipoComprobanteRelacionado == _tipoComprobante)
                    return;
                _context.AddWarning(
                    section: section,
                    message: "Cuando el tipo de relación tenga la clave 04, si este documento que se está generando es" +
                             " de tipo I (Ingreso) o E (Egreso), puede sustituir a un comprobante de tipo I (Ingreso) o" +
                             $" E (Egreso), en otro caso debe de sustituir a un comprobante del mismo tipo. Tipo de " +
                             $"comprobante actual {_tipoComprobante}. Tipo de comprobante relacionado {tipoComprobanteRelacionado}.");
                break;
            case "05":
                // Este documento que se está generando debe ser de tipo T (Traslado), y los documentos relacionados
                // deben ser un comprobante de tipo I (Ingreso) o E (Egreso).
                if(_tipoComprobante is "T" && tipoComprobanteRelacionado is "I" or "E")
                    return;
                _context.AddWarning(
                    section: section,
                    message: $"Este documento que se está generando debe ser de tipo T (Traslado), y los documentos " +
                             $"relacionados deben ser un comprobante de tipo I (Ingreso) o E (Egreso). Tipo de comprobante actual {_tipoComprobante}. Tipo de comprobante relacionado {tipoComprobanteRelacionado}.");
                break;
            case "06":
                // Este documento que se está generando debe ser de tipo I (Ingreso) o E (Egreso) y los documentos
                // relacionados deben ser de tipo T (Traslado).
                if(_tipoComprobante is "I" or "E" && tipoComprobanteRelacionado is "T")
                    return;
                _context.AddWarning(
                    section: section,
                    message: $"El comprobante debe ser de tipo I (Ingreso) o E (Egreso) y los" +
                             $" comprobantes relacionados deben ser de tipo T (Traslado). Tipo de comprobante actual {_tipoComprobante}. Tipo de comprobante relacionado {tipoComprobanteRelacionado}.");
                break;
            case "07":
                // Este documento que se está generando debe ser de tipo I (Ingreso) o E (Egreso) y los documentos
                // relacionados deben ser de  tipo I (Ingreso) o E (Egreso).
                if(_tipoComprobante is "I" or "E" && tipoComprobanteRelacionado is "I" or "E")
                    return;
                _context.AddWarning(
                    section: section,
                    message: $"Este documento que se está generando debe ser de tipo I (Ingreso) o E (Egreso) y los documentos relacionados deben ser de  tipo I (Ingreso) o E (Egreso). Tipo de comprobante actual {_tipoComprobante}. Tipo de comprobante relacionado {tipoComprobanteRelacionado}.");
                break;
        }
    }
}