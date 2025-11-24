using KPac.Application.Validator;
using KpacModels.Shared.Models.Comprobante.Complementos.Pagos;
using KpacModels.Shared.Models.Core;
using KpacModels.Shared.XmlProcessing.Validator.Interface;
using KpacModels.Shared.XmlProcessing.Validator.Pagos.Impuestos;

namespace KpacModels.Shared.XmlProcessing.Validator.Pagos;

public class PagosValidator : IVisitorPagos
{
    
    public readonly ValidatorContext Context = new();
    
    private readonly PagoValidator _pagoValidator;
    
    private readonly DoctoRelacionadoValidator _doctoValidator;
    
    //private readonly ClientValidator _clientValidator;
    
    private readonly ImpuestosDrValidate _impuestosDrValidator;
    
    private readonly TotalesValidator _totalesValidator;

    public PagosValidator()
        //ClientValidator client)
    {
        //_clientValidator = client;
        //_pagoValidator = new PagoValidator(client);
        _doctoValidator = new DoctoRelacionadoValidator();
        _impuestosDrValidator = new ImpuestosDrValidate();
    }
    public void Visit(Pagos20 root)
    {
        if (root.Version != "2.0")
        {
            Context.AddError(
                code: "CRP20999",
                section: "Pagos",
                message: $"El versión del complemento Pagos debe ser '2.0'. Valor registrado: {root.Version}.");
        }
    }

    public void Visit(Totales totales, List<RetencionP> retencionesTotales, List<TrasladoP> trasladosTotales)
    {
        var totalesValidator = new TotalesValidator(Context);
        totalesValidator.Validate(totales, retencionesTotales, trasladosTotales);
    }

    public async Task Visit(Pago pago, int num)
    {
        await _pagoValidator.Validate(pago, num, Context);
    }

    public async Task Visit(DoctoRelacionado doctoRelacionado, int numPago, int numDocto)
    {
         //await _doctoValidator.Validate(Context, doctoRelacionado, _clientValidator, numPago, numDocto);
    }

    public void Visit(ImpuestosDR impuestosDr, int numPago, int numDocto)
    {
        var section =
            $"Comprobante -> Complemento -> Pagos -> {numPago}. Pago -> {numDocto}. DoctoRelacionado -> ImpuestosDR";
        if (impuestosDr != null)
        {
            var retenciones = impuestosDr.Retenciones;
            var traslados = impuestosDr.Traslados;
            if ((retenciones == null || retenciones.Count == 0) && (traslados == null || traslados.Count == 0))
            {
                Context.AddError(
                    code: "CRP20248",
                    section: section,
                    message: "En caso de utilizar el nodo Impuestos en un documento relacionado, se deben incluir impuestos  de traslados y/o retenciones.");
            }
        }
    }

    public void Visit(RetencionDR retencion, int numPago, int numDocto, int noRetencion)
    {
        var section = $"Comprobante -> Complemento -> Pagos -> {numPago}. Pago -> {numDocto}. DoctoRelacionado -> ImpuestosDR -> {noRetencion}. Retención";
        _impuestosDrValidator.ValidateRetencion(retencion: retencion, context: Context, section: section);
        
    }
    
    public void Visit(TrasladoDR traslado, int numPago, int numDocto, int noTraslado)
    {
        var section = $"Comprobante -> Complemento -> Pagos -> {numPago}. Pago -> {numDocto}. DoctoRelacionado -> ImpuestosDR -> {noTraslado}. Traslado";
        _impuestosDrValidator.ValidateTraslado(traslado: traslado, context: Context, section: section);
    }

    public void Visit(ImpuestosP impuestosP, int noPago)
    {
        var impuestosValidator = new ImpuestosPValidate(Context);
        
        impuestosValidator.Validate(impuestosP, noPago);
        // TODO
        //  - **CRP20266**
        //  - En el caso de que sólo existan conceptos con TipoFactorDR Exento, en este nodo solo deben existir los atributos BaseP, ImpuestoP y TipoFactorP.
        //  - Deben existir los campos BaseP, ImpuestoP y TipoFactorP
    }

    public void Visit(decimal monto, int numPago)
    {
        _pagoValidator.ValidateMonto(monto: monto, numPago: numPago, context: Context);
    }
    
    public bool HasErrors()
    {
        return Context.HasErrors();
    }

    public List<Error> GetErrors()
    {
        return Context.GetErrors();
    }

    public List<Warning> GetWarnings()
    {
        return Context.GetWarnings();
    }

    public (List<Warning>, List<Error>) GetValidationResult()
    {
        return (Context.GetWarnings(), Context.GetErrors());
    }
    
}