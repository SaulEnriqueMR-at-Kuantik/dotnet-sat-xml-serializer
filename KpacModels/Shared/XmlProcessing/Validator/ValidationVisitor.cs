using KpacModels.Shared.Models.Comprobante;
using KpacModels.Shared.Models.Core;
using KpacModels.Shared.XmlProcessing.Validator.Comprobante;
using KpacModels.Shared.XmlProcessing.Validator.Comprobante.Interface;
using KpacModels.Shared.XmlProcessing.Validator.Interface;
using Emisor = KpacModels.Shared.Models.Comprobante.Emisor;
using Receptor = KpacModels.Shared.Models.Comprobante.Receptor;

namespace KpacModels.Shared.XmlProcessing.Validator;

/// <summary>
/// Clase para la validación de Comprobante 4.0.
/// Empleando Patrón de comportamiento visitor
/// 
/// </summary>
public class ValidationVisitor : IVisitor
{
    public readonly ValidatorContext Context = new();
    private EmisorValidator _emisorValidator = new ();
    private ReceptorValidator _receptorValidator = new ();
    private InformacionGlobalValidator _informacionGlobalValidator = new ();
    private ImpuestosValidator _impuestosValidator = new ();
    private INumElementValidatorAsync<CfdiRelacionado> _cfdiRelacionadoValidator;
    private readonly ComprobanteValidator _validator;
    //private readonly ClientValidator _clientValidator;
    
    public ValidationVisitor(
        //ClientValidator clientValidator,
        INumElementValidatorAsync<CfdiRelacionado> cfdiRelacionadoValidator)
    {
        _cfdiRelacionadoValidator = cfdiRelacionadoValidator;
        //_clientValidator = clientValidator;
        //_validator = new ComprobanteValidator(clientValidator);
    }
 
    public void Visit(Comprobante40 root)
    {
        _validator.Validate(root, Context);
    }

    public bool HasErrors()
    {
        return Context.HasErrors();
    }

    public (List<Warning>, List<Error>) GetValidationResult()
    {
        var warnings = Context.GetWarnings()?.ToList();
        var errors = Context.GetErrors()?.ToList();
        Context.CleanContext();
        return (warnings, errors);
    }

    public async Task VisitBasic(Comprobante40 root)
    {
        await _validator.ValidateBase(root, Context);
    }

    public void Visit(InformacionGlobal? informacionGlobal)
    {
        _informacionGlobalValidator.Validate(informacionGlobal, Context);
    }

    public void Visit(Emisor emisor)
    {
        _emisorValidator.Validate(emisor, Context);
    }

    public async Task Visit(CfdiRelacionado cfdiRelacionado, int noCfdi)
    {
        await _cfdiRelacionadoValidator.Validate(cfdiRelacionado, noCfdi, Context);
    }

    public void Visit(Receptor receptor)
    {
        _receptorValidator.Validate(receptor, Context);
    }

    public async Task Visit(Concepto conceptos, int numConcepto)
    {
        //ConceptosValidator conceptosValidator = new(_clientValidator, numConcepto, Context);
        //await conceptosValidator.Validate(conceptos);
    }

    public void Visit(Impuestos? impuestos)
    {
        if(impuestos == null) return;
        _impuestosValidator.Validate(impuestos, Context);
    }
    
}