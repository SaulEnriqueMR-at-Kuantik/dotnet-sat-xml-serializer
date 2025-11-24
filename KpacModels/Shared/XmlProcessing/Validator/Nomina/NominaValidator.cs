using KPac.Application.Validator;
using KPac.Application.Validator.Catalogos;
using KpacModels.Shared.Models.Comprobante.Complementos.Nomina;

namespace KpacModels.Shared.XmlProcessing.Validator.Nomina;

public class NominaValidator
{
    private readonly ValidatorContext _context;

    private Nomina12 _root;

    private const string Section = "Comprobante -> Complemento -> Nomina";
    
    private const string ErrorDecimales = "El atributo {0} solo puede tener 2 decimales.";

    public NominaValidator(ValidatorContext context)
    {
        _context = context;
    }
    public void Validate(Nomina12 nomina)
    {
        _root = nomina;
        ValidateVersion();
        ValidateTipoNomina();
        ValidateFechas();
        ValidateNumDiasPagados();
        ValidateTotalPercepciones();
        ValidateTotalDeducciones();
        ValidateTotalOtrosPagos();
    }

    private void ValidateVersion()
    {
        var version = _root.Version;
        if (version != "1.2")
        {
            _context.AddWarning(Section, "La version de Nomina debe ser '1.2'");
        }
    }

    private void ValidateTipoNomina()
    {
        var tipoNomina = _root.TipoNomina;
        if (!CatalogosNomina.c_TipoNomina.Contains(tipoNomina))
        {
            _context.AddError("NOM32", "El atributo TipoNomina debe ser una clave del catálogo c_TipoNomina.", Section);
        }
        _context.AddValue("tipoNomina", tipoNomina);
    }

    private void ValidateFechas()
    {
        const string errorIso = "El atributo {0} se debe expresar en la forma AAAA-MM-DD de acuerdo con la especificación ISO 8601";
        var fechaPagoString = _root.FechaPago;
        if (!RegexCatalog.IsIso8601ShortValid(fechaPagoString))
        {
            _context.AddWarning(Section, string.Format(errorIso, "FechaPago"));
            return;
        }

        var fechaInicialString = _root.FechaInicialPago;
        var fechaInicial = DateTime.Parse(fechaInicialString);
        if (!RegexCatalog.IsIso8601ShortValid(fechaInicialString))
        {
            _context.AddWarning(Section, string.Format(errorIso, "FechaInicialPago"));
            return;
        }
        
        var fechaFinalString = _root.FechaFinalPago;
        var fechaFinal = DateTime.Parse(fechaFinalString);
        if (!RegexCatalog.IsIso8601ShortValid(fechaFinalString))
        {
            _context.AddWarning(Section, string.Format(errorIso, "FechaFinalPago"));
            return;
        }

        if (fechaInicial > fechaFinal)
        {
            _context.AddError(
                "NOM35", 
                "El valor del atributo FechaInicialPago no es menor o igual al valor del atributo FechaFinalPago", 
                Section);
        }

    }

    private void ValidateNumDiasPagados()
    {
        var numDiasString = _root.NumeroDiasPagados;
        var numDias = decimal.Parse(numDiasString);
        if (numDias is < 0.001m or > 36160.00m)
        {
            _context.AddWarning(Section, $"El valor NumDiasPagados esta fuera de los limites permitidos. Min: 0.001 Max: 36160.00 Valor registrado: {numDiasString}");
        }
    }

    private void ValidateTotalPercepciones()
    {
        var percepciones = _root.Percepciones;
        var totalPercepcionesString = _root.TotalPercepciones;
        var totalPercepciones = decimal.Parse(totalPercepcionesString ?? "0");
        var countDecimales = ValidateHelper.CountDecimalPlaces(totalPercepciones);
        if (countDecimales > 2)
        {
            _context.AddWarning(Section, string.Format(ErrorDecimales, "TotalPercepciones"));
            return;
        }
        if (percepciones == null && !string.IsNullOrEmpty(totalPercepcionesString))
        {
            _context.AddError(
                "NOM36", 
                "Si el nodo Percepciones no existe, el atributo Nomina.TotalPercepciones no debe existir", 
                Section);
            return;
        }

        if (percepciones != null)
        {
            var totalSueldos = decimal.Parse(percepciones.TotalSueldos ?? "0");
            var totalSeparacion = decimal.Parse(percepciones.TotalSeparacionIndemnizacion ?? "0");
            var totalJubilacion = decimal.Parse(percepciones.TotalJubilacionPensionRetiro ?? "0");
            var totalesNodoPercepciones = totalSueldos + totalSeparacion + totalJubilacion;
            
            if (totalPercepciones != totalesNodoPercepciones)
            {
                _context.AddError(
                    "NOM37", 
                    "El valor del atributo TotalPercepciones no coincide con la suma de los atributos " +
                    "TotalSueldos más TotalSeparacionIndemnizacion más TotalJubilacionPensionRetiro del  nodo Percepciones.", 
                    Section);
                return;
            }
        }
    }

    private void ValidateTotalDeducciones()
    {
        var deducciones = _root.Deducciones;
        var totalDeduccionesString = _root.TotalDeducciones;
        var totalDeducciones = decimal.Parse(totalDeduccionesString ?? "0");
        var countDecimales = ValidateHelper.CountDecimalPlaces(totalDeducciones);
        if (countDecimales > 2)
        {
            _context.AddWarning(Section, string.Format(ErrorDecimales, "TotalDeducciones"));
            return;
        }
        if (deducciones == null && !string.IsNullOrEmpty(totalDeduccionesString))
        {
            _context.AddError("NOM38", "Si el nodo Deducciones no existe, el atributo de Nomina.TotalDeducciones no debe existir.", Section);
            return;
        }

        if (deducciones != null)
        {
            var totalOtras = decimal.Parse(deducciones.TotalOtrasDeducciones ?? "0");
            var totalRetenidos = decimal.Parse(deducciones.TotalImpuestosRetenidos ?? "0");
            var totalNodoDeducciones = totalOtras +  totalRetenidos;
            if (totalDeducciones != totalNodoDeducciones)
            {
                _context.AddError(
                    "NOM39", 
                    "El valor del atributo Nomina.TotalDeducciones no coincide con la suma de los atributos " +
                    "TotalOtrasDeducciones más TotalImpuestosRetenidos del elemento Deducciones", 
                    Section);
            }
        }
        
    }

    private void ValidateTotalOtrosPagos()
    {
        var otrosPagos = _root.OtrosPagos;
        var totalOtrosPagosString = _root.TotalOtrosPagos;
        var totalOtrosPagos = decimal.Parse(totalOtrosPagosString ?? "0");
        var countDecimales = ValidateHelper.CountDecimalPlaces(totalOtrosPagos);
        if (countDecimales > 2)
        {
            _context.AddWarning(Section, string.Format(ErrorDecimales, "TotalOtrosPagos"));
            return;
        }

        if (otrosPagos is { Count: > 0 } && string.IsNullOrEmpty(totalOtrosPagosString))
        {
            _context.AddError(
                "NOM40", 
                "Si el nodo OtrosPagos existe, el atributo Nomina.TotalOtrosPagos debe existir", 
                Section);
            return;
        }

        if (otrosPagos is { Count: > 0 })
        {
            var importe = otrosPagos.Sum(o => decimal.Parse(o.Importe));
            if (totalOtrosPagos != importe)
            {
                _context.AddError(
                    "NOM40", 
                    "El valor del atributo Nomina.TotalOtrosPagos no coincide con la suma de los atributos " +
                    "Importe de los nodos nomina12:OtrosPagos:OtroPago", 
                    Section);
            }
        }
        
    }
}