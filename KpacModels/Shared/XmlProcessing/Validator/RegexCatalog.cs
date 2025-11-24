using System.Text.RegularExpressions;
using KPac.Domain.Constants;

namespace KpacModels.Shared.XmlProcessing.Validator;

public static partial class RegexCatalog
{
    [GeneratedRegex(RegexConsts.Exact10Numbers, RegexOptions.Compiled)]
    private static partial Regex Exact10DigitRegex();

    public static bool IsExact10Digits(string input)
    {
        return Exact10DigitRegex().IsMatch(input);
    }
    
    [GeneratedRegex(RegexConsts.Exact5Numbers, RegexOptions.Compiled)]
    private static partial Regex Exact5DigitRegex();

    public static bool IsExact5Digits(string input)
    {
        return Exact5DigitRegex().IsMatch(input);
    }
    
    [GeneratedRegex(RegexConsts.RfcPattern, RegexOptions.Compiled)]
    private static partial Regex RfcRegex();

    public static bool IsRfcValid(string input)
    {
        return RfcRegex().IsMatch(input);
    }
    
    [GeneratedRegex(RegexConsts.EmailPattern, RegexOptions.Compiled)]
    private static partial Regex EmailRegex();
    
    public static bool IsEmailValid(string input)
    {
        return EmailRegex().IsMatch(input);
    }
    
    [GeneratedRegex(RegexConsts.TipoCambio, RegexOptions.Compiled)]
    private static partial Regex TipoCambio();
    
    public static bool IsTipoCambioValid(string input)
    {
        return TipoCambio().IsMatch(input);
    }
    
    [GeneratedRegex(RegexConsts.NumeroPedimento, RegexOptions.Compiled)]
    private static partial Regex NumeroPedimento();
    
    public static bool IsNumeroPedimentoValid(string input)
    {
        return NumeroPedimento().IsMatch(input);
    }
    

     [GeneratedRegex(RegexConsts.NumberAndLetter, RegexOptions.Compiled)]
     private static partial Regex NumberAndLetter();
     
     public static bool IsOnlyNumberAndLetter(string input)
     {
         return NumberAndLetter().IsMatch(input);
     }
     
     [GeneratedRegex(RegexConsts.StringSat, RegexOptions.Compiled)]
     private static partial Regex StringSat();
     
     public static bool IsStringSatValid(string input)
     {
         return StringSat().IsMatch(input);
     }
     
     
     [GeneratedRegex(RegexConsts.Tarjeta16Digits, RegexOptions.Compiled)]
     private static partial Regex Tarjeta16Digits();
     
     public static bool IsTarjeta16DigitsValid(string input)
     {
         return Tarjeta16Digits().IsMatch(input);
     }
     
     [GeneratedRegex(RegexConsts.TarjetaServicio, RegexOptions.Compiled)]
     private static partial Regex TarjetaServicio();
     
     public static bool IsTarjetaServicioValid(string input)
     {
         return TarjetaServicio().IsMatch(input);
     }
     
     [GeneratedRegex(RegexConsts.ChequeNominativo, RegexOptions.Compiled)]
     private static partial Regex ChequeNominativo();
     
     public static bool IsChequeNominativoValid(string input)
     {
         return ChequeNominativo().IsMatch(input);
     }
     
     [GeneratedRegex(RegexConsts.TransferenciaFondos, RegexOptions.Compiled)]
     private static partial Regex TransferenciaFondos();
     
     public static bool IsTransferenciaFondosValid(string input)
     {
         return TransferenciaFondos().IsMatch(input);
     }
     
     [GeneratedRegex(RegexConsts.MonederoElectronico, RegexOptions.Compiled)]
     private static partial Regex MonederoElectronico();
     
     public static bool IsMonederoElectronicoValid(string input)
     {
         return MonederoElectronico().IsMatch(input);
     }
     
     [GeneratedRegex(RegexConsts.DineroElectronico, RegexOptions.Compiled)]
     private static partial Regex DineroElectronico();
     
     public static bool IsDineroElectronicoValid(string input)
     {
         return DineroElectronico().IsMatch(input);
     }
     
     [GeneratedRegex(RegexConsts.TransferenciaFondosElectronicos, RegexOptions.Compiled)]
     private static partial Regex TransferenciaFondosElectronicos();

     public static bool IsTransferenciaFondosElectronicosValid(string input)
     {
         return TransferenciaFondosElectronicos().IsMatch(input);
     }
     
     [GeneratedRegex(RegexConsts.IdDocumentoPattern, RegexOptions.Compiled)]
     private static partial Regex IdDocumento();

     public static bool IsIdDocumentoValid(string input)
     {
         return IdDocumento().IsMatch(input);
     }
     
     [GeneratedRegex(RegexConsts.Date, RegexOptions.Compiled)]
     private static partial Regex Date();

     public static bool IsDateValid(string input)
     {
         return Date().IsMatch(input);
     }
     
     [GeneratedRegex(RegexConsts.CurpPattern, RegexOptions.Compiled)]
     private static partial Regex Curp();

     public static bool IsCurpValid(string input)
     {
         return Curp().IsMatch(input);
     }
    
     
     [GeneratedRegex(RegexConsts.Iso8601Short, RegexOptions.Compiled)]
     private static partial Regex Iso8601s();

     public static bool IsIso8601ShortValid(string input)
     {
         return Iso8601s().IsMatch(input);
     }
     
     [GeneratedRegex(RegexConsts.Iso8601, RegexOptions.Compiled)]
     private static partial Regex Iso8601();

     public static bool IsIso8601Valid(string input)
     {
         return Iso8601().IsMatch(input);
     }
     
     [GeneratedRegex(RegexConsts.CuentaBancaria, RegexOptions.Compiled)]
     private static partial Regex CuentaBancaria();

     public static bool IsCuentaBancariaValid(string input)
     {
         return CuentaBancaria().IsMatch(input);
     }

}