using KPac.Application.Formatter;
using KPac.Application.Validator.Catalogos;
using KpacModels.Shared.Models.Comprobante.Complementos.Pagos;
using KpacModels.Shared.XmlProcessing.Validator.Catalogos;

namespace KpacModels.Shared.XmlProcessing.Formatter.Pagos;

public class PagoFormatter
{
    private Pago _pago;
    
    private FormatContext _context;

    private string _section;

    //private RepositoryValidator<cfdi40_moneda> _repositoryMoneda;
    
    public PagoFormatter(FormatContext context
        //, ClientValidator client
        )
    {
        _context = context;
        //_repositoryMoneda = new RepositoryValidator<cfdi40_moneda>(client);
    }

    public async Task Format(Pago pago, int noPago)
    {
        _section = $"Comprobante -> Complemento -> Pagos -> {noPago}. Pago";
        _pago = pago;
        await FormatPago();
    }

    private async Task FormatPago()
    {

        if (_pago.FechaPago == null)
            _pago.FechaPago = PagosFormatHelper.GenerateFechaPago();
        
        if(_pago.FormaPago == "99")
            _context.AddError(_section, "El valor de FormaDePagoP debe ser distinto de '99'");

        if (_pago.Moneda == null)
        {
            _pago.Moneda = "MXN";
            _pago.TipoCambio = "1";
        }

        if(_pago.Moneda == "XXX")
            _context.AddError(_section, "El valor de MonedaP debe ser distinto de 'XXX'");

        if (string.IsNullOrEmpty(_pago.TipoCambio))
        {
            _context.AddError(_section, "El campo TipoCambioP se debe registrar");
            return;
        }

        // var monedaObject = await _repositoryMoneda.GetAsync("c_moneda", _pago.Moneda);
        // if (monedaObject == null)
        // {
        //     _context.AddError(_section, "El campo MonedaP no contiene un valor del catálogo c_Moneda.");
        //     return;
        // }
        //_context.AddValue("decimalesMoneda",  monedaObject.decimales);
        _context.AddValue("monedaP",  _pago.Moneda);
        
        FormatCuentaOrd();
        FormatCuentaBeneficiario();
        FormatTipoCadenaPago();

    }
    
    private void FormatCuentaOrd()
    {
        if (!CatalogosComprobante.formaPagoListCtaOrd.Contains(_pago.FormaPago))
        {
            _pago.RfcEmisorCuentaOrdenante = null;
            _pago.CuentaOrdenante = null;
            return;
        }
        var cuentaOrdenante = _pago.CuentaOrdenante;
        if (!string.IsNullOrEmpty(cuentaOrdenante))
        {
            PagosFormatHelper.ValidateCuentaOrdenante(
                cuentaOrdenante: cuentaOrdenante,
                formaPago: _pago.FormaPago,
                context: _context,
                section: _section);
        }

        
    }

    private void FormatCuentaBeneficiario()
    {
        if (!CatalogosComprobante.formaPagoListCtaBen.Contains(_pago.FormaPago))
        {
            _pago.RfcEmisorCuentaBeneficiario = null;
            _pago.CuentaBeneficiario = null;
            return;
        }
        var cuentaBeneficiario = _pago.CuentaBeneficiario;
        if (!string.IsNullOrEmpty(cuentaBeneficiario))
        {
            PagosFormatHelper.ValidateCuentaBeneficiario(
                cuentaOrdenante: cuentaBeneficiario,
                formaPago: _pago.FormaPago,
                context: _context,
                section: _section);
        }
        
       
    }

    private void FormatTipoCadenaPago()
    {
        if (_pago.FormaPago != "03")
        {
            _pago.TipoCadenaPago = null;
            _pago.CertificadoPago = null;
            _pago.CadenaPago = null;
            _pago.SelloPago = null;
            return;
        }

        if (!string.IsNullOrEmpty(_pago.TipoCadenaPago))
        {
            if(string.IsNullOrEmpty(_pago.CertificadoPago) || string.IsNullOrEmpty(_pago.SelloPago) || string.IsNullOrEmpty(_pago.CadenaPago))
                _context.AddError(_section, "Si existe el campo Tipo Cadena Pago es obligatorio registrar los campos 'Certificado Pago', 'Cadena Pago'  y 'Sello Pago'.");
        }
    }

}