using System.Globalization;
using KPac.Application.Formatter;
using KPac.Application.Validator.Catalogos;
using KpacModels.Shared.Models.Comprobante.Complementos.Nomina;

namespace KpacModels.Shared.XmlProcessing.Formatter.Nomina;

public class DeduccionesFormatter
{
    private static readonly string _section = "Comprobante -> Complemento -> Nomina -> Deducciones -> {0}. Deducción";

    private readonly FormatContext _context;

    public DeduccionesFormatter(FormatContext context)
    {
        _context = context;
    }
    public void Format(Deducciones deducciones)
    {
        var count = deducciones.Deduccion.Count;
        for (var i = 0; i < count; i++)
        {
            var deduccion = deducciones.Deduccion[i];
            FormatDeduccion(deduccion, i + 1);
        }

        var totalOtrasDeducciones = 0.00m;
        var totalImpuestosRetenidos = 0.00m;
        foreach (var deduccion in deducciones.Deduccion)
        {
            var importe = decimal.Parse(deduccion.Importe);
            var tipo = deduccion.Tipo;
            if (tipo == "002")
            {
                totalImpuestosRetenidos += importe;
                continue;
            }
            totalOtrasDeducciones += importe;
        }

        if (totalImpuestosRetenidos > 0)
            deducciones.TotalImpuestosRetenidos = totalImpuestosRetenidos.ToString(CultureInfo.InvariantCulture);
        
        if(totalOtrasDeducciones > 0)
            deducciones.TotalOtrasDeducciones = totalOtrasDeducciones.ToString(CultureInfo.InvariantCulture);

    }

    private void FormatDeduccion(Deduccion deduccion, int index)
    {
        if (!CatalogosNomina.c_TipoDeduccion.Contains(deduccion.Tipo))
        {
            _context.AddError(
                string.Format(_section, index), 
                "El valor de TipoDeduccion debe ser una clave del catálogo de c_TipoDeduccion");
            return;
        }

        var importe = deduccion.Importe;
        deduccion.Importe = FormatHelper.FormatStringToImporteSat(importe) ?? "0";
    }
}