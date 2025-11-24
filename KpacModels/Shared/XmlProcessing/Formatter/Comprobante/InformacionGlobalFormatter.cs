using KPac.Application.Formatter;
using KpacModels.Shared.Models.Comprobante;

namespace KpacModels.Shared.XmlProcessing.Formatter.Comprobante;

public class InformacionGlobalFormatter
{
    private readonly FormatContext _context;

    public InformacionGlobalFormatter(FormatContext context)
    {
        _context = context;
    }
    public void Format(InformacionGlobal? informacionGlobal)
    {
        if(informacionGlobal?.SrcAnio != null)
            informacionGlobal.Anio = informacionGlobal.SrcAnio.ToString() ?? string.Empty;
    }
}