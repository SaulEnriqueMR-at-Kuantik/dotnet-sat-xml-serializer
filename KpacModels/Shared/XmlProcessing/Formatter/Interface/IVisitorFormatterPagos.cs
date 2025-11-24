using KpacModels.Shared.Models.Comprobante;
using KpacModels.Shared.Models.Comprobante.Complementos.Pagos;
using KpacModels.Shared.Models.Core;

namespace KpacModels.Shared.XmlProcessing.Formatter.Interface;

public interface IVisitorFormatterPagos
{
        void Visit(Comprobante40 root);
        void Visit(Complemento? complemento);
        void Visit(Pagos20? pagos);
        Task Visit(Pago pago, int noPago);
        Task Visit(DoctoRelacionado doctoRelacionado, int numPago, int numDocto);
        void Visit(RetencionDR retencion, int numPago, int numDocto, int numRetencion);
        void Visit(TrasladoDR traslado, int numPago, int numDocto, int numTraslado);
       
        void Visit(Totales totales, decimal montoTotal, List<RetencionP> retencionesTotales,
                List<TrasladoP> trasladosTotales);
        List<Warning> Errors();
        void SaveImpuestosDr(ImpuestosDR? impuestos, string? equivalencia);
        ImpuestosP? Visit(ImpuestosP? impuestos);
        
        void Visit(Pago pago, decimal monto);
        void Clean();
}