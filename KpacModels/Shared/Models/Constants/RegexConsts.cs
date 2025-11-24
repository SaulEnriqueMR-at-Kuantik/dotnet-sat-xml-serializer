namespace KPac.Domain.Constants;

public static class RegexConsts
{
    public const string RfcPattern = @"^([A-ZÑ]|&){3,4}[0-9]{2}(0[1-9]|1[0-2])([12][0-9]|0[1-9]|3[01])[A-Z0-9]{3}$";

    public const string UuidPattern =
        @"^[0-9a-fA-F]{8}-[0-9a-fA-F]{4}-[1-5][0-9a-fA-F]{3}-[89abAB][0-9a-fA-F]{3}-[0-9a-fA-F]{12}$";

    public const string IdDocumentoPattern =
        "^([a-fA-F0-9]{8}-[a-fA-F0-9]{4}-[a-fA-F0-9]{4}-[a-fA-F0-9]{4}-[a-fA-F0-9]{12}|[0-9]{3}-[0-9]{2}-[0-9]{9})$";
    
    public const string MotivoPattern = @"^(01|02|03|04)$";

    public const string EmailPattern = @"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$";
    
    public const string Exact10Numbers = @"[0-9]{10}";
    
    public const string Exact5Numbers = @"[0-9]{5}";
    
    public const string TipoCambio = @"^[0-9]{1,18}(?:\.[0-9]{1,6})?$";
    
    public const string NumeroPedimento = @"^\d{2}  \d{2}  \d{4}  \d{7}$";

    public const string NumberAndLetter = "^[0-9a-zA-Z]+$";

    public const string StringSat = "^[^|]$";
    
    public const string Date = @"^\d{4}-(0[1-9]|1[0-2])-(0[1-9]|[12]\d|3[01])$";
    
    public const string CurpPattern = @"^[A-Z][AEIOUX][A-Z]{2}[0-9]{2}(0[1-9]|1[0-2])(0[1-9]|[12][0-9]|3[01])[MHX](?:[ABCMTZ]S|[BCJMOT]C|[CNPST]L|[GNQ]T|[GQS]R|C[MH]|[MY]N|[DH]G|NE|VZ|DF|SP)[BCDFGHJ-NP-TV-Z]{3}[0-9A-Z][0-9]$";

    
    /// Patrones para cuenta ordenante/beneficiario Pago 2.0

    public const string Tarjeta16Digits = @"^\d{16}$";
    
    public const string TarjetaServicio = @"^\d{15,16}$"; 
    
    public const string ChequeNominativo = @"^\d{11}$|^\d{18}$";

    public const string TransferenciaFondos = @"^(?:\d{10}|\d{16}|\d{18})$";
    
    public const string MonederoElectronico = @"^([0-9]{10,11}|[0-9]{15,16}|[0-9]{18}|[A-Z0-9_]{10,50})$";
    
    public const string DineroElectronico = @"^\d{10}$";
    
    public const string TransferenciaFondosElectronicos = @"^(\d{10}|\d{18})$";
    
    // Validar Fechas

    public const string Iso8601Short = @"^\d{4}-(0[1-9]|1[0-2])-(0[1-9]|[12]\d|3[01])$";

    public const string Iso8601 = @"^\d{4}-(0[1-9]|1[0-2])-(0[1-9]|[12]\d|3[01])T([01]\d|2[0-3]):[0-5]\d:[0-5]\d$";

    public const string CuentaBancaria = @"^.{10}$|^.{11}$|^.{16}$|^.{18}$";

}