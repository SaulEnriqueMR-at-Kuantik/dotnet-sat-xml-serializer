using KPac.Application.Validator;
using KpacModels.Shared.Models.Comprobante;

namespace KpacModels.Shared.XmlProcessing.Validator.Comprobante.ConceptoValidate;

public class CuentaPredialValidator
{
    private ValidatorContext _context;

    private  int _numConcepto;
    public CuentaPredialValidator(ValidatorContext context, int numConcepto)
    {
        _context = context;
        _numConcepto = numConcepto;
    }
    public void Validate(List<CuentaPredial> cuentasPredial)
    {
        var count = cuentasPredial.Count;
        for (int i = 0; i < count; i++)
        {
            var section = $"Comprobante -> {_numConcepto}. Concepto -> {i + 1}. CuentasPredial";
            var cuentaPredial = cuentasPredial[i];
            if (string.IsNullOrEmpty(cuentaPredial.Numero))
            {
                _context.AddError(
                    code: "CFDI40999", 
                    section: section,
                    message: "El Número registrado no puede ser vació.");
            }

            if (cuentaPredial.Numero.Length > 150)
            {
                _context.AddError(
                    code: "CFDI40999", 
                    section: section,
                    message: "El Número registrado no puede tener más de 150 caracteres.");
            }

            if (!RegexCatalog.IsOnlyNumberAndLetter(cuentaPredial.Numero))
            {
                _context.AddError(
                    code: "CFDI40999", 
                    section: section,
                    message: "El Número registrado no cumple con el formato establecido, solo debe tener números," +
                             $" letras mayúsculas y minúsculas. Valor registrado: {cuentaPredial.Numero}.");
            }
        }
    }
}