using KpacModels.Shared.Models.Comprobante;
using KpacModels.Shared.Models.Comprobante.Complementos.Nomina;
using KpacModels.Shared.Models.Core;
using Emisor = KpacModels.Shared.Models.Comprobante.Complementos.Nomina.Emisor;
using Receptor = KpacModels.Shared.Models.Comprobante.Complementos.Nomina.Receptor;

namespace KpacModels.Shared.XmlProcessing.Validator.Interface;

public interface IVisitorNomina
{
    void Visit(Comprobante40 comprobante);
    void Visit(Nomina12 nomina);
    void Visit(Emisor? emisor);
    void Visit(Receptor receptor);
    void Visit(Percepciones? percepciones);
    void Visit(Incapacidad incapacidad);
    void Visit(Deducciones? deducciones);
    void Visit(List<OtroPago>? otrosPagos);
    void Visit(OtroPago otroPago, int index);
    void VisitTotales(Nomina12 nomina);
    bool HasErrors();
    (List<Warning>, List<Error>) GetValidationResult();
}