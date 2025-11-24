using System.Globalization;
using KPac.Application.Validator;
using KPac.Domain.CatalgosSAT;
using KpacModels.Shared.Models.Comprobante.Complementos.Pagos;
using KpacModels.Shared.XmlProcessing.Validator.Catalogos;

namespace KpacModels.Shared.XmlProcessing.Validator.Pagos;

public class PagoValidator
{
    private string _section = string.Empty;
    
    private Pago _pago = new Pago();

    private ValidatorContext _context = new();

   // private RepositoryValidator<cfdi40_moneda> _repositoryMoneda;
    
    
    public PagoValidator()//ClientValidator client)
    {
        //_repositoryMoneda = new RepositoryValidator<cfdi40_moneda>(client);
    }
    
    public async Task Validate(Pago pago, int numPago, ValidatorContext context)
    {
        _section = $"Comprobante -> Complemento -> Pagos -> {numPago}. Pago";
        _pago = pago;
        _context = context;
        
        
        ValidateFecha();
        ValidateFormaPago();
        await ValidateMoneda();
        ValidateTipoCambio();
        ValidateMonto();
        ValidateNumOperacion();
        ValidateRfcEmisorCuentaOrdenante();
        ValidateCuentaOrdenante();
        ValidateRfcEmisorCuentaBeneficiario();
        ValidateCuentaBeneficiario();
        ValidateTipoCadenaPago();
        ValidateCertificadoCadenaSelloPago();
    }

    private void ValidateFecha()
    {
        var fechaPago = _pago.FechaPago;
        if (string.IsNullOrEmpty(fechaPago))
        {
            _context.AddWarning(
                section: _section, 
                message: "El atributo FechaPago no puede ser nulo ni vació, es requerido para registrar la fecha y hora en la que el beneficiario recibe el pago.");
            return;
        }
        
        const string format = "yyyy-MM-dd'T'HH:mm:ss";
        
        if (!DateTime.TryParseExact(fechaPago, format, CultureInfo.InvariantCulture, DateTimeStyles.None, out _))
        {
            _context.AddError(
                code: "CFDI40101",
                message: "El campo FechaPago no cumple con el patrón requerido",
                section: "Comprobante");
            return;
        }
    }

    private void ValidateFormaPago()
    {
        var formaPago = _pago.FormaPago;
        if (string.IsNullOrEmpty(formaPago))
        {
            _context.AddWarning(
                section: _section, 
                message: "El atributo FormaDePagoP no puede ser nulo ni vació, es requerido para registrar la clave correspondiente a la forma en que se recibió el pago.");
            return;
        }

        if (formaPago == "99")
        {
            _context.AddError(
                code: "CRP20212",
                message: "El valor del campo FormaDePagoP debe ser distinto de '99'.",
                section: _section);
            return;
        }

        if (!CatalogosComprobante.c_FormaPago.Contains(formaPago))
        {
            _context.AddError(
                code: "CRP20999",
                section: _section,
                message: "El campo FormaDePagoP debe contener un valor del Catalogo c_FormaPago.");
        }
    }

    private async Task ValidateMoneda()
    {
        var moneda = _pago.Moneda;
        if (string.IsNullOrEmpty(moneda))
        {
            _context.AddWarning(
                section: _section, 
                message: "El atributo MonedaP no puede ser nulo ni vació, es requerido para registrar la clave correspondiente a la forma en que se recibió el pago.");
            return;
        }

        if (moneda == "XXX")
        {
            _context.AddError(
                code: "CRP20213",
                section: _section,
                message: "El campo MonedaP debe ser distinto de 'XXX'.");
            return;
        }
        
        // var monedaObject = await _repositoryMoneda.GetAsync("c_moneda", moneda);
        // if (monedaObject == null)
        // {
        //     _context.AddError(
        //         code: "CRP20999",
        //         section: _section,
        //         message: "MonedaP debe contener un valor del catálogo c_Moneda.");
        //     return;
        // }
        // // Si es valido se guardara el atributo Moneda y el número de decimales en el contexto para realizar la
        // // validación de monedas en el docto relacionado.
        // _context.AddValue("monedaDecimalesP", monedaObject.decimales);
        // _context.AddValue("monedaP", moneda);
    }

    private void ValidateTipoCambio()
    {
        var moneda = _pago.Moneda;
        var tipoCambio = _pago.TipoCambio;
        if(string.IsNullOrEmpty(moneda)) return;
        
        if (moneda != "MXN" && string.IsNullOrEmpty(tipoCambio))
        {
            _context.AddError(
                code: "CRP20214",
                section: _section,
                message: "Si el atributo MonedaP es diferente de MXN, debe existir información en el atributo TipoCambioP.");
            return;
        }

        if (moneda == "MXN" && tipoCambio != "1")
        {
            _context.AddError(
                code: "CRP20215",
                section: _section,
                message: "Si el atributo MonedaP es MXN, se debe registrar el valor '1' en el atributo TipoCambioP. ");
            return;
        }
        var countDecimals = ValidateHelper.CountDecimalPlaces(decimal.Parse(tipoCambio ?? "0"));
        if (countDecimals > 6)
        {
            _context.AddWarning(
                section: _section,
                message: $"El atributo TipoCambioP puede tener solo hasta 6 decimales. Valor registrado: {tipoCambio} con {countDecimals} decimales.");
        }
        _context.AddValue("tipoCambioP", tipoCambio ?? "1");
        
        // TODO
        //  - **CRP20216**
        //  - Cuando el valor de este atributo se encuentre fuera de los límites establecidos, el emisor debe obtener
        //  de manera no automática una clave de confirmación para ratificar que el valor es correcto e integrarla al
        //  CFDI en el atributo CFDI:Confirmacion. 
        
    }

    private void ValidateMonto()
    {
        var montoString = _pago.Monto;
        if (string.IsNullOrEmpty(montoString))
        {
            _context.AddError(
                code: "CRP20999",
                section: _section,
                message: "El campo Monto es requerido.");
            return;
        }
        var monto = decimal.Parse(montoString);
        if (monto < 0)
        {
            _context.AddError(
                code: "CRP20218",
                section: _section,
                message: "El campo Monto debe ser mayor a 0.");
            return;
        }
        var monedaDecimales = int.Parse(_context.GetValue("monedaDecimales") ?? "0");
        var montoDecimales = ValidateHelper.CountDecimalPlaces(monto);
        if (monedaDecimales > montoDecimales)
        {
            _context.AddError(
                code: "CRP20219",
                section: _section,
                message: $"El valor del campo Monto debe tener hasta la cantidad de decimales que soporte la moneda" +
                         $" registrada en el campo MonedaP. Valor registrado en el campo Monto {monto} con " +
                         $"{montoDecimales} decimales. Número de decimales permitido por la moneda {monedaDecimales}.");
            return;
        }
        // TODO
        //  - **CRP20217**
        //  - Cuando la moneda registrada en el Documento Relacionado sea igual a la del Pago, la suma de los valores registrados en el nodo DoctoRelacionado, atributo ImpPagado, debe ser menor o igual que el valor de este atributo. Al ser mismas monedas, no se calculan los márgenes de variación (límites inferior y superior) por efecto de redondeo.
        //  - La suma de los valores registrados en el campo ImpPagado de los apartados DoctoRelacionado no es menor o igual que el valor del campo Monto.
        
        
        // TODO
        //  - **CRP20220**
        //  - Cuando el valor equivalente en MXN de este atributo exceda el límite establecido, el emisor debe obtener de manera no automática una clave de confirmación para ratificar que el importe es correcto e integrarla al CFDI en el atributo CFDI:Confirmacion. La clave de confirmación la asigna el PAC.
        //  - Cuando el valor del campo Monto se encuentre fuera de los límites establecidos, debe existir el campo Confirmacion
        
        // TODO
        //  - **CRP20276**
        //  - En el caso donde la suma de los atributos ImpPagado de cada documento relacionado convertido a la moneda del pago no sea menor o igual al atributo Monto en operaciones con diferentes divisas, se debe validar que el valor del atributo Monto sea mayor o igual al resultado de sumar los límites inferiores y menor o igual al resultado de sumar los límites superiores de cada ImpPagado calculados previamente.
        //  - El valor del campo Monto no se encuentra entre el límite inferior y superior permitido.
    }

    private void ValidateNumOperacion()
    {
        var numOperacion = _pago.NoOperacion;
        if(string.IsNullOrEmpty(numOperacion))return;
        if (numOperacion.Length is < 1 or > 100)
        {
            _context.AddWarning(
                section: _section, 
                message: $"El valor del campo NumOperacion debe contener se de 1 hasta 100 caracteres. Numero de " +
                         $"caracteres registrados en el campo NumOperacion {numOperacion.Length}.");
        }
    }

    private void ValidateRfcEmisorCuentaOrdenante()
    {
        var formaPago = _pago.FormaPago;
        var rfcEmisor = _pago.RfcEmisorCuentaOrdenante;
        if (!CatalogosComprobante.formaPagoListCtaOrd.Contains(formaPago) && !string.IsNullOrEmpty(rfcEmisor))
        {
            _context.AddError(
                code: "CRP20221",
                section: _section,
                message: "Cuando el valor del campo FormaDePagoP sea diferente a la clave 02, 03, 04, 05, 06, 28 y 29," +
                         " entonces el campo RfcEmisorCtaOrd no debe existir.");
            return;
        }
        
        var nombreBanco = _pago.NombreBancoOrdenanteExtranjero;

        if (rfcEmisor == "XEXX010101000" && string.IsNullOrEmpty(nombreBanco))
        {
            _context.AddError(
                code: "CRP20223",
                section: _section,
                message: "Cuando utilice el RFC genérico XEXX010101000 en el campo RfcEmisorCuentaOrdenante" +
                         " entonces el campo NombreBancoOrdenanteExtranjero se debe registrar.");
            return;
        }
        
        // TODO
        //  - **CRP20222**
        //  - Cuando no se utilice el RFC genérico XEXX010101000, el RFC debe estar en la lista de RFC inscritos en el SAT.
    }

    private void ValidateCuentaOrdenante()
    {
        var formaPago = _pago.FormaPago;
        var cuentaOrdenante = _pago.CuentaOrdenante;
        
        
        if (!CatalogosComprobante.formaPagoListCtaOrd.Contains(formaPago) && !string.IsNullOrEmpty(cuentaOrdenante))
        {
            _context.AddError(
                code: "CRP20224",
                section: _section,
                message: "Cuando el valor del campo FormaDePagoP sea diferente a la clave 02, 03, 04, 05, 06, 28 y 29, el campo CuentaOrdenante no se debe registrar. ");
            return;
        }
        
        if(string.IsNullOrEmpty(cuentaOrdenante)) 
            return;

        ValidateHelper.ValidateCuentaOrdenante(
            cuentaOrdenante: cuentaOrdenante,
            formaPago: formaPago,
            context: _context,
            section: _section);

    }

    private void ValidateRfcEmisorCuentaBeneficiario()
    {
        var formaPago = _pago.FormaPago;
        var rfcEmisor = _pago.RfcEmisorCuentaBeneficiario;
        if (!CatalogosComprobante.formaPagoListCtaOrd.Contains(formaPago) && !string.IsNullOrEmpty(rfcEmisor))
        {
            _context.AddError(
                code: "CRP20227",
                section: _section,
                message: "Cuando el valor del campo FormaDePagoP sea diferente a la clave 02, 03, 04, 05, 06, 28 y 29," +
                         " entonces el campo RfcEmisorCuentaBeneficiario no debe existir.");
            return;
        }

    }

    private void ValidateCuentaBeneficiario()
    {
        var formaPago = _pago.FormaPago;
        var cuentaBeneficiario = _pago.CuentaBeneficiario;
        
        
        if (!CatalogosComprobante.formaPagoListCtaBen.Contains(formaPago) && !string.IsNullOrEmpty(cuentaBeneficiario))
        {
            _context.AddError(
                code: "CRP20228",
                section: _section,
                message: "Cuando el valor del campo FormaDePagoP sea diferente a la clave 02, 03, 04, 05, 06, 28 y 29, el campo CuentaBeneficiario no se debe registrar. ");
            return;
        }
        
        if(string.IsNullOrEmpty(cuentaBeneficiario)) 
            return;

        ValidateHelper.ValidateCuentaBeneficiario(
            cuentaOrdenante: cuentaBeneficiario,
            formaPago: formaPago,
            context: _context,
            section: _section);
    }

    private void ValidateTipoCadenaPago()
    {
        var tipoCadenaPago = _pago.TipoCadenaPago;
        var formaPago = _pago.FormaPago;

        if (formaPago != "03" && !string.IsNullOrEmpty(tipoCadenaPago))
        {
            _context.AddError(
                code: "CRP20229",
                section: _section,
                message: "Cuando el valor del campo FormaDePagoP es diferente a 03 el campo TipoCadenaPago no se debe registrar.");
            return;
        }

        
        if(string.IsNullOrEmpty(tipoCadenaPago)) return;
        if (tipoCadenaPago != "01")
        {
            _context.AddError(
                code: "CRP20999",
                section: _section,
                message: "Cuando el valor del campo TipoCadenaPago no contiene un valor del catalogo c_TipoCadenaPago.");
            return;
        }
    }

    private void ValidateCertificadoCadenaSelloPago()
    {
        var tipoCadenaPago = _pago.TipoCadenaPago;
        // Si existe el campo TipoCadenaPago se debe registrar el campo CertificadoPago, CadenaPago y SelloPago.
        if (!string.IsNullOrEmpty(tipoCadenaPago))
        {
            var certificadoPago = _pago.CertificadoPago;
            if (string.IsNullOrEmpty(certificadoPago))
            {
                _context.AddError(
                    code: "CRP20230",
                    section: _section,
                    message: "Si existe el campo TipoCadenaPago es obligatorio registrar el campo CertificadoPago.");
            }
            var cadenaPago = _pago.CadenaPago;
            if (string.IsNullOrEmpty(cadenaPago))
            {
                _context.AddError(
                    code: "CRP20232",
                    section: _section,
                    message: "Si existe el campo TipoCadenaPago es obligatorio registrar el campo CadenaPago.");
            }
            var selloPago = _pago.SelloPago;
            if (string.IsNullOrEmpty(selloPago))
            {
                _context.AddError(
                    code: "CRP20234",
                    section: _section,
                    message: "Si existe el campo TipoCadenaPago es obligatorio registrar el campo SelloPago.");
            }
        }
        else
        {
            var certificadoPago = _pago.CertificadoPago;
            if (!string.IsNullOrEmpty(certificadoPago))
            {
                _context.AddError(
                    code: "CRP20230",
                    section: _section,
                    message: "Si no existe el campo TipoCadenaPago no se debe registrar el campo CertificadoPago.");
            }
            var cadenaPago = _pago.CadenaPago;
            if (!string.IsNullOrEmpty(cadenaPago))
            {
                _context.AddError(
                    code: "CRP20232",
                    section: _section,
                    message: "Si no existe el campo TipoCadenaPago no se debera registrar el campo CadenaPago.");
            }
            var selloPago = _pago.SelloPago;
            if (!string.IsNullOrEmpty(selloPago))
            {
                _context.AddError(
                    code: "CRP20234",
                    section: _section,
                    message: "Si no existe el campo TipoCadenaPago no se debera registrar el campo SelloPago.");
            }
        }
    }

    public void ValidateMonto(decimal monto, int numPago, ValidatorContext context)
    {
        _section = $"Comprobante -> Complemento -> Pagos -> {numPago}. Pago";
        var helper = new MontoHelper(context);
        var limiteInferiorImpPagado = helper.GetImpPagadoLimiteInferiorMonto();
        var limiteSuperiorImpPagado = helper.GetImpPagadoSuperiorMonto();
        if (limiteSuperiorImpPagado == limiteInferiorImpPagado)
        {
            if (monto  < limiteSuperiorImpPagado)
            {
                // No son iguales lanzar error
                _context.AddError(
                    code: "CRP20217",
                    message: $"La suma de los campos ImportePagado de los DocumentosRelacionados {limiteSuperiorImpPagado} es mayor al Monto del Pago {monto}.",
                    section: _section);
                return;
            }
        }
        else
        {
            // Si las monedas son diferentes del nodo DoctoRelacionado y Pago
            if (monto < limiteInferiorImpPagado)
            {
                // Error 1
                _context.AddError(
                    code: "CRP20276",
                    message: $"El valor del atributo Monto {monto} es menor al resultado de sumar los límites inferiores {limiteInferiorImpPagado} de cada ImpPagado calculados previamente.",
                    section: _section);
            }

            if (monto > limiteSuperiorImpPagado)
            {
                // Error 2
                _context.AddError(
                    code: "CRP20276",
                    message: $"El valor del atributo Monto {monto} es mayor al resultado de sumar los límites superiores {limiteSuperiorImpPagado} de cada ImpPagado calculados previamente.",
                    section: _section);
            }
        }

        helper.CleanLimites();
        helper.AddMontoPagado(decimal.Parse(_pago.TipoCambio ?? "1"), monto);
    }
    
}