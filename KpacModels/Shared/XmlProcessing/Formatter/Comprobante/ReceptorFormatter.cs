using KPac.Application.Formatter;
using KpacModels.Shared.Models.Comprobante;

namespace KpacModels.Shared.XmlProcessing.Formatter.Comprobante;

public class ReceptorFormatter
{
    
    private const string RfcGenericoNacional = "XAXX010101000";
    private const string RfcGenericoExtranjero = "XEXX010101000";
    private bool _hasInformacionGlobal;
    private Receptor _receptor;
    private string _lugarExpedicion;
    private FormatContext _context;
    
    public ReceptorFormatter(FormatContext context)
    {
        _context = context;
    }

    
    public void Format(Receptor receptor)
    {
        _receptor = receptor;
        _hasInformacionGlobal = _context.GetValue("hasInformacionGlobal") != null;
        _lugarExpedicion = _context.GetValue("lugarExpedicion") ?? string.Empty;
        FormatRfc();
        FormatNombre();
        FormatDomicilioFiscal();
        FormatResidenciaFiscal();
        FormatNumRegIdTrib();
        FormatRegimenFiscal();
        FormatUsoCfdi();
    }
    
    private void FormatRfc()
    {
        if (_hasInformacionGlobal)
        {
            _receptor.Rfc = "XAXX010101000";
            return;
        }
    }

    private void FormatNombre()
    {
        if (_hasInformacionGlobal)
        {
            _receptor.Nombre = "PUBLICO EN GENERAL";
            return;
        }
    }

    private void FormatDomicilioFiscal()
    {
        // Si el nodo InformacionGlobal está presente, se debe registrar el mismo valor que el campo LugarExpedicion.
        if (_hasInformacionGlobal)
        {
            _receptor.DomicilioFiscal = _lugarExpedicion;
            return;
        }
        //  Si el RFC es genérico nacional o extranjero, se debe registrar el mismo valor que el campo LugarExpedicion.
        if (_receptor.Rfc is RfcGenericoNacional or RfcGenericoExtranjero)
        {
            _receptor.DomicilioFiscal = _lugarExpedicion;
            return;
        }
    }

    private void FormatResidenciaFiscal()
    {
        // TODO
        //  1. Este campo es obligatorio cuando el **RFC** del receptor es un *RFC genérico extranjero*, y se incluya el complemento de *comercio exterior*.
        //  2. Este campo es obligatorio cuando se registre el campo **NumRegIdTrib**.
        //  3. Si el nodo **InformacionGlobal** está presente, este campo no debe existir.
    }

    private void FormatNumRegIdTrib()
    {
        // TODO
        //  1. Este campo es obligatorio cuando se incluya el complemento de comercio exterior.
        //  2. Si no existe el campo **ResidenciaFiscal**, este campo puede no existir.
        //  3. Si el nodo **InformacionGlobal** está presente, este campo no debe existir.
    }

    private void FormatRegimenFiscal()
    {
        // TODO
        //  1. Cuando se trate de operaciones con residentes en el extranjero y se registre el valor *“XEXX010101000”* en
        //     este campo se debe registrar la clave *“616”* Sin obligaciones fiscales.
        //  2. Si el nodo **InformacionGlobal** está presente, este campo debe tener el valor *“616”*.
    }

    private void FormatUsoCfdi()
    {
        // TODO
        //  En el caso de que se emita un CFDI a un residente en el extranjero con RFC genérico (*XEXX010101000*), en este campo se debe registrar la clave *“S01”* (Sin efectos fiscales).
    }

    
}