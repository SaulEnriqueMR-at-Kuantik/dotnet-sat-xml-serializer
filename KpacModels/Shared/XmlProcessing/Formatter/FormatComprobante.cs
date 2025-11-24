using KPac.Application.Formatter;
using KpacModels.Shared.Models.Comprobante;
using KpacModels.Shared.Models.Core;
using KpacModels.Shared.XmlProcessing.Formatter.Interface;

namespace KpacModels.Shared.XmlProcessing.Formatter;

public class FormatComprobante
{
    private IVisitorFormatter  _visitor;
    
    private IVisitorFormatterPagos _visitorPagos;

    private IVisitorFormatterNomina _visitorNomina;
    
    public FormatComprobante(
        IVisitorFormatter visitor,
        IVisitorFormatterPagos visitorPagos,
        IVisitorFormatterNomina visitorNomina)
    {
       _visitor = visitor;
       _visitorPagos = visitorPagos;
       _visitorNomina = visitorNomina;
    }

    public async Task<List<Warning>> Format(
        Comprobante40 comprobante,
        SettingsFormatter? configuracion)
    {
        
        if (comprobante.IsPagos20())
        {
            await comprobante.Format(_visitorPagos);
            return _visitorPagos.Errors();
        }
        
        if (comprobante.IsNomina12())
        {
            await comprobante.Format(_visitorNomina, configuracion);
            return _visitorNomina.Errors();
        }
        
        await comprobante.Format(_visitor);
        return _visitor.Errors();
    }
    
}