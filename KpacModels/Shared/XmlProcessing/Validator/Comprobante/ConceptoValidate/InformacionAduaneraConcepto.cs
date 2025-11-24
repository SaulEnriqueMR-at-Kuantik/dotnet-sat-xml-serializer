using KPac.Application.Common.Validators;
using KPac.Application.Validator;
using KpacModels.Shared.Models.Comprobante;

namespace KpacModels.Shared.XmlProcessing.Validator.Comprobante.ConceptoValidate;

public class InformacionAduaneraConcepto
{
    
    private ValidatorContext _context;
    private int _numConcepto;
    private bool _hasComercioExterior;
    public InformacionAduaneraConcepto(ValidatorContext context, int numConcepto)
    {
        _context = context;
        _numConcepto = numConcepto;
        _hasComercioExterior = (_context.GetValue("HasComercioExterior") != null);
    }

    public void Validate(List<InformacionAduanera> informacionAduanera)
    {
        
        var count = informacionAduanera.Count;

        if (_hasComercioExterior && count > 0)
        {
            _context.AddError(
                code: "CFDI40200",
                section: $"Comprobante -> {_numConcepto}.-Concepto -> Información Aduanera",
                message: "El NumeroPedimento no debe existir si se incluye el complemento de comercio exterior.");
            return;
        }
        
        for (var i = 0; i < count; i++)
        {
            var informacion = informacionAduanera[i];
            var section = $"Comprobante -> {_numConcepto}.-Concepto -> {i + 1}.-Información Aduanera";
            ValidateInformacion(informacion, section);
        }
    }

    private void ValidateInformacion(InformacionAduanera informacion, string section)
    {
        if (!RegexCatalog.IsNumeroPedimentoValid(informacion.NumeroPedimento))
        {
            _context.AddError(
                code: "CFDI40199",
                section: section,
                message: ErrorMessages.NumeroPedimento);
        }
    }
}