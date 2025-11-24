using System.Globalization;
using KPac.Application.Validator;
using KpacModels.Shared.Models.Comprobante;
using KpacModels.Shared.XmlProcessing.Validator.Catalogos;

namespace KpacModels.Shared.XmlProcessing.Validator.Comprobante;

public class ComprobanteValidator
{
    protected string TipoComprobante = string.Empty;
    protected decimal Subtotal = 0.0m;
    protected decimal Descuento = 0.0m;
    private ValidatorContext _context = new();
    //private ClientValidator _clientValidator;
    //private readonly RepositoryValidator<cfdi40_moneda> _repositoryMoneda;

    public ComprobanteValidator()
        //ClientValidator clientValidator)
    {
        //_clientValidator = clientValidator;
       // _repositoryMoneda = new RepositoryValidator<cfdi40_moneda>(clientValidator);
    }
    public async Task ValidateBase(Comprobante40 comprobante, ValidatorContext comprobanteContext)
    {
        _context = comprobanteContext;
        TipoComprobante = comprobante.TipoComprobante ?? string.Empty;
        ValidateVersion(comprobante.Version);
        ValidateSerie(comprobante.Serie);
        ValidateFolio(comprobante.Folio);
        ValidateFecha(comprobante.Fecha);
        ValidateFormaPago(comprobante.FormaPago, comprobante.MetodoPago);
        ValidateCondicionesPago(comprobante.CondicionesPago);
        await ValidateMoneda(comprobante.Moneda);
        ValidateTipoCambio(comprobante.TipoCambio, comprobante.Moneda);
        ValidateTipoComprobante(comprobante);
        ValidateExportacion(comprobante);
        ValidateMetodoPago(comprobante.MetodoPago);
        ValidateLugarExpedicion(comprobante.LugarExpedicion);
        ValidateConfirmacion(comprobante.Confirmacion);
        ValidateComplemento(comprobante.Complemento);
        ValidateInformacionGlobal(comprobante);
        _context.AddValue("tipoComprobante", TipoComprobante);
    }

    public void Validate(Comprobante40 comprobante, ValidatorContext comprobanteContext)
    {
        _context = comprobanteContext;
        TipoComprobante = comprobante.TipoComprobante ?? string.Empty;
        ValidateSubTotal(comprobante.Subtotal, comprobante.Conceptos);
        ValidateDescuento(comprobante.Descuento, comprobante.Conceptos, comprobante.Subtotal);
        ValidateTotal(comprobante);
    }

    


    /// <summary>
    /// Restricciones:
    /// - Valor 4.0
    /// </summary>
    /// <param name="version">Versión del comprobante</param>
    private void ValidateVersion(string version)
    {
        if(string.IsNullOrEmpty(version))
            _context.AddWarning(section: "Comprobante", message: "Version no puede ser nulo ni vacio.");
        if(version != "4.0")
            _context.AddWarning(section: "Comprobante", message: $"Version de comprobante '{version}' no valida, debe ser 4.0");
    }
    
    /// <summary>
    /// Restricciones:
    /// - Máximo 25 carácteres.
    /// </summary>
    /// <param name="serie">string</param>
    private void ValidateSerie(string? serie)
    {
        if (string.IsNullOrEmpty(serie)) return;
        if (serie.Length > 25)
        {
            _context.AddWarning(
                section: "Comprobante",
                message: "Serie debe tener máximo 25 carácteres.");
        }
    }
    
    /// <summary>
    /// Restricciones:
    /// - Máximo 40 carácteres.
    /// </summary>
    /// <param name="serie"></param>
    private void ValidateFolio(string? folio)
    {
        if (string.IsNullOrEmpty(folio)) return;
        if (folio.Length > 40)
        {
            _context.AddWarning(
                section: "Comprobante", 
                message: "Folio debe tener máximo 40 carácteres.");
        }
    }
    
    /// <summary>
    /// Restricciones: 
    /// - Diferencia entre Fecha y Fecha de timbrado no debe ser mayor a 72 horas.
    /// - Formato ISO **AAAA-MM-DDThh:mm:ss**.
    /// </summary>
    /// <param name="fecha">string</param>
    private void ValidateFecha(string? fecha)
    {
        if (string.IsNullOrWhiteSpace(fecha))
        {
            _context.AddWarning(
                message:"Fecha no debe ser nulo o vació.",
                section: "Comprobante");
            return;
        }
        const string format = "yyyy-MM-dd'T'HH:mm:ss";
        
        if (!DateTime.TryParseExact(fecha, format, CultureInfo.InvariantCulture, DateTimeStyles.None, out var fechaCfdi))
        {
            _context.AddError(
                code: "CFDI40101",
                message: "El campo Fecha no cumple con el patrón requerido",
                section: "Comprobante");
            return;
        }
        var threeDaysAgo = DateTime.Now.Subtract(TimeSpan.FromDays(3));
        Console.WriteLine(threeDaysAgo.ToString(format));
        if(fechaCfdi < threeDaysAgo)
            _context.AddWarning(section: "Comprobante",message: "Fecha de timbrado mayor a 3 dias");
    }
    
    // Tipos de comprobantes en los que forma de pago no debe existir.
    private readonly List<string> _tiposComprobanteNoDebeExistir = ["T", "N", "P"]; 
    // Tipos de comprobantes en los que forma de pago es obligatorio
    private readonly List<string> _tiposComprobanteObligatorio = ["I", "E"];
    private void ValidateFormaPago(string? formaPago, string? metodoPago = null)
    {
        // Obligatorio cuando TipoDeComprobante es I o E
        if (string.IsNullOrWhiteSpace(formaPago) && _tiposComprobanteObligatorio.Contains(TipoComprobante))
        {
            _context.AddWarning(
                section: "Comprobante",
                message: "FormaPago es obligatorio cuando TipoDeComprobante es I o E");
            return;
        }
        
        if(string.IsNullOrWhiteSpace(formaPago)) return;
        // Obligatorio pertenecer a catálogo: CFormaPago.
        if (!CatalogosComprobante.c_FormaPago.Contains(formaPago))
        {
            _context.AddError(
                code: "CFDI40104", 
                message: $"El valor de FormaPago '{formaPago}' no pertenece al catálogo c_FormaPago.",
                section: "Comprobante");
            return;
        }
        // No debe existir cuando TipoDeComprobante es N, T o P.
        if(_tiposComprobanteNoDebeExistir.Contains(TipoComprobante))
            _context.AddError(
                code: "CFDI40103",
                section:"Comprobante",
                message: "Si existe el tipo de comprobante T, N o P el campo FormaPago no debe existir.");
        
        // Si forma de pago es 99, MetodoPago debe tener el valor PPD
        if(formaPago != "99" && metodoPago == "PPD")
            _context.AddError(
                code: "CFDI40105", 
                section: "Comprobante", 
                message: "FormaPago debe contener el valor '99' cuando el atributo MetodoPago contenga el valor 'PPD'.");
        return;
    }
    
    /// <summary>
    /// Atributo condicional
    /// </summary>
    /// <param name="condicionesPago">string</param>
    private void ValidateCondicionesPago(string? condicionesPago)
    {
        if(string.IsNullOrEmpty(condicionesPago)) return;
        // CondicionesDePago no debe existir cuando TipoDeComprobante es T, P o N.
        if(_tiposComprobanteNoDebeExistir.Contains(TipoComprobante))
            _context.AddWarning(
                section: "Comprobante",
                message: "CondicionesDePago no debe existir cuando TipoDeComprobante es T, P o N");
            
    }
    
    private async Task ValidateMoneda(string? moneda)
    {
        if (string.IsNullOrEmpty(moneda))
        {
            _context.AddWarning(
                section:"Comprobante", 
                message: "El campo Moneda es requerido");
        }

        // var monedaObject = await _repositoryMoneda.GetAsync("c_moneda", moneda);
        // if (monedaObject == null)
        // {
        //     _context.AddError(
        //         code: "CFDI40113",
        //         section: "Comprobante",
        //         message: "Moneda debe contener un valor del catálogo c_Moneda.");
        //     return;
        // }
        //_context.AddValue("monedaDecimales", monedaObject.decimales);
        // Si TipoDeComprobante es P; Moneda debería tener el valor XXX
        if (TipoComprobante == "P" && moneda != "XXX")
        {
            _context.AddWarning(
                section: "Comprobante",
                message: "Si TipoDeComprobante es P, Moneda debe tener el valor XXX");
            return;
        }

        // Si TipoDeComprobante es N; Moneda debería tener el valor MXN.
        if (TipoComprobante == "N" && moneda != "MXN")
        {
            _context.AddWarning(
                section: "Comprobante",
                message: "Si TipoDeComprobante es N, Moneda debe tener el valor MXN");
            return;
        }
        
        
    }
    
    /// <summary>
    /// Validar que la suma de los importes de los conceptos sea la misma que el subtotal.
    /// </summary>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="FormatException"></exception>
    /// <exception cref="OverflowException"></exception>
    /// <param name="subtotal"></param>
    /// <param name="conceptos"></param>
    private void ValidateSubTotal(string subtotal, List<Concepto>? conceptos)
    {
        if (string.IsNullOrEmpty(subtotal))
        {
            _context.AddWarning(
                section:"Comprobante",
                message: "SubTotal es requerido.");
        }
        var subtotalDecimal = decimal.Parse(subtotal);
        if (TipoComprobante is "T" or "P" && subtotalDecimal != decimal.Zero)
        {
            _context.AddError(
                code: "CFDI40109",
                section: "Comprobante",
                message: "Si el comprobante es T o P el importe debe ser 0, o cero con decimales.");
            return;
        }
        if (conceptos != null)
            foreach (var concepto in conceptos)
            {
                var importe = decimal.Parse(concepto.Importe);
                Subtotal = DecimalOperator.Suma(Subtotal, importe);
            }

        if (Subtotal != subtotalDecimal)
        {
            _context.AddError(
                code: "CFDI40108",
                section: "Comprobante",
                message: "El importe registrado en el campo SubTotal no es igual al redondeo de la suma de los " +
                         "importes de los conceptos registrados.");
        }
        
        // TODO 
        //  - **CFDI40107**:
        //  - El valor de este atributo debe tener hasta la cantidad de decimales que soporte la moneda.
        //  - El valor de este campo SubTotal excede la cantidad de decimales que soporta la moneda.
        
    }
    
    
    private readonly List<string> _tiposComprobanteExistirDescuento = ["I", "E", "N"];

    private void ValidateDescuento(string? descuento, List<Concepto>? conceptos, string subtotal)
    {
        if (conceptos != null)
            foreach (var concepto in conceptos)
            {
                if (string.IsNullOrEmpty(concepto.Descuento)) 
                    continue;
                var descuentoConcepto = decimal.Parse(concepto.Descuento);
                Descuento = DecimalOperator.Suma(Descuento, descuentoConcepto);

            }
        // Si TipoDeComprobante es I, E o N y algún concepto incluye un descuento agregar error
        if (_tiposComprobanteExistirDescuento.Contains(TipoComprobante) && Descuento != decimal.Zero && string.IsNullOrEmpty(descuento))
        {
            _context.AddError(
                code: "CFDI40111",
                section: "Comprobante",
                message: "El TipoDeComprobante es I, E o N, y algun concepto incluye el campo Descuento, por lo que es " +
                         "obligatorio agregar el campo Descuento");
            return;
        }
        if (Descuento > Subtotal)
        {
            _context.AddError(
                code: "CFDI40110",
                message: "El valor de Descuento debe ser menor o igual que el atributo Subtotal.",
                section: "Comprobante");
        }
        
        // TODO
        //  - **CFDI40112**:
        //  - El valor de este atributo debe tener hasta la cantidad de decimales que soporte la moneda.
        //   - El valor del campo Descuento excede la cantidad de decimales que soporta la moneda.
    }
    
    
    
    private void ValidateTipoCambio(string? tipoCambio, string? moneda)
    {
        // TipoCambio debe existir cuando Moneda es distinto a "MXN" o "XXX"
        if ((moneda != "MXN" && moneda != "XXX") && string.IsNullOrEmpty(tipoCambio))
        {
            _context.AddError(
                code: "CFDI40115",
                section: "Comprobante",
                message: "El campo TipoCambio se debe registrar cuando el campo Moneda tiene un valor distinto " +
                         "de MXN y XXX.");
            return;
        }
        // Si Moneda es MXN, puede omitirse el atributo TipoCambio, pero si se incluye, debe tener el valor 1
        if (moneda is "MXN" && !string.IsNullOrEmpty(tipoCambio) && tipoCambio != "1")
        {
            _context.AddError(
                code: "CFDI40114",
                section: "Comprobante",
                message: $"Si Moneda es MXN, puede omitirse el atributo TipoCambio, pero si se incluye, debe tener el " +
                         $"valor 1. Valor actual de TipoCambio: '{tipoCambio}'");
            return;
        }
        if(string.IsNullOrEmpty(tipoCambio)) return;
        
        if (!RegexCatalog.IsTipoCambioValid(tipoCambio))
        {
            _context.AddError(
                code: "CFDI40117",
                section: "Comprobante",
                message: "El atributo TipoCambio debe cumplir con el patrón [0-9]{1,18}(.[0-9]{1,6}).");
        }
        
        // TODO
        //  **CFDI40118**:
        //  - Si el valor de este atributo está fuera del porcentaje aplicable a la moneda tomado del catálogo c_Moneda, el emisor debe obtener del PAC que vaya a timbrar el CFDI, de manera no automática, una clave de confirmación para ratificar que el valor es correcto e integrar dicha clave en el atributo Confirmacion.
        //    - Cuando el valor del campo TipoCambio se encuentre fuera de los límites establecidos, debe existir el campo Confirmacion.
        
    }
    
    private void ValidateTotal(Comprobante40 comprobante)
    {
        if (!decimal.TryParse(comprobante.Total, out var totalComprobante))
        {
            _context.AddError(
                code: "CFDI40999",
                section: "Comprobante",
                message: "El campo Total no es valido.");
            return;
        }
            
        if ((TipoComprobante is "P" or "T") && totalComprobante != decimal.Zero)
        {
            _context.AddWarning(
                section: "Comprobante",
                message: "Cuando TipoDeComprobante es P o T, Total debe ser 0.");
        }

        var totalImpuestosTraslados = comprobante.Impuestos?.TotalImpuestosTrasladados ?? "0";
        var totalImpuestosRetenidos = comprobante.Impuestos?.TotalImpuestosRetenidos ?? "0";
        
        var totalDecimal = Subtotal - Descuento + decimal.Parse(totalImpuestosTraslados) - 
                           decimal.Parse(totalImpuestosRetenidos);

        if (comprobante.Complemento is { ImpuestosLocales.Count: > 0 })
        {
            var impLocales = comprobante.Complemento?.ImpuestosLocales[0];
            var totalLocalTraslado = impLocales?.TotalTraslados;
            var totalLocalRetenido = impLocales?.TotalRetenciones;
            totalDecimal = totalDecimal + decimal.Parse(totalLocalTraslado ?? "0") - decimal.Parse(totalLocalRetenido ?? "0");
        }

        if (totalComprobante != totalDecimal)
        {
            _context.AddError(
                section: "Comprobante",
                code: "CFDI40119",
                message: "El campo Total no corresponde con la suma del subtotal, menos los descuentos aplicables, más " +
                         "las contribuciones recibidas (impuestos trasladados - federales o locales, derechos, productos, " +
                         "aprovechamientos, aportaciones de seguridad social, contribuciones de mejoras) menos los " +
                         $"impuestos retenidos y/o locales. Valor actual: {totalComprobante}, valor esperado: {totalDecimal}");
        }
        
        // TODO
        //  **CFDI40120**:
        //  - Si el valor es superior al límite que establezca el SAT en la Resolución Miscelánea Fiscal vigente, el
        //  emisor debe obtener del PAC que vaya a timbrar el CFDI, de manera no automática, una clave de confirmación
        //  para ratificar que el valor es correcto e integrar dicha clave en el atributo Confirmacion.


    }
    
    private void ValidateTipoComprobante(Comprobante40 comprobante)
    {
        if (!CatalogosComprobante.c_TipoDeComprobante.Contains(comprobante.TipoComprobante ?? string.Empty))
        {
            _context.AddError(
                code: "CFDI40121",
                message: "TipoDeComprobante debe contener un valor del catálogo c_TipoDeComprobante",
                section: "Comprobante");
            return;
        }
        // CondicionesDePago: No debe existir el campo CondicionesDePago cuando el campo TipoDeComprobante es
        // T (Traslado), P (Pago) o N (Nómina).
        if ((TipoComprobante is "P" or "T" or "N") && !string.IsNullOrEmpty(comprobante.CondicionesPago))
        {
            _context.AddWarning(
                section: "Comprobante",
                message: "No debe existir el campo CondicionesDePago cuando el campo TipoDeComprobante es T (Traslado), " +
                         "P (Pago) o N (Nómina).");
        }
        // Descuento: No debe existir el campo Descuento de los conceptos cuando el campo TipoDeComprobante es
        // T (Traslado) o P (Pago).
        if ((TipoComprobante is "P" or "T") && !string.IsNullOrEmpty(comprobante.Descuento))
        {
            _context.AddWarning(
                section: "Comprobante",
                message: "No debe existir el campo Descuento de los conceptos cuando el campo TipoDeComprobante es T " +
                         "(Traslado) o P (Pago).");
        }
        // No debe existir el nodo Impuestos cuando el campo TipoDeComprobante es T (Traslado), P (Pago) o N (Nómina).
        if ((TipoComprobante is "P" or "T" or "N") && comprobante.Impuestos != null)
        {
            _context.AddWarning(
                section: "Comprobante",
                message: "No debe existir el nodo Impuestos cuando el campo TipoDeComprobante es T (Traslado), P (Pago) " +
                         "o N (Nómina).");
        }
        
        
    }

    private void ValidateExportacion(Comprobante40 comprobante)
    {
        if (!CatalogosComprobante.c_Exportacion.Contains(comprobante.Exportacion))
        {
            _context.AddError(
                section: "Comprobante",
                code: "CFDI40123",
                message: "El campo Exportación no contiene un valor del catálogo c_Exportacion.");
            return;
        }
        
        if(comprobante.Complemento == null) return;
        
        if (comprobante.Complemento.ComercioExterior is {Count: 0} && comprobante.Exportacion == "02")
        {
            _context.AddError(
                section: "Comprobante",
                code: "CFDI40122",
                message: "El campo Exportación contiene el valor 02, el CFDI debe contener el complemento para Comercio Exterior.");
            return;
        }
    }
    
    private void ValidateMetodoPago(string? metodoPago)
    {
        if (string.IsNullOrEmpty(metodoPago)) return;
        // Debe contener un valor del catálogo c_MetodoPago.
        if (!CatalogosComprobante.c_MetodoPago.Contains(metodoPago))
        {
            _context.AddError(
                section: "Comprobante",
                code: "CFDI40124",
                message: "El campo MetodoPago, no contiene un valor del catálogo c_MetodoPago.");
            return;
        }
        // Se debe omitir el atributo MetodoPago cuando el TipoDeComprobante es T o P.
        if ((TipoComprobante is "P" or "T") && !string.IsNullOrEmpty(metodoPago))
        {
            _context.AddError(
                section: "Comprobante",
                code: "CFDI40125",
                message: "Se debe omitir el atributo MetodoPago cuando el TipoDeComprobante es T o P.");
            return;
        }
    }
    
    private void ValidateLugarExpedicion(string lugarExpedicion)
    {
        // TODO
        //  **CFDI40126**:
        //  - Este atributo, debe contener un valor del catálogo **c_CodigoPostal**.
        _context.AddValue("lugarExpedicion", lugarExpedicion);
    }

    private void ValidateConfirmacion(string? confirmacion)
    {
        // TODO
        //  **Total**: Cuando el valor equivalente en **MXN** (Peso Mexicano) de este campo exceda el límite
        //  establecido, debe existir el campo **Confirmacion**.
        //  : **Confirmacion**: Si el valor está fuera del porcentaje aplicable a la moneda tomado del catálogo
        //  : **c_Moneda**, el emisor debe obtener del proveedor de certificación de CFDI que vaya a timbrar el CFDI de
        //  manera no automática, una clave de confirmación para ratificar que el valor es correcto e integrar dicha
        //  clave en el campo Confirmacion.
    }
    
    private void ValidateComplemento(Complemento? complemento)
    {
        if(complemento == null) return;
        if(complemento.ComercioExterior is { Count: > 0 })
            _context.AddValue("HasComercioExterior", "true");
        if(complemento.CartaPorte is { Count: > 0 })
            _context.AddValue("HasCartaPorte", "true");
    }
    
    private void ValidateInformacionGlobal(Comprobante40 comprobante)
    {
        if (comprobante is { TipoComprobante: "I", Receptor: { Rfc: "XAXX010101000", Nombre: "PUBLICO EN GENERAL" }, InformacionGlobal: null })
        {
            _context.AddError(
                code: "CFDI40130",
                section: "Comprobante -> InformacionGlobal",
                message: "Cuando el tipo de comprobante sea Ingreso y el campo Rfc del nodo receptor corresponda al " +
                         "valor 'XAXX010101000' y el campo Nombre del nodo Receptor contenga la descripción “PUBLICO " +
                         "EN GENERAL”, el nodo Información Global debe existir.");
            return;
        }
    }
    
}