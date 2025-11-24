using KPac.Application.Formatter;
using KPac.Application.Validator.Catalogos;
using KpacModels.Shared.Models.Comprobante.Complementos.Nomina;
using RegexCatalog = KpacModels.Shared.XmlProcessing.Validator.RegexCatalog;

namespace KpacModels.Shared.XmlProcessing.Formatter.Nomina;

public class EmisorNominaFormatter
{
    
    private readonly FormatContext _context;
    
    private readonly string _section = "Comprobante -> Complemento -> Nomina -> Emisor";

    public EmisorNominaFormatter(FormatContext context)
    {
        _context = context;
    }
    
    private Emisor _emisor =  new Emisor();
    public void Format(Emisor root)
    {
        _emisor = root;
        FormatCurp();
        FormatRegistroPatronal();
        FormatRfcPatronOrigen();
        FormatEntidadSncf();
    }

    private void FormatCurp()
    {
        var lengthRfcEmisor = int.Parse(_context.GetValue("lengthRfcEmisor") ?? "0");
        if (lengthRfcEmisor != 13)
        {
            _emisor.Curp = null;
            return;
        }

        if (_emisor.Curp == null)
        {
            _context.AddError(_section, "Si el atributo Comprobante.Emisor.Rfc tiene longitud 13, el atributo Nomina12:Emisor:Curp, debe existir.");
            return;
        }

        if (!RegexCatalog.IsCurpValid(_emisor.Curp))
        {
            _context.AddError(_section, "El atributo Curp no tiene formato valido.");
        }
    }

    private void FormatRegistroPatronal()
    {
        if (_emisor.RegistroPatronal != null)
        {
            _context.AddValue("hasRegistroPatronal", "true");
            var settings = _context.GetSettings();
            if (settings == null || settings.TipoAntiguedad == null)
            {
                _context.AddError(_section, "Cuando el atributo Nomina.Emisor.RegistroPatronal existe, se debera agregar a las configuraciones el tipo de Antiguedad que se generara. Semanas o YMD");
                return;
            }
            if (_emisor.RegistroPatronal.Length is < 1 or > 20)
            {
                _context.AddError(_section, "El atributo RegistroPatronal solo puede conformarse de 1 hasta 20 caracteres.");
            }
        }
    }

    private void FormatRfcPatronOrigen()
    {
        if (_emisor.RfcPatronOrigen != null)
        {
            _context.AddValue("hasRegistroPatronal", "true");
            if (!RegexCatalog.IsRfcValid(_emisor.RfcPatronOrigen))
            {
                _context.AddError(_section,
                    "El atributo RegistroPatronal solo puede conformarse de 1 hasta 20 caracteres.");
            }
        }
    }

    private void FormatEntidadSncf()
    {
        var entidadSncf = _emisor.EntidadSncf;
        if(entidadSncf == null)
            return;
        if (!CatalogosNomina.c_OrigenRecurso.Contains(entidadSncf.OrigenRecurso))
        {
            _context.AddError($"{_section} -> EntidadSNCF",
                "El atributo OrigenRecurso debe tener un valor del catálogo c_OrigenRecurso");
            return;
        }

        if (entidadSncf.OrigenRecurso != "IM")
        {
            entidadSncf.MontoRecursoPropio = null;
            return;
        }

        if (entidadSncf.MontoRecursoPropio == null)
        {
            _context.AddError($"{_section} -> EntidadSNCF",
                "Cuando el valor registrado en el atributo OrigenRecurso corresponde a la clave 'IM' (Ingresos Mixtos), el atributo MontoRecursoPropio debe existir.");
        }
        
        // TODO
        //  El valor de este dato debe ser menor que la suma de los campos TotalPercepciones y TotalOtrosPagos.
    }
    
    
    
    
    
}