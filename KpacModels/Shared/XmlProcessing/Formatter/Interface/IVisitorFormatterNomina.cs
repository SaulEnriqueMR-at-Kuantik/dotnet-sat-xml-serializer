using KPac.Application.Formatter;
using KpacModels.Shared.Models.Comprobante;
using KpacModels.Shared.Models.Comprobante.Complementos.Nomina;
using KpacModels.Shared.Models.Core;
using Emisor = KpacModels.Shared.Models.Comprobante.Complementos.Nomina.Emisor;
using Receptor = KpacModels.Shared.Models.Comprobante.Complementos.Nomina.Receptor;

namespace KpacModels.Shared.XmlProcessing.Formatter.Interface;

public interface IVisitorFormatterNomina
{
    void Visit(Comprobante40 root, SettingsFormatter? configuracion);
    void Visit(Complemento? complemento);
    void Visit(Nomina12 nomina);
    void Visit(Emisor? emisor);
    Task Visit(Receptor? receptor);
    void Visit(Percepciones? percepciones);
    void Visit(Deducciones? deducciones);
    void Visit(List<OtroPago>? otrosPagos);
    void Visit(OtroPago otroPago, int index);
    void Visit(Incapacidad incapacidad,  int index);
    void Visit(List<Incapacidad>? incapacidades, List<Percepcion>? percepciones);
    void VisitTotales(Nomina12 nomina);
    void Visit(Comprobante40 root);
    List<Warning> Errors();
    
    bool HasErrors();
    
    void Clean();

    
}