using System.Globalization;
using KPac.Application.Formatter;
using KPac.Application.Validator.Catalogos;
using KpacModels.Shared.Models.Comprobante.Complementos.Nomina;
using RegexCatalog = KpacModels.Shared.XmlProcessing.Validator.RegexCatalog;

namespace KpacModels.Shared.XmlProcessing.Formatter.Nomina;

public class NominaFormatter
{
    
    private readonly FormatContext  _context;

    private readonly string section = "Comprobante -> Complemento -> Nomina";

    public NominaFormatter(FormatContext context)
    {
        _context = context;
    }
    public void Format(Nomina12 root)
    {
        FormatBaseAttributes(root);
    }

    private void FormatBaseAttributes(Nomina12 nomina)
    {
        nomina.Version = "1.2";

        if (!CatalogosNomina.c_TipoNomina.Contains(nomina.TipoNomina))
        {
            _context.AddError(section, "El valor del campo TipoNomina debe pertenecer al catalogo c_TipoNomina.");
            return;
        }

        if (nomina.TipoNomina == "E")
        {
            // Si el atributo Nomina.TipoNomina es extraordinaria (E) el tipo de periodicidad de pago debe ser la clave "99".
            _context.AddValue("periodicidad", "99");
        }

        if (!RegexCatalog.IsIso8601ShortValid(nomina.FechaPago))
        {
            _context.AddError(section, "El valor del campo FechaPago debe tener el formato de la especificación ISO 8601 - AAAA-MM-DD.");
            return;
        }
        
        if (!RegexCatalog.IsIso8601ShortValid(nomina.FechaInicialPago))
        {
            _context.AddError(section, "El valor del campo FechaInicialPago debe tener el formato de la especificación ISO 8601 - AAAA-MM-DD.");
            return;
        }
        
        if (!RegexCatalog.IsIso8601ShortValid(nomina.FechaFinalPago))
        {
            _context.AddError(section, "El valor del campo FechaFinalPago debe tener el formato de la especificación ISO 8601 - AAAA-MM-DD.");
            return;
        }
        var fechaInicial = DateTime.Parse(nomina.FechaInicialPago);
        var fechaFinal = DateTime.Parse(nomina.FechaFinalPago);
        if (fechaInicial > fechaFinal)
        {
            _context.AddError(section, "El valor del campo FechaInicialPago debe ser menor o igual al valor del atributo FechaFinalPago.");
            return;
        }

        if (!decimal.TryParse(nomina.NumeroDiasPagados, out var numeroDiasPagados))
        {
            _context.AddError(section, "El valor del campo NumDiasPagados no es un decimal, por lo que no es valido.");
            return;
        }

        if (numeroDiasPagados is < 0.001m or > 36160.00m)
        {
            _context.AddError(section, "Valor del campo NumDiasPagados fuera de rango. Valor minimo = 0.001m. Valor maximo = 36160.00m.");
            return;
        }
        _context.AddValue("fechaFinalPago", fechaFinal.ToString(CultureInfo.InvariantCulture));
        
        _context.AddValue("numDiasPagados", nomina.NumeroDiasPagados);
    }

    public void FormatTotales(Nomina12 nomina)
    {
        FormatTotalPercepciones(nomina);
        FormatTotalDeducciones(nomina);
        FormatTotalOtrosPagos(nomina);
    }
    
    private void FormatTotalPercepciones(Nomina12 nomina)
    {
        var percepciones = nomina.Percepciones;
        if (percepciones == null)
            return;
        var totalSueldos = decimal.Parse(percepciones.TotalSueldos ?? "0");
        var totalSeparacion = decimal.Parse(percepciones.TotalSeparacionIndemnizacion ?? "0");
        var totalJubilacion = decimal.Parse(percepciones.TotalJubilacionPensionRetiro ?? "0");
        var totalPercepciones = totalSueldos +  totalSeparacion + totalJubilacion;
        nomina.TotalPercepciones = totalPercepciones.ToString("F2");
        _context.AddValue("totalPercepciones", nomina.TotalPercepciones);
    }
    
    private void FormatTotalDeducciones(Nomina12 nomina)
    {
        var deducciones = nomina.Deducciones;
        if(deducciones == null)
            return;
        var totalOtrasDeducciones = decimal.Parse(deducciones.TotalOtrasDeducciones ?? "0");
        var totalImpuestosRetenidos = decimal.Parse(deducciones.TotalImpuestosRetenidos ?? "0");
        var totalDeducciones = totalOtrasDeducciones +  totalImpuestosRetenidos;
        nomina.TotalDeducciones = totalDeducciones.ToString("F2");
        _context.AddValue("totalDeducciones", nomina.TotalDeducciones);
    }

    private void FormatTotalOtrosPagos(Nomina12 nomina)
    {
        var otrosPagos = nomina.OtrosPagos;
        if (otrosPagos == null)
            return;
        var totalOtrosPagos = 0m;
        foreach (var otrosPago in otrosPagos)
        {
            var importe = decimal.Parse(otrosPago.Importe);
            totalOtrosPagos += importe;
        }
        nomina.TotalOtrosPagos = totalOtrosPagos.ToString("F2");
        _context.AddValue("totalOtrosPagos", nomina.TotalOtrosPagos);
    }
}