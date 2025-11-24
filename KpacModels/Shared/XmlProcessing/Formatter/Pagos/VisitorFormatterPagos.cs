using System.Globalization;
using KPac.Application.Formatter;
using KpacModels.Shared.Models.Comprobante;
using KpacModels.Shared.Models.Comprobante.Complementos.Pagos;
using KpacModels.Shared.Models.Core;
using KpacModels.Shared.XmlProcessing.Formatter.Interface;
using KpacModels.Shared.XmlProcessing.Formatter.Pagos.ComprobanteFormatter;

namespace KpacModels.Shared.XmlProcessing.Formatter.Pagos;

public class VisitorFormatterPagos : IVisitorFormatterPagos
{
    private readonly ComprobanteFormatterPagos _comprobanteFormatterPagos;

    private readonly PagoFormatter _pagoFormatter;
    
    private readonly DoctoRelacionadoFormatter _doctoRelacionadoFormatter;

    private readonly FormatContext _context;
    
    private readonly ImpuestosDrFormatter _impuestosDrFormatter;
    
    private readonly ImpuestosPFormatter  _impuestosPFormatter;
    
    private readonly TotalesFormatter _totalesFormatter;
    
    public VisitorFormatterPagos(
        FormatContext context,  
        ComprobanteFormatterPagos comprobanteFormatterPagos, 
        PagoFormatter pagoFormatter,
        DoctoRelacionadoFormatter doctoRelacionadoFormatter,
        ImpuestosDrFormatter impuestosDrFormatter,
        ImpuestosPFormatter impuestosPFormatter,
        TotalesFormatter totalesFormatter)
    {
        _context  = context;
        _comprobanteFormatterPagos = comprobanteFormatterPagos;
        _pagoFormatter   = pagoFormatter;
        _doctoRelacionadoFormatter = doctoRelacionadoFormatter;
        _impuestosDrFormatter = impuestosDrFormatter;
        _impuestosPFormatter = impuestosPFormatter;
        _totalesFormatter = totalesFormatter;
    }
    
    public void Visit(Comprobante40 root)
    {
        _comprobanteFormatterPagos.Format(root);
    }

    public void Visit(Complemento? complemento)
    {
        if (complemento != null)
        {
            complemento.CartaPorte = null;
            complemento.ComercioExterior = null;
            complemento.ImpuestosLocales = null;
            complemento.LeyendasFiscales = null;
            complemento.Nomina = null;
            complemento.TimbreFiscalDigital =  null;
        }
        else
        {
            _context.AddError("Comprobante -> Complemento",
                "Debe existir el nodo Complemento en el Comprobante");
            return;
        }

        if (complemento.Pagos == null || complemento.Pagos.Count() == 0)
        {
            _context.AddError("Comprobante -> Complemento -> Pagos",
                "El Complemento debe contener un nodo de Pagos registrado.");
        }

    }

    public void Visit(Pagos20 pagos)
    {
        pagos.Version = "2.0";
    }

    public async Task Visit(Pago pago, int noPago)
    {
        await _pagoFormatter.Format(pago,  noPago);
    }

    public async Task Visit(DoctoRelacionado doctoRelacionado, int noPago, int noDocto)
    {
        await _doctoRelacionadoFormatter.Format(doctoRelacionado, noPago, noDocto);
    }

    public void Visit(RetencionDR retencion, int numPago, int numDocto, int numRetencion)
    {
        _impuestosDrFormatter.Format(retencion, numPago, numDocto, numRetencion);
    }

    public void Visit(TrasladoDR traslado, int numPago, int numDocto, int numTraslado)
    {
        _impuestosDrFormatter.Format(traslado, numPago, numDocto, numTraslado);
    }

    public void Visit(Totales totales, decimal montoTotal, List<RetencionP> retencionesTotales,
        List<TrasladoP> trasladosTotales)
    {
        _totalesFormatter.Format(totales, montoTotal, retencionesTotales, trasladosTotales);
    }

    public List<Warning> Errors()
    {
        var errors = _context.GetErrors();
        _context.Clear();
        return errors;
    }

    public void SaveImpuestosDr(ImpuestosDR? impuestos, string? equivalencia)
    {
        _impuestosPFormatter.SaveImpuestosDr(impuestos, equivalencia);
    }

    public ImpuestosP? Visit(ImpuestosP? impuestos)
    {
        var retenciones= _impuestosPFormatter.GetRetencionesP();
        var traslados = _impuestosPFormatter.GetTrasladosP();
        if (retenciones != null || traslados != null)
        {
            if(impuestos ==  null)
                impuestos = new ImpuestosP();
            impuestos.Retenciones = retenciones;
            impuestos.Traslados = traslados;
        }
        
        return impuestos;
    }

    public void Visit(Pago pago, decimal monto)
    {
        var decimalesMonedaString = _context.GetValue("decimalesMoneda");
        var decimalesMoneda = int.Parse(decimalesMonedaString ?? "2");
        pago.Monto = Math.Round(monto, decimalesMoneda).ToString(CultureInfo.InvariantCulture);
    }

    public void Clean()
    {
        _context.Clear();
    }
}