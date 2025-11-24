using KPac.Application.Validator;
using KpacModels.Shared.Models.Comprobante;
using KpacModels.Shared.XmlProcessing.Validator.Catalogos;

namespace KpacModels.Shared.XmlProcessing.Validator.Comprobante;

public class EmisorValidator
{
    
    private ValidatorContext _context = new();
    public void Validate(Emisor emisor, ValidatorContext comprobanteContext)
    {
        _context = comprobanteContext;
        ValidateRfc(emisor.Rfc);
        ValidateNombre(emisor.Nombre);
        ValidateRegimenFiscal(emisor.RegimenFiscal, emisor.Rfc);
        ValidateFacAtrAdquiriente(emisor.FacAtrAdquirent);
    }

    private void ValidateRfc(string rfc)
    {
        // Validar que Rfc no sea nulo o  vació
        if (string.IsNullOrEmpty(rfc))
        {
            _context.AddWarning(
                section: "Comprobante -> Emisor",
                message: "El Rfc de Emisor no puede estar vació.");
            return;
        }
        // Validar que el Rfc de Emisor siga el patron del SAT
        if (!RegexCatalog.IsRfcValid(rfc))
        {
            _context.AddWarning(
                section: "Comprobante -> Emisor",
                message: $"Formato invalido de Rfc Emisor. El Rfc {rfc} no sigue el patron establecido.");
            return;
        }
        _context.AddValue("rfcEmisor", rfc);
    }

    private void ValidateNombre(string nombre)
    {
        // TODO
        //  Este atributo, debe encontrarse en la lista de RFC inscritos no cancelados en el SAT.
        //  El campo Nombre del emisor, debe pertenecer al nombre asociado al RFC registrado en el campo Rfc del Emisor.
        
    }

    private void ValidateRegimenFiscal(string? regimenFiscal, string rfc)
    {
        if (string.IsNullOrEmpty(regimenFiscal))
        {
            _context.AddWarning(
                section: "Comprobante -> Emisor",
                message: "Regimen Fiscal no puede ser nulo o vació.");
            return;
        }
        
        
        // Validar que RegimenFiscal este en c_RegimenFiscal
        if (!CatalogosComprobante.c_RegimenFiscal.Contains(regimenFiscal))
        {
            _context.AddError(
                code: "CFDI40140",
                section: "Comprobante -> Emisor",
                message: "El campo RegimenFiscal, no contiene un valor del catálogo c_RegimenFiscal.");
            return;
        }
        // Si es persona física, validar que RegimenFiscal sea correspondiente a persona física
        if (rfc.Length == 13)
        {
            if (!CatalogosComprobante.listRegimenFiscalFisica.Contains(regimenFiscal))
            {
                _context.AddError(
                    code: "CFDI40141",
                    section: "Comprobante -> Emisor",
                    message: "El Rfc emisor corresponde a persona física, el regimen fiscal no corresponde con el " +
                             "tipo de persona");
                return;
            }
        }
        
        // Si es persona moral, validar que RegimenFiscal sea correspondiente a persona moral
        if (rfc.Length == 12)
        {
            if (!CatalogosComprobante.listRegimenFiscalMoral.Contains(regimenFiscal))
            {
                _context.AddError(
                    code: "CFDI40141",
                    section: "Comprobante -> Emisor",
                    message: "El Rfc emisor corresponde a persona moral, el regimen fiscal no corresponde con el " +
                             "tipo de persona");
                return;
            }
        }
        _context.AddValue("regimenFiscalEmisor", regimenFiscal);
    }

    private void ValidateFacAtrAdquiriente(string? emisorFacAtrAdquirent)
    {
        // TODO
        //  Este atributo, debe contener el número de operación  siempre que la respuesta del servicio del
        //  Validador de RFC para emitir facturas a través del adquirente, sea en sentido positivo.
    }
}