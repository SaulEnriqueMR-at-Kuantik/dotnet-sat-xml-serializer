using KPac.Application.Validator;
using KpacModels.Shared.Models.Comprobante;
using KpacModels.Shared.XmlProcessing.Validator.Catalogos;

namespace KpacModels.Shared.XmlProcessing.Validator.Comprobante.ConceptoValidate;

public class ACuentaTercerosConcepto
{
    private ValidatorContext _context;
    private string _section;
    private string _rfcEmisor;
    private string _rfcReceptor;

    //private RepositoryValidator<cfdi40_tasaocuota> _repository;
    public ACuentaTercerosConcepto(ValidatorContext context,  int numConcepto
        //, ClientValidator clientValidator
        )
    {
        _context = context;
        _section = $"Comprobante -> {numConcepto}.-Concepto -> ACuentaTerceros";
        _rfcEmisor = _context.GetValue("rfcEmisor") ?? string.Empty;
        _rfcReceptor = _context.GetValue("rfcReceptor") ?? string.Empty;
    }

    public void Validate(ACuentaTerceros cuentaTerceros)
    {
        ValidateRfc(cuentaTerceros.Rfc);
        ValidateNombre(cuentaTerceros.Nombre);
        ValidateRegimenFiscal(cuentaTerceros.RegimenFiscal);
        ValidateDomicilio(cuentaTerceros.DomicilioFiscal);
    }

    private void ValidateRfc(string rfc)
    {
        // Validar que el Rfc de Emisor siga el patron del SAT
        if (!RegexCatalog.IsRfcValid(rfc))
        {
            _context.AddWarning(
                section: _section,
                message: $"Formato invalido de Rfc Emisor. El Rfc {rfc} no sigue el patron establecido.");
            return;
        }

        if (rfc == _rfcReceptor || rfc == _rfcEmisor)
        {
            _context.AddError(
                code: "CFDI40188",
                section: _section,
                message: "El valor del campo RfcACuentaTerceros, debe ser diferente de los valores de los campos Rfc" +
                         $" del Emisor y Receptor. Valor registrado: {rfc}. Rfc del emisor: {_rfcEmisor}. Rfc del Receptor: {_rfcReceptor}");
        }
        // TODO
        //  - **CFDI40187**
        //  - Si el valor de este atributo, es distinto de “EXT990101NI1”, debe encontrarse en la lista l_LCO.
        // 	- El valor registrado en el campo RfcACuentaTerceros, no se encuentra en la lista l_LCO.
    }

    private void ValidateNombre(string nombre)
    {
        // TODO
        //  **CFDI40189**
        // 	- Si el valor del atributo RfcACuentaTerceros es distinto de “EXT990101NI1", debe encontrarse en la lista de RFC inscritos no cancelados en el SAT, en otro caso debe contener la descripción  “EXPEDICIÓN DE CFDI POR RESIDENTES EN MÉXICO QUE PRESTAN SERVICIOS DE INTERMEDIACIÓN ENTRE TERCEROS A OFERENTES DE BIENES Y SERVICIOS RESIDENTES EN EL EXTRANJERO”.
        // 		- El valor  registrado debe encontrarse en la lista de RFC inscritos no cancelados en el SAT, en otro caso debe contener la descripción “EXPEDICIÓN DE CFDI POR RESIDENTES EN MÉXICO QUE PRESTAN SERVICIOS DE INTERMEDIACIÓN ENTRE TERCEROS A OFERENTES DE BIENES Y SERVICIOS RESIDENTES EN EL EXTRANJERO”.
        //  - **CFDI40190**
        // 	- Este atributo, debe pertenecer al nombre asociado al RFC registrado en el atributo Rfc del Nodo ACuentaTerceros.
        // 		- El campo NombreACuentaTerceros, debe pertenecer al nombre asociado al RFC registrado en el campo Rfc del tercero.
    }

    private void ValidateRegimenFiscal(string regimenFiscal)
    {
        if (!CatalogosComprobante.c_RegimenFiscal.Contains(regimenFiscal))
        {
            _context.AddError(
                code: "CFDI40191",
                section: _section,
                message: $"El campo RegimenFiscalACuentaTerceros, no contiene un valor del catálogo c_RegimenFiscal. Valor registrado: {regimenFiscal}.");
        }
    }

    private void ValidateDomicilio(string domicilio)
    {
        // TODO
        //  - **CFDI40192**
        // 	- El valor de este atributo debe encontrarse en la lista de RFC inscritos no cancelados en el SAT.
        // 		- El valor registrado en el atributo DomicilioFiscalACuentaTerceros, debe encontrarse en la lista de RFC inscritos no cancelados en el SAT.
        //  - **CFDI40193**
        // 	- El valor de este atributo, debe pertenecer al nombre asociado al RFC registrado en el atributo RfcACuentaTerceros del Nodo ACuentaTerceros, en caso de que el valor del atributo RfcACuentaTerceros sea "EXT990101NI1", el valor registrado en éste atributo debe ser igual al valor del atributo "LugarExpedicion".
        // 		- El valor registrado en el atributo DomicilioFiscalACuentaTerceros, debe pertenecer al nombre asociado al RFC registrado en el campo Rfc del tercero, o debe ser igual al valor del atributo "LugarExpedicion".
    }
}