using KPac.Application.Common.Validators;
using KPac.Application.Validator;
using KPac.Domain.CatalgosSAT;
using KpacModels.Shared.Models.Comprobante;

namespace KpacModels.Shared.XmlProcessing.Validator.Comprobante.ConceptoValidate;

public class ParteValidate
{
    //private RepositoryValidator<cfdi40_claveprodserv> _repository;

    private ValidatorContext _comprobanteContext;

    private string _section = string.Empty;
    
    private int _numConcepto;

    private int _noDecimalesMoneda;
    public ParteValidate(ValidatorContext comprobanteContext, int numConcepto
        //, ClientValidator client
        )
    {
        //_repository = new RepositoryValidator<cfdi40_claveprodserv>(client);
        _comprobanteContext = comprobanteContext;
        _numConcepto = numConcepto;
        var noDecimalesString = _comprobanteContext.GetValue("monedaDecimales");
        _noDecimalesMoneda = int.Parse(noDecimalesString ?? "0");
    }

    public async Task Validate(Parte parte, int noParte)
    {
        _section = $"Comprobante -> {_numConcepto}. Concepto -> {noParte}. Parte";
        await ValidateClaveProSer(parte.ClaveProdServ);
        ValidateNoIdentificacion(parte.NoIdentificacion);
        ValidateCantidad(parte.Cantidad);
        ValidateUnidad(parte.Unidad);
        ValidateDescripcion(parte.Descripcion);
        ValidateValorUnitario(parte.ValorUnitario);
        ValidateImporte(parte.Importe, parte.ValorUnitario, parte.Cantidad);
        ValidateInformacionAduanera(parte.InformacionAduanera);
    }

    private async Task ValidateClaveProSer(string claveProdServ)
    {
        if (string.IsNullOrWhiteSpace(claveProdServ))
        {
            _comprobanteContext.AddWarning(
                section: _section,
                message: "El campo ClaveProdServ no puede ser nulo ni vacio");
            return;
        }

        // if (! await _repository.ExistsAsync("c_claveprodserv", claveProdServ))
        // {
        //     _comprobanteContext.AddError(
        //         code: "CFDI40196",
        //         section: _section,
        //         message: "El campo ClaveProdServ, no contiene un valor del catálogo c_ClaveProdServ.");
        //     return;
        // }
    }

    private void ValidateNoIdentificacion(string? noIdentificacion)
    {
        if(string.IsNullOrEmpty(noIdentificacion))
            return;

        if (noIdentificacion.Length > 100)
        {
            _comprobanteContext.AddWarning(
                section: _section,
                message: "El campo NoIdentificacion no puede tener mas de 100 caracteres.");
        }
    }

    private void ValidateCantidad(string cantidad)
    {
        if (string.IsNullOrEmpty(cantidad))
        {
            _comprobanteContext.AddWarning(
                section: _section,
                message: "El campo Cantidad es obligatorio, no puede ser nulo ni vació.");
        }
    }

    private void ValidateUnidad(string? unidad)
    {
        
    }

    private void ValidateDescripcion(string descripcion)
    {
        if (string.IsNullOrEmpty(descripcion))
        {
            _comprobanteContext.AddWarning(
                section: _section,
                message: "El campo Descripcion es obligatorio, no puede ser nulo ni vació.");
            return;
        }

        if (descripcion.Length > 1000)
        {
            _comprobanteContext.AddWarning(
                section: _section,
                message: "El campo Descripcion no puede tener mas de 1000 caracteres.");
        }
    }

    private void ValidateValorUnitario(string valorUnitarioString)
    {
        if(string.IsNullOrWhiteSpace(valorUnitarioString))
            return;
        var valorUnitario = decimal.Parse(valorUnitarioString);
        if (valorUnitario < 0)
        {
            _comprobanteContext.AddError(
                code: "CFDI40197",
                section: _section,
                message: "El valor del campo ValorUnitario debe ser mayor que cero (0).");
            return;
        }
    }

    private void ValidateImporte(string importeString, string valorUnitarioString, string cantidadString)
    {

        if (!string.IsNullOrEmpty(importeString) && 
            !string.IsNullOrEmpty(valorUnitarioString) &&
            !string.IsNullOrEmpty(cantidadString))
        {
            var cantidad = decimal.Parse(cantidadString);
            var valorUnitario = decimal.Parse(valorUnitarioString);
            var importe = decimal.Parse(importeString);
            var limiteSuperior = DecimalOperatorLimites.CalcularLimiteSuperiorConcepto(
                cantidad: cantidad,
                valorUnitario: valorUnitario,
                decimalesMoneda: _noDecimalesMoneda);
            var limiteInferior = DecimalOperatorLimites.CalcularLimiteInferiorConcepto(
                cantidad: cantidad,
                valorUnitario: valorUnitario,
                decimalesMoneda: _noDecimalesMoneda);
            if (importe > limiteSuperior || importe < limiteInferior)
            {
                _comprobanteContext.AddError(
                    code: "CFDI40198",
                    section: _section,
                    message: $"El valor del campo Importe no se encuentra entre el limite inferior y superior permitido." +
                             $" Limite superior: {limiteSuperior}. Limite inferior: {limiteInferior}. Valor importe: {importe}");
                return;
            }
        }
    }

    private void ValidateInformacionAduanera(List<InformacionAduanera>? informacionAduaneraList)
    {
        if(informacionAduaneraList == null || informacionAduaneraList.Count == 0)
            return;
        var count = informacionAduaneraList.Count;
        for (int i = 0; i < count; i++)
        {
            var section = $"{_section} -> InformacionAduanera -> {i + 1}. NumeroPedimento";
            var informacionAduanera = informacionAduaneraList[i];
            ValidateInformacion(informacionAduanera, section);
        }
    }
    
    
    private void ValidateInformacion(InformacionAduanera informacion, string section)
    {
        if (!RegexCatalog.IsNumeroPedimentoValid(informacion.NumeroPedimento))
        {
            _comprobanteContext.AddError(
                code: "CFDI40199",
                section: section,
                message: ErrorMessages.NumeroPedimento);
        }
    }
}