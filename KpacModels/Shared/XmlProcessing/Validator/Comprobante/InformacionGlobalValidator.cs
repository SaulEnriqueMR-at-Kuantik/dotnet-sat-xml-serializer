using KPac.Application.Validator;
using KpacModels.Shared.Models.Comprobante;
using KpacModels.Shared.XmlProcessing.Validator.Catalogos;

namespace KpacModels.Shared.XmlProcessing.Validator.Comprobante;

public class InformacionGlobalValidator
{
    private ValidatorContext _context = new();

    private string _periodicidad = string.Empty;
    public void Validate(InformacionGlobal? informacionGlobal, ValidatorContext comprobanteContext)
    {
        _context = comprobanteContext;
        
        
        ValidatePeriodicidad(informacionGlobal.Periodicidad);
        ValidateMeses(informacionGlobal.Meses);
        ValidateAnio(informacionGlobal.Anio);
        
    }

    /// <summary>
    /// Válida que TipoComprobante sea 'I', Rfc Receptor sea genérico nacional y que información global sea diferente de nulo.
    /// </summary>
    /// <param name="informacionGlobal">Nodo Comprobante 4.0</param>
    /// <returns>bool</returns>
    /*private bool ValidateNode(InformacionGlobal? informacionGlobal)
    {
        var tipoComprobante = _context.GetValue("tipoComprobante");
        var rfcReceptor = _context.GetValue("rfcReceptor");
        if(string.IsNullOrEmpty(tipoComprobante) || string.IsNullOrEmpty(rfcReceptor)) return false;


        if (tipoComprobante == "I" && rfcReceptor == "XAXX010101000" && informacionGlobal == null)
        {
            _context.AddError(
                code: "CFDI40130",
                section: "Comprobante -> InformacionGlobal",
                message: "Cuando el tipo de comprobante sea Ingreso y el campo Rfc del nodo receptor corresponda al " +
                         "valor 'XAXX010101000' y el campo Nombre del nodo Receptor contenga la descripción “PUBLICO " +
                         "EN GENERAL”, el nodo Información Global debe existir.");
            return false;
        }
        return true;
    }*/

    private void ValidatePeriodicidad(string periodicidad)
    {
        if (string.IsNullOrEmpty(periodicidad))
        {
            _context.AddWarning(
                section: "Comprobante -> InformacionGlobal",
                message: "Periodicidad debe contener un valor, es requerido.");
            return;
        }

        if (!CatalogosComprobante.c_Periodicidad.Contains(periodicidad))
        {
            _context.AddError(
                code: "CFDI40131",
                section: "Comprobante -> InformacionGlobal",
                message: $"El campo Periodicidad, no contiene un valor del catálogo c_Periodicidad. Valor registrado: {periodicidad}.");
        }
        
        var regimeFiscalEmisor = _context.GetValue("regimeFiscalEmisor") ?? string.Empty;
        if (regimeFiscalEmisor == "621" && periodicidad != "05")
        {
            _context.AddError(
                code: "CFDI40132",
                section: "Comprobante -> InformacionGlobal",
                message: "Cuando RegimenFiscal de Emisor tiene el valor '621', Periodicidad debe contener el valor '05'");
            return;
        }
        if (regimeFiscalEmisor != "621" && periodicidad == "05")
        {
            _context.AddError(
                code: "CFDI40132",
                section: "Comprobante -> InformacionGlobal",
                message: "Cuando Periodicidad tiene el valor '05', el campo RegimenFiscal de Emisor debe tener el valor '621', ");
            return;
        }
        _periodicidad = periodicidad;
    }

    private void ValidateMeses(string meses)
    {
        if (string.IsNullOrEmpty(meses))
        {
            _context.AddWarning(
                section: "Comprobante -> InformacionGlobal",
                message: "Meses es un campo obligatorio.");
            return;
        }

        if (!CatalogosComprobante.c_Meses.Contains(meses))
        {
            _context.AddError(
                code: "CFDI40133",
                section: "Comprobante -> InformacionGlobal",
                message: "El campo Meses, no contiene un valor del catálogo c_Meses. ");
            return;
        }
        // Cuando el valor del campo Periodicidad sea “05”, este campo debe contener alguno de los valores
        // “13”, “14”, “15”, “16”, “17” o “18”. (Meses bimestrales)
        if (_periodicidad == "05" && !CatalogosComprobante.mesesBimestrales.Contains(meses))
        {
            _context.AddError(
                code: "CFDI40135",
                section: "Comprobante -> InformacionGlobal",
                message: "El atributo Periodicidad contiene el valor “05”, Meses debe contener alguno de los" +
                         " valores “13”, “14”, “15”, “16”, “17” o “18”.");
            return;
        }
        // Si el atributo Periodicidad contiene un valor diferente de “05”, este atributo debe contener alguno de los
        // valores “01”, “02”, “03”, “04”, “05”, “06”, “07”, “08”, “09”, “10”, “11”, “12”.
        if (_periodicidad != "05" && !CatalogosComprobante.meses.Contains(meses))
        {
            _context.AddError(
                code: "CFDI40134",
                section: "Comprobante -> InformacionGlobal",
                message: "El atributo Periodicidad contiene el valor diferente a “05”, Meses debe contener alguno de los" +
                         " valores “01”, “02”, “03”, “04”, “05”, “06”, “07”, “08”, “09”, “10”, “11”, “12”.");
            return;
        }
        
    }

    private void ValidateAnio(string anio)
    {
        if (string.IsNullOrEmpty(anio))
        {
            _context.AddWarning(
                section: "Comprobante -> InformacionGlobal",
                message: "El Año es un campo obligatorio.");
        }

        var listYears = GetFiveYearsBefore();
        if (!listYears.Contains(anio))
        {
            _context.AddError(
                code: "CFDI40136",
                section: "Comprobante -> InformacionGlobal",
                message: "El valor registrado en el campo Año, no es igual al año en curso o no contiene un valor de" +
                         " hasta 5 ejercicios anteriores.");
        }
    }

    /// <summary>
    /// Obtener 5 años anteriores
    /// </summary>
    /// <returns>Lista con los 5 años anteriores</returns>
    private static List<string> GetFiveYearsBefore()
    {
        List<string> listFiveYears = [];
        for (var i = 0; i < 5; i++)
        {
            var year = DateTime.Today.Year - i;
            listFiveYears.Add(year.ToString());
        }
        return listFiveYears;
    }
}