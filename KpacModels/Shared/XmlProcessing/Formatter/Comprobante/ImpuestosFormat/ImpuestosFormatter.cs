using KPac.Application.Formatter;
using KpacModels.Shared.Models.Comprobante;

namespace KpacModels.Shared.XmlProcessing.Formatter.Comprobante.ImpuestosFormat;

public class ImpuestosFormatter
{
    private FormatContext _context;
    private string _tipoComprobante;

    public ImpuestosFormatter(FormatContext context)
    {
        _context = context;
    }
    public void Format(Impuestos? impuestos)
    {
        _tipoComprobante = _context.GetValue("tipoComprobante") ?? string.Empty;
        if (_tipoComprobante is "T" or "P" or "N" && impuestos != null)
        {
            impuestos.Clear();
            return;
        }
        
        if(impuestos is null)
            impuestos = new Impuestos();

        var trasladosConcepto = _context.GetImpuestosTraslados();

        var traslados = FormatHelper.ImpuestosSummaryTraslados(trasladosConcepto);
        
        if (traslados.Count > 0)
        {
            if (impuestos.Traslados == null)
                impuestos.Traslados = [];
            var totalImpuestosTraslados = GetTotalImpuestosTraslados(traslados);
            impuestos.TotalImpuestosTrasladados = totalImpuestosTraslados;
            impuestos.Traslados = traslados;
        }
        else
        {
            impuestos.Traslados?.Clear();
            impuestos.TotalImpuestosTrasladados = null;
        }

        var retencionesConcepto = _context.GetImpuestosRetenciones();
        var retenciones = FormatHelper.ImpuestosSummaryRetenciones(retencionesConcepto);
        if (retenciones.Count > 0)
        {
            if(impuestos.Retenciones == null)
                impuestos.Retenciones = [];
            var totalImpuestosRetenciones = GetTotalImpuestosRetenciones(retenciones);
            impuestos.TotalImpuestosRetenidos = totalImpuestosRetenciones;
            impuestos.Retenciones = retenciones;
        }
        else
        {
            impuestos.Retenciones?.Clear();
            impuestos.TotalImpuestosRetenidos = null;
        }
    }

    private string? GetTotalImpuestosTraslados(List<ImpuestoT> traslados)
    {
        var soloExentos = true;
        var totalImpuestosTraslados = 0.0m;
        foreach (var traslado in traslados)
        {
            if (traslado.TipoFactor != "Exento")
            {
                soloExentos = false;
            }
            var importe = decimal.Parse(traslado.Importe ?? "0");
            totalImpuestosTraslados += importe;
        }

        if (soloExentos == false)
        {
            _context.AddValue("totalImpuestosTraslados", totalImpuestosTraslados.ToString("F2"));
            return totalImpuestosTraslados.ToString("F2");
        }
        return null;
    }
    
    private string? GetTotalImpuestosRetenciones(List<ImpuestoR> retenciones)
    {
        var totalImpuestosRetenciones = 0.0m;
        foreach (var retencion in retenciones)
        {
            var importe = decimal.Parse(retencion.Importe ?? "0");
            totalImpuestosRetenciones += importe;
        }
        _context.AddValue("totalImpuestosRetenciones", totalImpuestosRetenciones.ToString("F2"));
        return totalImpuestosRetenciones.ToString("F2");
    }
}