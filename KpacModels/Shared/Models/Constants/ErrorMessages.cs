namespace KPac.Application.Common.Validators;

public class ErrorMessages
{
    public const string Rfc =
        "Formato invalido del RFC. Debe seguir el siguiente patrón: 3-4 letras, YYMMDD, y 3 caracteres alfanumericos.";

    public const string Uuid = "Formato del UUID invalido.";
    
    public const string Motivo = "El código de motivo proporcionado debe tener uno de los siguientes valores: '01', '02', '03', '04'.";

    public const string Email = "El correo electronico ingresado no es valido";

    public const string NumeroPedimento =
        "El número de pedimento es inválido. El formato esperado es 'XX  XX  XXXX  XXXXXXX'." +
        " Ejemplo correcto: '12  12  1234  1234567', Los cuales representan los últimos 2 dígitos del año de validación " +
        "seguidos por dos espacios, 2 dígitos de la aduana de despacho seguidos por dos espacios, 4 dígitos del número de" +
        " la patente seguidos por dos espacios, 1 dígito que corresponde al último dígito del año en curso, salvo que se" +
        " trate de un pedimento consolidado iniciado en el año inmediato anterior o del pedimento original de una rectificación," +
        " seguido de 6 dígitos de la numeración progresiva por aduana.";
}