using KPac.Application.Formatter;
using KPac.Application.Formatter.Nomina;
using KpacModels.Shared.Models.Comprobante;
using KpacModels.Shared.Models.Comprobante.Complementos.Nomina;
using KpacModels.Shared.Models.Core;
using KpacModels.Shared.XmlProcessing.Formatter.Interface;
using KpacModels.Shared.XmlProcessing.Formatter.Nomina.ComprobanteFormatter;
using Emisor = KpacModels.Shared.Models.Comprobante.Complementos.Nomina.Emisor;
using Receptor = KpacModels.Shared.Models.Comprobante.Complementos.Nomina.Receptor;

namespace KpacModels.Shared.XmlProcessing.Formatter.Nomina;

public class VisitorFormatterNomina : IVisitorFormatterNomina
{

    private readonly ComprobanteFormatterNomina _comprobanteFormatterNomina;
    
    private readonly NominaFormatter _nominaBaseFormatter;
    
    private readonly EmisorNominaFormatter _emisorFormatter;

    private readonly ReceptorNominaFormatter _receptorNominaFormatter;

    private readonly DeduccionesFormatter _deduccionesFormatter;

    private readonly IncapacidadesFormatter _incapacidadesFormatter;

    private readonly OtrosPagosFormatter _otrosPagosFormatter;

    private readonly PercepcionesFormatter _percepcionesFormatter;
    
    private readonly FormatContext _context;
    
    public VisitorFormatterNomina(
        ComprobanteFormatterNomina  comprobanteFormatterNomina, 
        FormatContext context,
        NominaFormatter nominaBaseFormatter,
        EmisorNominaFormatter emisorFormatter,
        DeduccionesFormatter  deduccionesFormatter,
        IncapacidadesFormatter  incapacidadesFormatter,
        OtrosPagosFormatter otrosPagosFormatter,
        PercepcionesFormatter  percepcionesFormatter,
        ReceptorNominaFormatter receptorNominaFormatter)
    {
        _comprobanteFormatterNomina = comprobanteFormatterNomina;
        _nominaBaseFormatter = nominaBaseFormatter;
        _emisorFormatter = emisorFormatter;
        _receptorNominaFormatter = receptorNominaFormatter;
        _deduccionesFormatter = deduccionesFormatter;
        _incapacidadesFormatter = incapacidadesFormatter;
        _otrosPagosFormatter = otrosPagosFormatter;
        _percepcionesFormatter = percepcionesFormatter;
        _context = context;
    }
    
    public void Visit(Comprobante40 root, SettingsFormatter? configuracion)
    {
        if(_context.HasErrors())
            return;
        _comprobanteFormatterNomina.Format(root, configuracion);
    }

    public void Visit(Complemento? complemento)
    {
        if(_context.HasErrors())
            return;
        if (complemento == null)
        {
            _context.AddError("Comprobante", "Es requerido que exista el nodo Complemento");
            return;
        }

        if (complemento.Nomina == null)
        {
            _context.AddError("Comprobante -> Complemento", "Es requerido que exista el nodo Nomina");
        }
    }

    public void Visit(Nomina12 nomina)
    {
        if(_context.HasErrors())
            return;
        _nominaBaseFormatter.Format(nomina);
    }
    
    public void VisitTotales(Nomina12 nomina)
    {
        if(_context.HasErrors())
            return;
        _nominaBaseFormatter.FormatTotales(nomina);
    }

    public void Visit(Comprobante40 root)
    {
        if(_context.HasErrors())
            return;
        _comprobanteFormatterNomina.Format(root);
    }

    public void Visit(Emisor emisor)
    {
        if(_context.HasErrors())
            return;
        _emisorFormatter.Format(emisor);
    }

    public async Task Visit(Receptor? receptor)
    {
        if(_context.HasErrors())
            return;
        if (receptor == null)
        {
            _context.AddError("Comprobante -> Complemento -> Nomina", "Es requerido que exista el nodo Receptor");
            return;
        }
        // TODO
        //await _receptorNominaFormatter.Format(receptor);
    }

    public void Visit(Percepciones? percepciones)
    {
        if(_context.HasErrors())
            return;
        if(percepciones == null)
            return;
        _percepcionesFormatter.Format(percepciones);
    }

    public void Visit(Deducciones? deducciones)
    {
        if(_context.HasErrors())
            return;
        if(deducciones == null)
            return;
        _deduccionesFormatter.Format(deducciones);
    }

    public void Visit(List<OtroPago>? otrosPagos)
    {
        if(_context.HasErrors())
            return;
        _otrosPagosFormatter.Format(otrosPagos);
    }

    public void Visit(OtroPago otroPago, int index)
    {
        if(_context.HasErrors())
            return;
        _otrosPagosFormatter.Format(otroPago, index);
    }
    
    public void Visit(List<Incapacidad>? incapacidades, List<Percepcion>? percepciones)
    {
        if(_context.HasErrors())
            return;
        var section = "Comprobante -> Complemento -> Nomina -> Incapacidades";
        if (_context.ExistPercepcion("014"))
        {
            if (incapacidades is null || incapacidades.Count == 0)
            {
                _context.AddError(
                    section,
                    "El nodo Incapacidades es obligatorio, si existe una Percepcion con TipoPercepcion con valor '014'");
            }

            var totalIncapacidades = incapacidades?.Sum(i => decimal.Parse(i.Importe ?? "0"));
            var percepcion = percepciones?.FirstOrDefault(p => p.Tipo == "014");
            if (totalIncapacidades != null && percepcion != null)
            {
                var totalPercepcion =
                    decimal.Parse(percepcion.ImporteExento) + decimal.Parse(percepcion.ImporteGravado);
                if (totalPercepcion != totalIncapacidades)
                {
                    _context.AddError(
                        section, 
                        $"Si la clave expresada en el atributo Nomina.Percepciones.Percepcion.TipoPercepcion es '014' la suma de los campos ImporteMonetario debe ser igual a la suma de los valores ImporteGravado e ImporteExento de la percepción. Suma incapacidades = {totalIncapacidades}. Suma Percepcion = {totalPercepcion}");
                }
            }
        }
    }

    public void Visit(Incapacidad incapacidad, int index)
    {
        if(_context.HasErrors())
            return;
        _incapacidadesFormatter.Format(incapacidad, index);
    }

    public List<Warning> Errors()
    {
        var errors = _context.GetErrors();
        return errors;
    }

    public bool HasErrors()
    {
        return _context.HasErrors();
    }

    public void Clean()
    {
        _context.Clear();
    }
}