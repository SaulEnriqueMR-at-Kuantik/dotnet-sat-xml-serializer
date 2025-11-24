using KPac.Application.Formatter;
using KPac.Application.Validator.Catalogos;
using KpacModels.Shared.Models.Comprobante.Complementos.Nomina;

namespace KpacModels.Shared.XmlProcessing.Formatter.Nomina;

public class IncapacidadesFormatter
{
    private readonly FormatContext _context;

    private readonly string _section = "Comprobante -> Complemento -> Nomina -> {0}. Incapacidad";

    public IncapacidadesFormatter(FormatContext context)
    {
        _context = context;
    }
    // El nodo Incapacidades debe existir, si la clave expresada en el atributo Nomina.Percepciones.Percepcion.TipoPercepcion es "014"
    
    // Si la clave expresada en el atributo Nomina.Percepciones.Percepcion.TipoPercepcion es "014" la suma de los campos ImporteMonetario debe ser igual a la suma de los valores ImporteGravado e ImporteExento de la percepción
    public void Format(Incapacidad incapacidad, int index)
    {
        var dias = decimal.Parse(incapacidad.Dias);
        
        if(dias < 1)
            incapacidad.Dias = "1";
        else
            incapacidad.Dias = dias.ToString("F0");

        if (!CatalogosNomina.c_TipoIncapacidad.Contains(incapacidad.Tipo))
        {
            _context.AddError(
                string.Format(_section, index), 
                "El valor del atributo Incapacidad.TipoIncapacidad debe ser una clave del catálogo de c_TIpoIncapacidad");
            return;
        }
        
        if(string.IsNullOrEmpty(incapacidad.Importe))
            return;
        
        var importe = decimal.Parse(incapacidad.Importe);
        incapacidad.Importe = importe.ToString("F2");
        
        
    }
}