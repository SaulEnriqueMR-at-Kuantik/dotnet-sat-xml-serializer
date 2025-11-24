using KpacModels.Shared.Models.Comprobante;
using KpacModels.Shared.Models.Core;
using KpacModels.Shared.XmlProcessing.Validator.Interface;

namespace KpacModels.Shared.XmlProcessing.Validator;

public class ValidatorService : IValidatorService
{
    private readonly IVisitor _visitorComprobante;
    
    private readonly IVisitorPagos  _visitorPagos;

    private readonly IVisitorNomina _visitorNomina;
    
    public ValidatorService(IVisitor visitorComprobante, IVisitorPagos  visitorPagos, IVisitorNomina visitorNomina)
    {
        _visitorComprobante = visitorComprobante;
        _visitorPagos = visitorPagos;
        _visitorNomina = visitorNomina;
    }
    public async Task<(List<Warning>, List<Error>)> Validate(Comprobante40 comprobante)
    {
        var tipoComprobante = comprobante.TipoComprobante;
        var complemento = comprobante.Complemento;
        if(tipoComprobante == "P"){
            if (complemento?.Pagos != null && complemento.Pagos.Count != 0)
            {
                var pagos = complemento.Pagos.FirstOrDefault();
                if(pagos != null)
                    await pagos.Accept(_visitorPagos);
            }
            if(_visitorPagos.HasErrors())
                return (_visitorPagos.GetWarnings(),  _visitorPagos.GetErrors());
        }

        if (tipoComprobante == "N")
        {
            await comprobante.Accept(_visitorNomina);
            if(_visitorNomina.HasErrors())
                return _visitorNomina.GetValidationResult();
        }

        await comprobante.Accept(_visitorComprobante);
        if(_visitorComprobante.HasErrors())
            return _visitorComprobante.GetValidationResult();
        return ([], []);
    }
}