using KPac.Application.Validator;
using KpacModels.Shared.Models.Comprobante;
using KpacModels.Shared.XmlProcessing.Validator.Catalogos;

namespace KpacModels.Shared.XmlProcessing.Validator.Comprobante;

public class ReceptorValidator
{
    private ValidatorContext _context = new();
    
    private string _rfc = string.Empty;
    
    private bool _hasComercioExterior;
    public void Validate(Receptor receptor, ValidatorContext comprobanteContext)
    {
        _hasComercioExterior = (_context.GetValue("HasComercioExterior") != null);
        _context = comprobanteContext;
        var tipoComprobante = comprobanteContext.GetValue("tipoComprobante");
        ValidateRfc(receptor.Rfc);
        ValidateNombre(receptor.Nombre);
        ValidateDomicilioFiscal(receptor.DomicilioFiscal);
        ValidateResidenciaFiscal(receptor.ResidenciaFiscal, receptor.NumRegIdTrib);
        ValidateNumRegIdTrib(receptor.NumRegIdTrib);
        ValidateRegimenFiscal(receptor.RegimenFiscal);
        ValidateUsoCfdi(receptor.UsoCfdi);
    }

    private void ValidateRfc(string rfc)
    {
        _rfc = rfc;
        
        if (!RegexCatalog.IsRfcValid(rfc))
        {
            _context.AddWarning(
                section: "Comprobante -> Receptor",
                message: $"Formato invalido de Rfc Receptor. El Rfc {rfc} no sigue el patron establecido.");
            return;
        }
        // TODO
        //  Cuando no se utilice un RFC genérico (XAXX010101000) el RFC debe estar en la lista de RFC inscritos no
        //  cancelados en el SAT.
        _context.AddValue("rfcReceptor",rfc);
    }

    private void ValidateNombre(string nombre)
    {
        if (string.IsNullOrEmpty(nombre))
        {
            _context.AddWarning(
                section: "Comprobante -> Receptor",
                message: "Nombre no puede ser nulo o vació");
        }
        if (nombre == "PUBLICO EN GENERAL" && _rfc != "XAXX010101000")
        {
            _context.AddError(
                code: "CFDI40146",
                section: "Comprobante -> Receptor",
                message: "Cuando el atributo Nombre tiene el valor “PUBLICO EN GENERAL”, el atributo RFC de Receptor debe tener el RFC genérico (XAXX010101000),  ");
        }
        
        // TODO
        //  Este atributo, debe pertenecer al nombre asociado al RFC registrado en el atributo Rfc del Nodo Receptor.
    }

    private void ValidateDomicilioFiscal(string domicilioFiscal)
    {
        // Si el valor del atributo Rfc del receptor es 'XAXX010101000' o 'XEXX010101000', este atributo debe
        // ser igual al valor del atributo LugarExpedicion.
        if (_rfc is "XAXX010101000" or "XEXX010101000")
        {
            // Acceder al Bag para obtener LugarExpedicion.
            var lugarExpedicion = _context.GetValue("lugarExpedicion");
            if (lugarExpedicion != domicilioFiscal)
            {
                _context.AddError(
                    code: "CFDI40149",
                    section: "Comprobante -> Receptor",
                    message: "Si el valor del atributo Rfc del receptor es 'XAXX010101000' o 'XEXX010101000', este atributo debe ser igual al valor del atributo LugarExpedicion.");
            }
        }
        // TODO
        //  Este atributo, debe encontrarse en la lista de RFC inscritos no cancelados en el SAT.
    }

    private void ValidateResidenciaFiscal(string? residenciaFiscal, string? numRegIdTrib)
    {
        // TODO
        //  Si el RFC del receptor es de un RFC registrado en el SAT o un RFC genérico nacional, este atributo NO debe existir.
        
        // Si el RFC del receptor es un RFC genérico extranjero y el comprobante incluye el complemento de Comercio
        // Exterior. Debe existir ResidenciaFiscal
        if (_rfc == "XEXX010101000" && _hasComercioExterior && string.IsNullOrEmpty(residenciaFiscal))
        {
            _context.AddError(
                code: "CFDI40153",
                section: "Comprobante -> Receptor",
                message: "Si el RFC del receptor es un RFC genérico extranjero y el comprobante incluye el complemento de comercio exterior, ResidenciaFiscal debe existir.");
            return;
        }
        
        // Si NumRegIdTrib tiene algun valor debe existir ResidenciaFiscal.
        if (!string.IsNullOrEmpty(numRegIdTrib) && string.IsNullOrEmpty(residenciaFiscal))
        {
            _context.AddError(
                code: "CFDI40153",
                section: "Comprobante -> Receptor",
                message: "Se debe registrar un valor en en el campo ResidenciaFiscal, cuando en el campo NumRegIdTrib se registre información.");
        }
        
        // Si es nulo o vació no regresa nada.
        if(string.IsNullOrEmpty(residenciaFiscal)) return;
        
        // Validar que residencial fiscal contiene un valor del catálogo c_Pais.
        if (!CatalogosComprobante.c_Pais.Contains(residenciaFiscal))
        {
            _context.AddError(
                code: "CFDI40150",
                section: "Comprobante -> Receptor",
                message: "El campo ResidenciaFiscal, no contiene un valor del catálogo c_Pais.");
        }
        
        // ResidenciaFiscal no puede ser MEX.
        if (residenciaFiscal == "MEX")
        {
            _context.AddError(
                code: "CFDI40152",
                section: "Comprobante -> Receptor",
                message: "El valor del campo ResidenciaFiscal no puede ser MEX.");
        }
        
    }

    private void ValidateNumRegIdTrib(string? numRegIdTrib)
    {
        // TODO
        //   Este atributo  debe cumplir con el patrón correspondiente incluido en la columna “Formato de Registro de
        //  Identidad Tributaria” que publique en el catalogo de C_Pais.
        
        // TODO
        //  Si el RFC del receptor es de un RFC registrado en el SAT o un RFC genérico nacional, este atributo NO debe existir.
        
        if (_rfc == "XAXX010101000" && !string.IsNullOrEmpty(numRegIdTrib))
        {
            _context.AddError(
                code: "CFDI40154",
                section: "Comprobante -> Receptor",
                message: "El RFC del receptor es de un RFC registrado en el SAT o un RFC genérico nacional, NumRegIdTrib no debe estar registrado.");
            return;
        }
        // Si el RFC del receptor es un RFC genérico extranjero y el comprobante incluye el complemento de Comercio
        // Exterior. Debe existir NumRegIdTrib
        if (_rfc == "XEXX010101000" && _hasComercioExterior && string.IsNullOrEmpty(numRegIdTrib))
        {
            _context.AddError(
                code: "CFDI40155",
                section: "Comprobante -> Receptor",
                message: "Si el RFC del receptor es un RFC genérico extranjero y el comprobante incluye el complemento de comercio exterior, ResidenciaFiscal debe existir.");
            return;
        }
    }

    private void ValidateRegimenFiscal(string regimenFiscal)
    {
        // RegimenFiscal es obligatorio.
        if (string.IsNullOrEmpty(regimenFiscal))
        {
            _context.AddWarning(
                section: "Comprobante -> Receptor",
                message: "RegimenFiscalReceptor no debe ser nulo ni vació");
            return;
        }
        //  RegimenFiscalR debe contener un valor del catálogo c_RegimenFiscal.
        if (!CatalogosComprobante.c_RegimenFiscal.Contains(regimenFiscal))
        {
            _context.AddError(
                code: "CFDI40157",
                section: "Comprobante -> Receptor",
                message: "El campo RegimenFiscalR, no contiene un valor del catálogo c_RegimenFiscal.");
            return;
        }
        
        // Si es Rfc persona física, validar que RegimenFiscalR sea correspondiente a persona física
        if (_rfc.Length == 13)
        {
            if (!CatalogosComprobante.listRegimenFiscalFisica.Contains(regimenFiscal))
            {
                _context.AddError(
                    code: "CFDI40158",
                    section: "Comprobante -> Receptor",
                    message: "El Rfc receptor es persona física, el RegimelFiscalR no corresponde con el " +
                             "tipo de persona");
                return;
            }
        }
        
        // Si es Rfc persona moral, validar que RegimenFiscal sea correspondiente a persona moral
        if (_rfc.Length == 12)
        {
            if (!CatalogosComprobante.listRegimenFiscalMoral.Contains(regimenFiscal))
            {
                _context.AddError(
                    code: "CFDI40158",
                    section: "Comprobante -> Receptor",
                    message: "El Rfc del emisor es persona moral, el RegimelFiscalR no corresponde con el " +
                             "tipo de persona");
                return;
            }
        }

        if ((_rfc is "XAXX010101000" or "XEXX010101000") && regimenFiscal != "616")
        {
            _context.AddError(
                code: "CFDI40159",
                section: "Comprobante -> Receptor",
                message: "Si el atributo Rfc del Receptor contiene el valor “XAXX010101000” o el valor “XEXX010101000”, " +
                         "se debe registrar la clave “616” en RegimenFiscalR.");
        }
        
    }

    private void ValidateUsoCfdi(string usoCfdi)
    {
        // Validar que no venga nulo o vació.
        if (string.IsNullOrEmpty(usoCfdi))
        {
            _context.AddWarning(
                section: "Comprobante -> Receptor",
                message: "UsoCfdi es requerido.");
        }

        if (!CatalogosComprobante.c_UsoCFDI.Contains(usoCfdi))
        {
            _context.AddError(
                section: "Comprobante -> Receptor",
                code: "CFDI40160",
                message: "El campo UsoCFDI, no contiene un valor del catálogo c_UsoCFDI.");
        }


        if (_rfc == "XEXX010101000" && usoCfdi != "S01")
        {
            _context.AddWarning(
                section: "Comprobante -> Receptor",
                message: "Si se registra un Rfc generico extranjero (XEXX010101000), UsoCFDI debe tener registrado el " +
                         "valor 'S01'.");
        }
        
        // Si es Rfc persona moral, verificar que UsoCFDI no este presente en la lista donde no corresponde UsoCFDI moral.
        if (_rfc.Length == 12)
        {
            if (CatalogosComprobante.listUsoCfdiExcepcionesMoral.Contains(usoCfdi))
            {
                _context.AddError(
                    code: "CFDI40161",
                    section: "Comprobante -> Receptor",
                    message: "El Rfc del emisor es persona moral, el RegimelFiscalR no corresponde con el " +
                             "tipo de persona");
                return;
            }
        }
    }
}