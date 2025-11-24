using KPac.Application.Formatter;
using KpacModels.Shared.Models.Comprobante.Complementos.Nomina;

namespace KpacModels.Shared.XmlProcessing.Formatter.Nomina;

public class OtrosPagosFormatter
{

    private static FormatContext _context;

    public OtrosPagosFormatter(FormatContext context)
    {
        _context = context;
    }
    //TipoOtroPago
    //- Si el valor del atributo Nomina.Receptor.TipoRegimen es "02" debe existir el campo TipoOtroPago con la clave "002", siempre que, no se haya registrado otro elemento OtroPago con el valor "007" o "008" en el atributo TipoOtroPago. 
    //- Si en el atributo Nomina.Receptor.TipoRegimen existe una clave distinta a "02", el atributo TipoOtroPago no deberá contener la clave "002", "007" o "008".

    private static string _section = "Comprobante -> Complemento -> Nomina -> {0}. OtroPago";
    public void Format(OtroPago otroPago, int index)
    {
        var tipo = otroPago.Tipo;
        var importe = decimal.Parse(otroPago.Importe);
        if (tipo != "002" && importe <= 0)
        {
            _context.AddError(
                string.Format(_section, index), 
                "Si el valor del atributo TipoOtroPago es diferente a '002', el atributo Importe debe ser mayor que cero." );
            return;
        }

        if (tipo is "002" && otroPago.SubsidioAlEmpleo is null)
        {
            _context.AddError(
                string.Format(_section, index), 
                "Si el valor del atributo TipoOtroPago es '002' es obligatorio el nodo SubsidioAlEmpleo." );
            return;
        }

        if (tipo is "004" && otroPago.CompensacionSaldosAFavor is null)
        {
            _context.AddError(
                string.Format(_section, index), 
                "Si el valor del atributo TipoOtroPago es '004' es obligatorio el nodo CompensacionSaldosAFavor." );
            return;
        }
        
        // Importe solo debe tener 2 decimales.
        otroPago.Importe = FormatHelper.FormatDecimalToImporteSat(importe) ?? "0";

        FormatSubsidioAlEmpleo(otroPago.SubsidioAlEmpleo);
        
        FormatCompensacionSaldos(otroPago.CompensacionSaldosAFavor);
    }

    private void FormatCompensacionSaldos(CompensacionSaldosAFavor? compensacionSaldosAFavor)
    {
        if(compensacionSaldosAFavor is null)
            return;
        var saldo = decimal.Parse(compensacionSaldosAFavor.SaldoAFavor);
        var remanente = decimal.Parse(compensacionSaldosAFavor.RemanenteSaldoAFavor);
        

        if (saldo < remanente)
        {
            compensacionSaldosAFavor.SaldoAFavor = FormatHelper.FormatDecimalToImporteSat(remanente) ?? "0";
        }
        compensacionSaldosAFavor.SaldoAFavor = FormatHelper.FormatDecimalToImporteSat(saldo) ?? "0";

        if (int.TryParse(compensacionSaldosAFavor.Anio, out int anio))
        {
            if (anio < 2016)
            {
                compensacionSaldosAFavor.Anio = "2016";
            }
        }

    }

    private void FormatSubsidioAlEmpleo(SubsidioAlEmpleo? subsidioAlEmpleo)
    {
        if (subsidioAlEmpleo is null)
            return;
        var subsidio = decimal.Parse(subsidioAlEmpleo.SubsidioCausado);

        var numDiasPagados = decimal.Parse(_context.GetValue("numDiasPagados") ?? "0");
        if (numDiasPagados <= 31)
        {
            if (subsidio > 407.02m)
            {
                subsidioAlEmpleo.SubsidioCausado = "407.02";
            }
        }

        if (numDiasPagados > 31)
        {
            var factor = numDiasPagados * 13.39m;
            if(subsidio > factor)
                subsidioAlEmpleo.SubsidioCausado = factor.ToString("F2");
        }
        
        subsidioAlEmpleo.SubsidioCausado = FormatHelper.FormatDecimalToImporteSat(subsidio) ?? "0";
    }

    public void Format(List<OtroPago>? otrosPagos)
    {
        var tipoRegimen = _context.GetValue("tipoRegimen") ?? string.Empty;
        var listTiposOtros = otrosPagos?.Select(o => o.Tipo).ToList() ?? [];
        if (tipoRegimen == "02")
        {
            if(listTiposOtros.Contains("002"))
                return;
            if (listTiposOtros.Contains("007") || listTiposOtros.Contains("008"))
                return;
        }

        if (tipoRegimen != "02")
        {
            List<OtroPago> newList = [];
            foreach (var otroPago in otrosPagos ?? [])
            {
                if(otroPago.Tipo != "002" && otroPago.Tipo != "007" && otroPago.Tipo != "008")
                    newList.Add(otroPago);
            }
            otrosPagos?.Clear();
            otrosPagos?.AddRange(newList);
        }
    }
    
}