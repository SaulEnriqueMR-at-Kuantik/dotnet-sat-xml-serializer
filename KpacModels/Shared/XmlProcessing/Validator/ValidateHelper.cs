using System.Globalization;
using KpacModels.Shared.XmlProcessing.Validator.Catalogos;

namespace KpacModels.Shared.XmlProcessing.Validator;

public class ValidateHelper
{
    /// <summary>
    /// Count the number of decimal places
    /// </summary>
    /// <param name="value">decimal value</param>
    /// <returns>Number of decimal places</returns>
    /// <example>
    /// input: 34.788m
    /// return: 3
    /// input: 34.001m
    /// return: 3
    /// input: 12.3
    /// return: 1
    /// </example>
    public static int CountDecimalPlaces(decimal value)
    {
        int[] bits = decimal.GetBits(value);
        int scale = (bits[3] >> 16) & 0x7F;
        return scale;
    }

    public static decimal StringToDecimal(string value)
    {
        return decimal.Parse(value, CultureInfo.InvariantCulture);
    }
    
    /// <summary>
    /// Validar que la TasaOCuota de un Impuesto exista en el catálogo c_TasaOCuota.
    /// Una TasaOCuota válida debe estar presente en el c_TasaOCuota.
    /// <see href="https://www.cfdi.org.mx/wp-content/uploads/2020/02/Tabla-de-Valores-de-Tasa-o-Cuota.png?ezimgfmt=ng:webp/ngcb1">Cátalogo con las reglas</see>
    /// Debera coincidir:
    ///  - La TasaOCuota debe encontrarse entre el valor mínimo o máximo (también puede ser un valor fijo)
    /// - Tipo Factor
    /// - Impuesto
    /// </summary>
    /// <param name="tasaOCuotaList">Lista con TasaOCuota.</param>
    /// <param name="tasaOCuota">Valor en decimal de la TasaOCuota que validaremos.</param>
    /// <param name="tipoFactor">TipoFactor del Impuesto que validaremos (Tasa, Cuota, Exento).</param>
    /// <returns>True si es una TasaOCuota valida, False si no es valida.</returns>
    public static bool ExistTasaOCuota(List<TasaOCuota> tasaOCuotaList, decimal tasaOCuota, string tipoFactor)
    {
        foreach (var tasaOCuotaSingle in tasaOCuotaList)
        {
            if (tasaOCuotaSingle.Factor != tipoFactor)
                continue;
            if (!tasaOCuotaSingle.IsRango && tasaOCuotaSingle.ValorMaximo == tasaOCuota)
                return true;
                
            if (tasaOCuotaSingle.IsRango)
                if(tasaOCuota <= tasaOCuotaSingle.ValorMaximo && tasaOCuota >= tasaOCuotaSingle.ValorMinimo)
                    return true;
        }
        return false;
    }
    
    // public static async Task<decimal> ParidadDolarDof(int dia, int mes, int year)
    // {
    //     using HttpClient client = new HttpClient();
    //
    //     string url = $"https://dof.gob.mx/indicadores_detalle.php?cod_tipo_indicador=158&dfecha={dia:D2}%2F{mes:D2}%2F{year}&hfecha={dia:D2}%2F{mes:D2}%2F{year}";
    //     
    //     try
    //     {
    //         var response = await Http.GetAsync(url);
    //         //HttpResponseMessage response = await client.GetAsync(url);
    //         //response.EnsureSuccessStatusCode();
    //         string html = response.Content;                                                                         
    //
    //         HtmlDocument doc = new HtmlDocument();
    //         doc.LoadHtml(html);
    //
    //         var lastTd = doc.DocumentNode.SelectSingleNode("//tr[contains(@class, 'Celda 1')]/td[last()]");
    //
    //         return lastTd != null ? decimal.Parse(lastTd.InnerText.Trim()) : throw new Exception("No se encontró el valor.");
    //     }
    //     catch (Exception ex)
    //     {
    //         Console.WriteLine($"❌ Error al obtener la paridad: {ex.Message}");
    //         return 0;
    //     }
    // }

    /// <summary>
    /// Obtener la lista de TasaOCuota, se puede realizar un filtrado si se requieren solo las TasaOCuota que pertenece a Traslados o Retenciones.
    /// Se requiere enviar el Tipo Impuesto para filtrar.
    /// </summary>
    /// <param name="impuesto">Tipo impuesto requerido</param>
    /// <param name="traslado">Solo si requieres solo las TasaOCuota de traslado</param>
    /// <param name="retencion">Solo si requieres solo las TasaOCuota de retención</param>
    /// <returns></returns>
    public static List<TasaOCuota> GetListTasaOCuota(string impuesto, bool traslado = false, bool retencion = false)
    {
        if(traslado)
            return CatalogosComprobante.c_TasaOCuota
                .Where(s => s.Impuesto == impuesto && s.Traslado == traslado)
                .ToList();
        if(retencion)
            return CatalogosComprobante.c_TasaOCuota
                .Where(s => s.Impuesto == impuesto && s.Retencion == retencion)
                .ToList();
        return CatalogosComprobante.c_TasaOCuota
            .Where(s => s.Impuesto == impuesto)
            .ToList();
    }
    
    /// <summary>
    /// Clasifica los errores, sirve para verificar que no se encuentren códigos de que invaliden la relacion del rfc
    /// con los campos del receptor
    /// </summary>
    /// <param name="errores">Lista de errores tipo <see cref="ErrorValidate"/> </param>
    /// <param name="rfc">String Rfc asociado a la lista de errores</param>
    /// <returns>Objeto con atributos booleanos</returns>
    // public static object ClasificarErrores(List<ErrorValidateDto> errores, string rfc)
    // {
    //     var existsRfc = true;
    //     var matchName = true;
    //     var matchZipCode = true;
    //     var matchFiscalRegimen = true;
    //
    //     foreach (var error in errores)
    //     {
    //         if (error.Code is "03" or "04")
    //             throw new ValidateException("We're experiencing issues with our service. Please try again later or contact support if the problem persists.");
    //         
    //         // Si el rfc no se encuentra en la lista de no cancelados en el sat
    //         if (error.Code == "CFDI40143")
    //         {
    //             existsRfc = false;
    //             matchName = false;
    //             matchZipCode = false;
    //             matchFiscalRegimen = false;
    //             break;
    //         }
    //
    //         // Si el nombre no está asociado al rfc
    //         if(error.Code == "CFDI40145")
    //             matchName = false;
    //         
    //         // Si el código postal no está asociado al rfc
    //         if(error.Code == "CFDI40148")
    //             matchZipCode = false;
    //         
    //         // Si el regimen fiscal no está asociado al rfc
    //         if(error.Code == "CFDI40158")
    //             matchFiscalRegimen = false;
    //         
    //     }
    //
    //     return new
    //     {
    //         rfc,
    //         existsRfc,
    //         matchName,
    //         matchZipCode,
    //         matchFiscalRegimen
    //     };
    // }

    /// <summary>
    /// Validar que la cuenta corriente sigua el patrón especificado del catálogo FormaPago acorde a la regla "CRP20225"
    /// </summary>
    /// <param name="cuentaOrdenante">Cuenta ordenante que se validara</param>
    /// <param name="formaPago">Forma de Pago correspondiente</param>
    /// <param name="context">Objeto Context para agregar el error si la cuenta no es válida</param>
    /// <param name="section">Seccion desde donde se está realizando la validación</param>
    public static void ValidateCuentaOrdenante(string cuentaOrdenante, string formaPago, ValidatorContext context, string section)
    {
        var code = "CRP20225";
        var message =
            $"El campo CuentaOrdenante no cumple con el patrón requerido. Patrón del catalogo c_FormaPago según la forma de pago {formaPago}:";
        switch (formaPago)
        {
            case "02":
                // Cheque Nominativo
                if (!RegexCatalog.IsChequeNominativoValid(cuentaOrdenante))
                {
                    context.AddError(
                        code: code, 
                        section: section, 
                        message: $"{message} cadena con 11 o 18 dígitos." );
                }
                break;
            case "03":
                // Transferencia de fondos
                if (!RegexCatalog.IsTransferenciaFondosValid(cuentaOrdenante))
                {
                    context.AddError(
                        code: code, 
                        section: section, 
                        message: $"{message} cadena con 10, 16 o 18 dígitos." );
                }
                break;
            case "04" or "28":
                // Tarjeta de 16 dígitos
                if(!RegexCatalog.IsTarjeta16DigitsValid(cuentaOrdenante))
                {
                    context.AddError(
                    code: code, 
                    section: section, 
                    message: $"{message} cadena con 16 dígitos." );
                }
                break;
            case "05":
                // Monedero electrónico
                if(!RegexCatalog.IsMonederoElectronicoValid(cuentaOrdenante)){
                    context.AddError(
                        code: code, 
                        section: section, 
                        message: $"{message} cadena con 10, 11, 15, 16 o 18 dígitos, o cadena de 10 a 50 caracteres alfanuméricos." );
                }
                break;
            case "06":
                // Dinero electrónico
                if(!RegexCatalog.IsDineroElectronicoValid(cuentaOrdenante)){
                    context.AddError(
                        code: code, 
                        section: section, 
                        message: $"{message} cadena con 10 dígitos." );
                }
                break;
            case "29":
                // Tarjeta de servicios
                if(!RegexCatalog.IsTarjetaServicioValid(cuentaOrdenante)){
                    context.AddError(
                        code: code, 
                        section: section, 
                        message: $"{message} cadena con 15 o 16 dígitos." );
                }
                break;
        }
    }

    public static void ValidateCuentaBeneficiario(string cuentaOrdenante, string formaPago,
        ValidatorContext context, string section)
    {
        var code = "CRP20226";
        
        var message =
            $"El campo CuentaBeneficiario no cumple con el patrón requerido. Patrón del catalogo c_FormaPago según la forma de pago {formaPago}:";
        switch (formaPago)
        {
            case "02" or "04" or "05" or "28" or "29":
                // Monedero electrónico
                if (!RegexCatalog.IsMonederoElectronicoValid(cuentaOrdenante))
                {
                    context.AddError(
                        code: code, 
                        section: section, 
                        message: $"{message} cadena con 10, 11, 15, 16 o 18 dígitos, o cadena de 10 a 50 caracteres alfanuméricos." );
                }
                break;
            case "03":
                // Transferencia de fondos electrónicos
                if (!RegexCatalog.IsTransferenciaFondosElectronicosValid(cuentaOrdenante))
                {
                    context.AddError(
                        code: code, 
                        section: section, 
                        message: $"{message} cadena con 10 o 18 dígitos." );
                }
                break;
        }
    }
}