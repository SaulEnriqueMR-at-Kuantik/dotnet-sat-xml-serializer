using KpacModels.Shared.Models.Comprobante.Complementos.Pagos;
using KpacModels.Shared.Models.Core;

namespace KpacModels.Shared.XmlProcessing.Validator.Interface;

public interface IVisitorPagos
{
    void Visit(Pagos20 root);
    void Visit(Totales totales, List<RetencionP> retencionesTotales, List<TrasladoP> trasladosTotales);
    Task Visit(Pago pago, int numPago);
    Task Visit(DoctoRelacionado doctoRelacionado, int numPago, int numDocto);
    void Visit(ImpuestosDR impuestosDr, int numPago, int numDocto);
    void Visit(RetencionDR retencion, int numPago, int numDocto, int noRetencion);
    void Visit(TrasladoDR traslado, int numPago, int numDocto, int noTraslado);
    void Visit(ImpuestosP impuestos, int noPago);
    void Visit(decimal monto, int numPago);
    bool HasErrors();
    List<Error> GetErrors();
    List<Warning> GetWarnings();
    (List<Warning>, List<Error>) GetValidationResult();
}