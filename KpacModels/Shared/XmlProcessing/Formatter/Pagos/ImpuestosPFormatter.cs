using System.Globalization;
using KPac.Application.Formatter;
using KPac.Application.Validator;
using KpacModels.Shared.Models.Comprobante.Complementos.Pagos;

namespace KpacModels.Shared.XmlProcessing.Formatter.Pagos;

public class ImpuestosPFormatter
{
    private List<RetencionP> _retenciones = [];

    private List<TrasladoP> _traslados = [];

    private FormatContext _context;
    
    public ImpuestosPFormatter(FormatContext context)
    {
        _context = context;
    }

    public void SaveImpuestosDr(ImpuestosDR? impuestosOriginal, string? equivalenciaString)
    {

        var impuestos = impuestosOriginal?.Clone() as ImpuestosDR;
        if(impuestos == null) return;

        var equivalencia = decimal.Parse(equivalenciaString ?? "1");
        
        
        var trasladosP = ConvertTrasladosDrToTrasladosP(impuestos.Traslados, equivalencia);
        if (trasladosP != null)
        {
            _traslados.AddRange(trasladosP);
        }
        
        var retencionesP = ConvertRetencionesDrToRetencionesP(impuestos.Retenciones, equivalencia);
        if (retencionesP != null)
        {
            _retenciones.AddRange(retencionesP);
        }

        
    }

    private static List<TrasladoP>? ConvertTrasladosDrToTrasladosP(List<TrasladoDR>? trasladosDr, decimal equivalencia)
    {
        if(trasladosDr == null || trasladosDr.Count == 0) return null;
        List<TrasladoP> trasladosP = [];
        foreach (var trasladoDr in trasladosDr)
        {
            var trasladoP = new TrasladoP()
            {
                Impuesto = trasladoDr.Impuesto,
                TipoFactor = trasladoDr.TipoFactor,
                TasaOCuota = trasladoDr.TasaOCuota,
                Importe = ConvertImporteConTipoCambio(trasladoDr.Importe, equivalencia),
                Base = (decimal.Parse(trasladoDr.Base) /  equivalencia).ToString(CultureInfo.InvariantCulture),
            };
            trasladosP.Add(trasladoP);
        }
        return trasladosP;
    }

    private static List<RetencionP>? ConvertRetencionesDrToRetencionesP(List<RetencionDR>? retencionesDr, decimal tipoCambio)
    {
        if (retencionesDr == null  || retencionesDr.Count == 0) return null;
        List<RetencionP> retencionesP = [];
        foreach (var retencionDr in retencionesDr)
        {
            var retencionP = new RetencionP()
            {
                Impuesto = retencionDr.Impuesto,
                Importe = (decimal.Parse(retencionDr.Importe) /  tipoCambio).ToString(CultureInfo.InvariantCulture),
            };
            retencionesP.Add(retencionP);
        }
        return retencionesP;
    }
    
    private static string? ConvertImporteConTipoCambio(string? importe, decimal equivalencia)
    {
        if (decimal.TryParse(importe, CultureInfo.InvariantCulture, out var importeDecimal))
        {
            var convertido = importeDecimal / equivalencia;
            return convertido.ToString(CultureInfo.InvariantCulture);
        }

        return null;
    }

    public List<RetencionP>? GetRetencionesP()
    {
        if(_retenciones.Count == 0) 
            return null;
        var decimales = int.Parse(_context.GetValue("decimalesMoneda") ?? "2");
        _retenciones = FormatHelper.ImpuestosSummaryRetenciones(_retenciones);
        var retencionesTruncadas = new List<RetencionP>();
        foreach (var retencion in _retenciones)
        {
            var newRetencion = new RetencionP()
            {
                Impuesto = retencion.Impuesto,
                Importe = DecimalOperatorLimites.TruncarDecimal(decimal.Parse(retencion.Importe), decimales)
                    .ToString(CultureInfo.InvariantCulture)
            };
            retencionesTruncadas.Add(newRetencion);
        }
        _retenciones.Clear();
        return retencionesTruncadas;
    }

    public List<TrasladoP>? GetTrasladosP()
    {
        if(_traslados.Count == 0) 
            return null;
        var decimales = int.Parse(_context.GetValue("decimalesMoneda") ?? "2");
        _traslados = FormatHelper.ImpuestosSummaryTraslados(_traslados);
        var trasladosTruncados = new  List<TrasladoP>();
        foreach (var traslado in _traslados)
        {
            var newTrasado = new TrasladoP
            {
                Impuesto = traslado.Impuesto,
                Importe = traslado.Importe !=  null ? DecimalOperatorLimites.TruncarDecimal(decimal.Parse(traslado.Importe), decimales).ToString(CultureInfo.InvariantCulture) : null,
                TipoFactor = traslado.TipoFactor,
                Base = DecimalOperatorLimites.TruncarDecimal(decimal.Parse(traslado.Base), decimales).ToString(CultureInfo.InvariantCulture),
                TasaOCuota = traslado.TasaOCuota,
            };
            trasladosTruncados.Add(newTrasado);
        }
        _traslados.Clear();
        return trasladosTruncados;
    }
}