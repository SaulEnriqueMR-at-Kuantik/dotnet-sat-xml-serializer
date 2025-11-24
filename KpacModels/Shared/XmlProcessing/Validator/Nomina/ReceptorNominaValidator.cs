using KPac.Application.Validator;
using KpacModels.Shared.Models.Comprobante.Complementos.Nomina;

namespace KpacModels.Shared.XmlProcessing.Validator.Nomina;

public class ReceptorNominaValidator
{
    private readonly ValidatorContext _context;

    private readonly string _section = "Comprobante -> Complemento -> Nomina -> Receptor";
    
    
    private Receptor _receptor;

    public ReceptorNominaValidator(ValidatorContext context)
    {
        _context = context;
    }

    public void Validate(Receptor receptor)
    {
        _receptor = receptor;
        ValidateCurp();
        ValidateNumSeguridadSocial();
        ValidateFechaInicioRelLaboral();
        ValidateAntiguedad();
        ValidateTipoContrato();
        ValidateSindicalizado();
        ValidateTipoJornada();
        ValidateTipoRegimen();
        ValidateNumEmpleado();
        ValidateDepartamento();
        ValidatePuesto();
        ValidateRiesgoPuesto();
        ValidatePeriodicidadPago();
        ValidateBanco();
        ValidateCuentaBancaria();
        ValidateSalarioBaseCotApor();
        ValidateSalarioDiarioIntegrado();
        ValidateClaveEntFed();
    }

    private void ValidateCurp()
    {
        var curp = _receptor.Curp;
        if (!RegexCatalog.IsCurpValid(curp))
        {
            _context.AddWarning(_section, "El atributo curp debe seguir el patrón t_CURP");
        }
    }

    private void ValidateNumSeguridadSocial()
    {
        var noSeguridad = _receptor.NoSeguridadSocial;
        if(string.IsNullOrEmpty(noSeguridad))
            return;
        var length =  noSeguridad.Length;
        if (length is < 1 or > 15)
        {
            _context.AddWarning(_section, "El atributo NumSeguridadSocial debe tener solo entre 1 a 15 caracteres");
        }
    }

    private void ValidateFechaInicioRelLaboral()
    {
        throw new NotImplementedException();
    }

    private void ValidateAntiguedad()
    {
        throw new NotImplementedException();
    }

    private void ValidateTipoContrato()
    {
        throw new NotImplementedException();
    }

    private void ValidateSindicalizado()
    {
        throw new NotImplementedException();
    }

    private void ValidateTipoJornada()
    {
        throw new NotImplementedException();
    }

    private void ValidateTipoRegimen()
    {
        throw new NotImplementedException();
    }

    private void ValidateNumEmpleado()
    {
        throw new NotImplementedException();
    }

    private void ValidateDepartamento()
    {
        throw new NotImplementedException();
    }

    private void ValidatePuesto()
    {
        throw new NotImplementedException();
    }

    private void ValidateRiesgoPuesto()
    {
        throw new NotImplementedException();
    }

    private void ValidatePeriodicidadPago()
    {
        // NOM33 Si el atributo Nomina.TipoNomina es ordinaria el tipo de periodicidad de pago debe ser distinta de la clave "99".
    }

    private void ValidateBanco()
    {
        throw new NotImplementedException();
    }

    private void ValidateCuentaBancaria()
    {
        throw new NotImplementedException();
    }

    private void ValidateSalarioBaseCotApor()
    {
        throw new NotImplementedException();
    }

    private void ValidateSalarioDiarioIntegrado()
    {
        throw new NotImplementedException();
    }

    private void ValidateClaveEntFed()
    {
        throw new NotImplementedException();
    }
}