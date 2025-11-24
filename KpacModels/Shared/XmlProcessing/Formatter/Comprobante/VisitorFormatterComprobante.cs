using KPac.Application.Formatter;
using KpacModels.Shared.Models.Comprobante;
using KpacModels.Shared.Models.Core;
using KpacModels.Shared.XmlProcessing.Formatter.Comprobante.ConceptosFormat;
using KpacModels.Shared.XmlProcessing.Formatter.Comprobante.ImpuestosFormat;
using KpacModels.Shared.XmlProcessing.Formatter.Interface;

namespace KpacModels.Shared.XmlProcessing.Formatter.Comprobante;

public class VisitorFormatterComprobante  : IVisitorFormatter
{
    private readonly BaseFormatter _formatter;
    private readonly ConceptoFormatter _conceptoFormatter;
    private readonly ImpuestosFormatter _impuestosFormatter;
    private readonly ReceptorFormatter _receptorFormatter;
    private readonly InformacionGlobalFormatter _informacionGlobalFormatter;
    private readonly FormatContext _context;
    public VisitorFormatterComprobante(
        BaseFormatter baseFormatter, 
        ConceptoFormatter conceptoFormatter, 
        ImpuestosFormatter impuestosFormatter, 
        ReceptorFormatter receptorFormatter,
        InformacionGlobalFormatter  informacionGlobalFormatter,
        FormatContext  context)
    {
       _formatter = baseFormatter;
       _conceptoFormatter = conceptoFormatter;
       _impuestosFormatter = impuestosFormatter;
       _receptorFormatter = receptorFormatter;
       _informacionGlobalFormatter = informacionGlobalFormatter;
       _context = context;
    }

    public void SaveAttributeBase(Comprobante40 root)
    {
        if (root.TipoComprobante == null)
        {
            _context.AddError(
                section: "Comprobante",
                message: "El campo TipoDeComprobante es obligatorio.");
            return;
        }
        _context.AddValue("tipoComprobante", root.TipoComprobante);
        
        if (root.LugarExpedicion == null)
        {
            _context.AddError(
                section: "Comprobante",
                message: "El campo LugarExpedicion es obligatorio.");
            return; 
        }
        _context.AddValue("lugarExpedicion", root.LugarExpedicion);
        
        if(root.InformacionGlobal != null)
            _context.AddValue("hasInformacionGlobal", "true");
        
        
    }

    public void Visit(Comprobante40 root)
    {
        _formatter.Format(root);
    }

    public void Visit(InformacionGlobal? informacionGlobal)
    {
        _informacionGlobalFormatter.Format(informacionGlobal);
    }

    public void Visit(Emisor emisor)
    {
        // TODO Propuesta:
        //  Usar los usuarios que estan en la base de datos de mongo
    }

    public void Visit(CfdiRelacionado cfdiRelacionados, int noCfdi)
    {
        //throw new NotImplementedException();
    }

    public void Visit(Receptor receptor)
    {
        
        // TODO Propuesta:
        //  Usar los usuarios que estan en la base de datos de mongo
        
        _receptorFormatter.Format(receptor);
    }

    public void Visit(Concepto concepto, int noConcepto)
    {
       
        _conceptoFormatter.Format(concepto, noConcepto);
    }

    public void Visit(Impuestos? impuestos)
    {
        _impuestosFormatter.Format(impuestos);
    }

    public List<Warning> Errors()
    {
        var errors = _context.GetErrors();
        _context.Clear();
        return errors;
    }

    public void Clean()
    {
        _context.Clear();
    }
}