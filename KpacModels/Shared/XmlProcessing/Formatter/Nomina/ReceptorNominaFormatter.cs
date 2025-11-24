

using KpacModels.Shared.XmlProcessing.Formatter;

namespace KPac.Application.Formatter.Nomina;

public class ReceptorNominaFormatter
{
    private readonly FormatContext _context;

    //private readonly RepositoryValidator<cfdi40_estado> _repositoryEstados;

    private const string Section = "Comprobante -> Complemento -> Nomina -> Receptor";
    // public ReceptorNominaFormatter(FormatContext context, ClientValidator clientValidator)
    // {
    //     _context = context;
    //     _repositoryEstados = new RepositoryValidator<cfdi40_estado>(clientValidator);
    // }
    // private Receptor _receptor =  new Receptor();

    // private bool _hasRegistroPatronal;
    // public async Task Format(Receptor root)
    // {
    //     _hasRegistroPatronal = _context.GetValue("hasRegistroPatronal") is "true";
    //     _receptor = root;
    //     FormatCurp();
    //     FormatNumSeguridadSocial();
    //     FormatFechaIncioRelacionLaboral();
    //     FomatTipoContrato();
    //     FormatSindicalizado();
    //     FormatTipoJornada();
    //     FormatTipoRegimen();
    //     FormatRiesgoPuesto();
    //     FormatPeriodicidadPago();
    //     FormatBanco_CuentaBancaria();
    //     FormatSalarioBaseCotizacion();
    //     FormatSalarioDiarioIntegrado();
    //     await FormatClaveEntidadFederal();
    //     FormatSubcontratacion();
    //
    // }

    

    // private void FormatCurp()
    // {
    //     if (!RegexCatalog.IsCurpValid(_receptor.Curp))
    //     {
    //         _context.AddError(Section, "El atributo Curp no cumple con el patrón t_CURP");
    //     }
    // }
    //
    // private void FormatNumSeguridadSocial()
    // {
    //     if (_receptor.NoSeguridadSocial == null && _hasRegistroPatronal)
    //     {
    //         _context.AddError(Section, "Si el atributo Nomina.Emisor.RegistroPatronal existe, debe existir el atributo nomina12:Receptor:NumSeguridadSocial");
    //         return;
    //     }
    //     if(_receptor.NoSeguridadSocial == null && !_hasRegistroPatronal)
    //         return;
    //     if(_receptor.NoSeguridadSocial?.Length is < 1 or > 15)
    //         _context.AddError(Section, "El atributo NumSeguridadSocial debe contener entre 1 a 15 caracteres");
    // }
    //
    // private void FormatFechaIncioRelacionLaboral()
    // {
    //     Logs.Info("Entra a FormatFechaIncioRelacionLaboral");
    //     if (_receptor.FechaInicioRelacionLaboral == null && _hasRegistroPatronal)
    //     {
    //         _context.AddError(Section, "Si el atributo Nomina.Emisor.RegistroPatronal existe, debe existir el atributo nomina12:Receptor:FechaInicioRelLaboral");
    //         return;
    //     }
    //     if(_receptor.FechaInicioRelacionLaboral == null && !_hasRegistroPatronal)
    //         return;
    //     if (!DateTime.TryParseExact(
    //             _receptor.FechaInicioRelacionLaboral, 
    //             DateIsoFormats.ISO_8601_SHORT,
    //             CultureInfo.InvariantCulture, 
    //             DateTimeStyles.None, 
    //             out var fechaInicioRelacionLaboral))
    //     {
    //         _context.AddError(Section, "El atributo FechaInicioRelLaboral debe expresarse en la forma AAAA-MM-DD, de acuerdo con la especificación ISO 8601");
    //         return;
    //     }
    //     var fechaFinalPagoString = _context.GetValue("fechaFinalPago");
    //     var fechaFinalPago =  Convert.ToDateTime(fechaFinalPagoString);
    //
    //     if (fechaInicioRelacionLaboral > fechaFinalPago)
    //     {
    //         _context.AddError(Section, "El atributo FechaInicioRelLaboral debe ser menor o igual al atributo FechaFinalPago");
    //         return;
    //     }
    //     Logs.Info("LLega aca");
    //     FormatAntiguedad(fechaInicioRelacionLaboral, fechaFinalPago);
    // }
    //
    // private void FormatAntiguedad(DateTime fechaInicio, DateTime fechaFinal)
    // {
    //     if (!_hasRegistroPatronal)
    //     {
    //         return;
    //     }
    //
    //     var settings = _context.GetSettings();
    //     if (settings?.TipoAntiguedad is EnumFormatAntiguedad.Semanas)
    //     {
    //         _receptor.Antiguedad = AntiguedadHelper.CalcularSemanas(fechaInicio, fechaFinal);
    //         return;
    //     }
    //     
    //     if (settings?.TipoAntiguedad is EnumFormatAntiguedad.Ymd)
    //     {
    //         _receptor.Antiguedad = AntiguedadHelper.CalcularYMD(fechaInicio, fechaFinal);
    //         return;
    //     }
    // }
    //
    // private void FomatTipoContrato()
    // {
    //     if (!CatalogosNomina.c_TipoContrato.Contains(_receptor.TipoContrato))
    //     {
    //         _context.AddError(Section, "El valor del atributo TipoContrato debe pertenecer al catalogo c_TipoContrato");
    //     }
    // }
    //
    // private void FormatSindicalizado()
    // {
    //     var sindicalizado = CleanText.Normalize(_receptor.Sindicalizado);
    //     if (sindicalizado is "si")
    //     {
    //         _receptor.Sindicalizado = "Sí";
    //         return;
    //     }
    //     if (sindicalizado is "no")
    //     {
    //         _receptor.Sindicalizado = "No";
    //         return;
    //     }
    //
    // }
    //
    // private void FormatTipoJornada()
    // {
    //     if(_receptor.TipoJornada is null)
    //         return;
    //     if (!CatalogosNomina.c_TipoJornada.Contains(_receptor.TipoJornada))
    //     {
    //         _context.AddError(Section, "El valor del atributo TipoJornada debe pertenecer al catalogo c_TipoJornada");
    //     }
    // }
    //
    // private void FormatTipoRegimen()
    // {
    //     if (!CatalogosNomina.c_TipoRegimen.Contains(_receptor.TipoRegimen))
    //     {
    //         _context.AddError(Section, "El valor del atributo TipoRegimen debe pertenecer al catalogo c_TipoRegimen");
    //         return;
    //     }
    //
    //     List<string> tipoRegimenValido = ["02", "03", "04"];
    //     // Cuando TipoContrato tiene una clave entre los valores 01 - 08 entonces TipoRegimen debe contener 
    //     // alguna de las claves 02, 03 o 04.
    //     if (CatalogosNomina.c_TipoContrato_01_08.Contains(_receptor.TipoContrato))
    //     {
    //         if(!tipoRegimenValido.Contains(_receptor.TipoRegimen))
    //             _context.AddError(
    //                 Section, 
    //                 "Cuando TipoContrato tiene una clave entre los valores 01 - 08 entonces TipoRegimen debe contener alguna de las claves 02, 03 o 04");
    //     }
    //     
    //     // Si TipoContrato tiene una clave 09 o más, entonces TipoRegimen debe ser una clave 05 hasta el 99
    //     if (CatalogosNomina.c_TipoContrato_09.Contains(_receptor.TipoContrato))
    //     {
    //         if(!CatalogosNomina.c_TipoRegimen_05_99.Contains(_receptor.TipoRegimen))
    //             _context.AddError(
    //                 Section, 
    //                 "Si TipoContrato tiene una clave 09 o más, entonces TipoRegimen debe ser una clave 05 hasta el 99");
    //     }
    //     
    //     _context.AddValue("tipoRegimen", _receptor.TipoRegimen);
    // }
    //
    // private void FormatRiesgoPuesto()
    // {
    //     if(string.IsNullOrEmpty(_receptor.RiesgoPuesto))
    //         return;
    //     if(!CatalogosNomina.c_RiesgoPuesto.Contains(_receptor.RiesgoPuesto))
    //         _context.AddError(Section, "El atributo Nomina.Receptor.RiesgoPuesto debe ser una clave del catálogo de c_RiesgoPuesto");
    // }
    //
    // private void FormatPeriodicidadPago()
    // {
    //     if(!CatalogosNomina.c_PeriodicidadPago.Contains(_receptor.PeriodicidadPago))
    //         _context.AddError(Section, "El atributo Nomina.Receptor.PeriodicidadPago debe ser una clave del catálogo de c_PeriodicidadPago");
    // }
    //
    // private void FormatBanco_CuentaBancaria()
    // {
    //     if(_receptor.Banco is null && _receptor.CuentaBancaria is null)
    //         return;
    //     
    //     if (!RegexCatalog.IsCuentaBancariaValid(_receptor.CuentaBancaria ?? string.Empty))
    //     {
    //         _context.AddError(Section, "El atributo CuentaBancaria debe tener una longitud de 10, 11, 16 o 18 posiciones");
    //         return;
    //     }
    //
    //     if (_receptor.CuentaBancaria?.Length is 18)
    //     {
    //         _receptor.Banco = null;
    //         return;
    //     }
    //
    //     if (_receptor.CuentaBancaria is { Length: 10 or 11 or 16 } && string.IsNullOrEmpty(_receptor.Banco))
    //     {
    //         _context.AddError(Section, "Si se registra una cuenta de tarjeta de débito a 16 posiciones o una cuenta bancaria a 11 posiciones o un número de teléfono celular a 10 posiciones, debe existir el atributo Banco");
    //     }
    //     
    //     if (!CatalogosNomina.c_Banco.Contains(_receptor.Banco ?? string.Empty))
    //     {
    //         _context.AddError(Section, "El atributo Banco debe ser una clave del catálogo de c_Banco");
    //     }
    //     
    // }
    // private void FormatSalarioBaseCotizacion()
    // {
    //     if(_receptor.SalarioBaseCotizacion is null)
    //         return;
    //     var salarioBaseCotizacion = decimal.Parse(_receptor.SalarioBaseCotizacion);
    //     var decimales = ValidateHelper.CountDecimalPlaces(salarioBaseCotizacion);
    //     if (decimales > 2)
    //     {
    //         var newValue = salarioBaseCotizacion.ToString("F2");
    //         _receptor.SalarioBaseCotizacion = newValue;
    //     }
    // }
    //
    // private void FormatSalarioDiarioIntegrado()
    // {
    //     if(_receptor.SalarioDiarioIntegrado is null)
    //         return;
    //     var salarioDiarioIntegrado = decimal.Parse(_receptor.SalarioDiarioIntegrado);
    //     var decimales = ValidateHelper.CountDecimalPlaces(salarioDiarioIntegrado);
    //     if (decimales > 2)
    //     {
    //         var newValue = salarioDiarioIntegrado.ToString("F2");
    //         _receptor.SalarioDiarioIntegrado = newValue;
    //     }
    // }
    //
    // private async Task FormatClaveEntidadFederal()
    // {
    //     if (!await _repositoryEstados.ExistsAsync("c_estado", _receptor.ClaveEntidadFederativa))
    //     {
    //         _context.AddError(Section, "El valor del atributo ClaveEntFed debe ser una clave del catálogo de c_Estado");
    //     }
    // }
    //
    // private void FormatSubcontratacion()
    // {
    //     var tiempoTotal = 0.000m;
    //     if(_receptor.Subcontratacion is null || _receptor.Subcontratacion.Count == 0)
    //         return;
    //     var count = _receptor.Subcontratacion.Count;
    //     var section = "{0} -> {1}. Subcontratación";
    //     for (var i = 0; i < count; i++)
    //     {
    //         var subcontratacion = _receptor.Subcontratacion[i];
    //         var tiempo = decimal.Parse(subcontratacion.PorcentajeTiempo);
    //         tiempoTotal += tiempo;
    //         if (!RegexCatalog.IsRfcValid(subcontratacion.RfcLabora))
    //         {
    //             _context.AddError(string.Format(section, Section, i + 1), "El valor del atributo RfcLabora no tiene un formato valido");
    //         }
    //     }
    //
    //     if (tiempoTotal != 100)
    //     {
    //         _context.AddError($"{Section} -> Subcontratación", $"La suma de los valores PorcentajeTiempo registrados en el atributo Nomina.Receptor.SubContratacion.PorcentajeTiempo debe ser igual a 100. Tiempo total registrado: %{tiempoTotal}");
    //     }
    // }
}