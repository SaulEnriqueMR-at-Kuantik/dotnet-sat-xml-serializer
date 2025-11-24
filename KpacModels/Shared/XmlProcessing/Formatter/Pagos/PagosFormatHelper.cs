using KPac.Application.Formatter;
using KpacModels.Shared.Models.Comprobante.Complementos.Pagos;
using RegexCatalog = KpacModels.Shared.XmlProcessing.Validator.RegexCatalog;

namespace KpacModels.Shared.XmlProcessing.Formatter.Pagos;

public class PagosFormatHelper
{
    public static string GenerateFechaPago()
    {
        var hoy = DateTime.Today;
        return new DateTime(hoy.Year, hoy.Month, hoy.Day, hour: 12, minute: 0, second: 0).ToString("yyyy-MM-ddTHH:mm:ss");
    }
    
    public static void ValidateCuentaOrdenante(string cuentaOrdenante, string formaPago, FormatContext context, string section)
    {
        var message =
            $"El campo CuentaOrdenante no cumple con el patrón requerido. Patrón del catalogo c_FormaPago según la forma de pago {formaPago}:";
        switch (formaPago)
        {
            case "02":
                // Cheque Nominativo
                if (!RegexCatalog.IsChequeNominativoValid(cuentaOrdenante))
                {
                    context.AddError(
                        section: section, 
                        message: $"{message} cadena con 11 o 18 dígitos." );
                }
                break;
            case "03":
                // Transferencia de fondos
                if (!RegexCatalog.IsTransferenciaFondosValid(cuentaOrdenante))
                {
                    context.AddError(
                        section: section, 
                        message: $"{message} cadena con 10, 16 o 18 dígitos." );
                }
                break;
            case "04" or "28":
                // Tarjeta de 16 dígitos
                if(!RegexCatalog.IsTarjeta16DigitsValid(cuentaOrdenante))
                {
                    context.AddError(
                    section: section, 
                    message: $"{message} cadena con 16 dígitos." );
                }
                break;
            case "05":
                // Monedero electrónico
                if(!RegexCatalog.IsMonederoElectronicoValid(cuentaOrdenante)){
                    context.AddError(
                        section: section, 
                        message: $"{message} cadena con 10, 11, 15, 16 o 18 dígitos, o cadena de 10 a 50 caracteres alfanuméricos." );
                }
                break;
            case "06":
                // Dinero electrónico
                if(!RegexCatalog.IsDineroElectronicoValid(cuentaOrdenante)){
                    context.AddError(
                        section: section, 
                        message: $"{message} cadena con 10 dígitos." );
                }
                break;
            case "29":
                // Tarjeta de servicios
                if(!RegexCatalog.IsTarjetaServicioValid(cuentaOrdenante)){
                    context.AddError(
                        section: section, 
                        message: $"{message} cadena con 15 o 16 dígitos." );
                }
                break;
        }
    }
    
    public static void ValidateCuentaBeneficiario(string cuentaOrdenante, string formaPago,
        FormatContext context, string section)
    {
        var message =
            $"El campo CuentaBeneficiario no cumple con el patrón requerido. Patrón del catalogo c_FormaPago según la forma de pago {formaPago}:";
        switch (formaPago)
        {
            case "02" or "04" or "05" or "28" or "29":
                // Monedero electrónico
                if (!RegexCatalog.IsMonederoElectronicoValid(cuentaOrdenante))
                {
                    context.AddError(
                        section: section, 
                        message: $"{message} cadena con 10, 11, 15, 16 o 18 dígitos, o cadena de 10 a 50 caracteres alfanuméricos." );
                }
                break;
            case "03":
                // Transferencia de fondos electrónicos
                if (!RegexCatalog.IsTransferenciaFondosElectronicosValid(cuentaOrdenante))
                {
                    context.AddError(
                        section: section, 
                        message: $"{message} cadena con 10 o 18 dígitos." );
                }
                break;
        }
        
    }

    public static void FiltrarImpuestos(ImpuestosDR? impuestos, string objetoImpuesto)
    {
        if (impuestos == null) return;

        List<string>? impuestosAExcluir = objetoImpuesto switch
        {
            "06" or "08" => ["002", "003"],
            "07" => ["002"],
            _ => null
        };

        if (impuestosAExcluir == null)
            return;

        impuestos.Retenciones?.RemoveAll(r => impuestosAExcluir.Contains(r.Impuesto));
        impuestos.Traslados?.RemoveAll(t => impuestosAExcluir.Contains(t.Impuesto));
    }

    public static decimal CalculateMonto(string montoString, decimal tipoCambio)
    {
        var monto = decimal.Parse(montoString);
        return monto *  tipoCambio;
    }
}