using KpacModels.Shared.Models.Comprobante;
using KpacModels.Shared.Models.Comprobante.Complementos.Nomina;
using KpacModels.Shared.Models.Core;
using KpacModels.Shared.XmlProcessing.Validator.Interface;
using KpacModels.Shared.XmlProcessing.Validator.Nomina.Comprobante;
using Emisor = KpacModels.Shared.Models.Comprobante.Complementos.Nomina.Emisor;
using Receptor = KpacModels.Shared.Models.Comprobante.Complementos.Nomina.Receptor;

namespace KpacModels.Shared.XmlProcessing.Validator.Nomina;

public class VisitorNominaValidator : IVisitorNomina
{
    private readonly ComprobanteNominaValidator _comprobante;

    private readonly NominaValidator _nominaValidator;

    public VisitorNominaValidator(
        ComprobanteNominaValidator comprobante,
        NominaValidator nominaValidator)
    {
        _comprobante = comprobante;
        _nominaValidator = nominaValidator;
    }
    public void Visit(Comprobante40 comprobante)
    {
        _comprobante.Validate(comprobante);
    }

    public void Visit(Nomina12 nomina)
    {
        _nominaValidator.Validate(nomina);
    }

    public void Visit(Emisor? emisor)
    {
        throw new NotImplementedException();
    }

    public void Visit(Receptor receptor)
    {
        throw new NotImplementedException();
    }

    public void Visit(Percepciones? percepciones)
    {
        throw new NotImplementedException();
    }

    public void Visit(Incapacidad incapacidad)
    {
        throw new NotImplementedException();
    }

    public void Visit(Deducciones? deducciones)
    {
        throw new NotImplementedException();
    }

    public void Visit(List<OtroPago>? otrosPagos)
    {
        throw new NotImplementedException();
    }

    public void Visit(OtroPago otroPago, int index)
    {
        throw new NotImplementedException();
    }

    public void VisitTotales(Nomina12 nomina)
    {
        throw new NotImplementedException();
    }

    public bool HasErrors()
    {
        throw new NotImplementedException();
    }

    public (List<Warning>, List<Error>) GetValidationResult()
    {
        throw new NotImplementedException();
    }
}