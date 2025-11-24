using KPac.Application.Validator;
using KpacModels.Shared.Models.Comprobante;
using KpacModels.Shared.XmlProcessing.Validator.Catalogos;

namespace KpacModels.Shared.XmlProcessing.Validator.Comprobante.ImpuestosValidate;

public class TrasladosValidator
{
    // Bandera para guardar si los Traslados son solo Exentos
    private readonly bool _soloExentos;
    // Contexto global para el validador
    private ValidatorContext _context;
    // Lista para ir registrando los impuestos y validar que solo se agregue un tipo de impuesto
    private List<string> _impuestoRegistrado = [];
    // Helper para obtener el total de Importes según su tipo de Impuesto
    private ImpuestosHelper _impuestosHelper;
    // Número de decimales que tiene la moneda
    private int _monedaDecimales;
    // Suma de importes Totales
    private decimal _totalTraslados;
    // Variable para guardar la sección donde se está realizando la validación
    private string _section = string.Empty;
    public TrasladosValidator(ValidatorContext context)
    {
        _context = context;
        _impuestosHelper = new ImpuestosHelper(context);
        // Valor obtenido desde ImpuestosConcepto, para validar si se registraron traslados solo exentos
        // Si es nulo solo hay traslados exentos.
        _soloExentos = (context.GetValue("soloExentos") == null);
        var monedaDecimalesString = _context.GetValue("monedaDecimales");
        _monedaDecimales = int.Parse(monedaDecimalesString ?? "0");
    }
    public void ValidateTraslados(List<ImpuestoT>? traslados, string? totalImpuestosTrasladados)
    {
        // Si _soloExentos es false (existen traslados diferentes a Exento) y totalImpuestosTraslados es nulo o vació
        // Agregar error CFDI40212
        if (!_soloExentos && string.IsNullOrEmpty(totalImpuestosTrasladados))
        {
            _context.AddError(
                code: "CFDI40212",
                section: "Comprobante -> Impuestos",
                message: "Debe existir el atributo TotalImpuestosTrasladados, cuando existan conceptos con un TipoFactor" +
                         " distinto a Exento. ");
            return;
        }

        if ((traslados is null || traslados.Count == 0) && _context.CountTraslados() > 0)
        {
            _context.AddWarning(
                section: "Comprobante -> Impuestos",
                message: "Existen Traslados de Impuestos Concepto que no estan registrados en Impuestos.");
            return;
        }
        
        if(traslados is null || traslados.Count == 0) return;
        var count = traslados.Count;
        for (int i = 0; i < count; i++)
        {
            var traslado = traslados[i];
            _section = $"Comprobante -> Impuestos -> {i + 1}. Traslado";
            ValidateTrasladoSingle(traslado);
        }
        // Validar en la lista de traslados este vacía
        // Si tiene valores quiere decir que hay Traslados Concepto que no están registradas en Impuestos
        if (_context.CountTraslados() > 0)
        {
            _context.AddWarning(
                section: "Comprobante -> Impuestos",
                message: "Existen Traslados de Impuestos Concepto que no estan registrados en Impuestos.");
            return;
        }
    }

    private void ValidateTrasladoSingle(ImpuestoT traslado)
    {
        
        var trasladoValidation = _impuestosHelper.GetTraslado(traslado);
        
        //Validar 
        if (trasladoValidation == null)
        {
            _context.AddWarning(
                section: _section,
                message: "El impuesto declarado no existe en el nodo Impuestos Concepto.");
            return;
        }

        ValidateUniqueImpuesto(traslado);
        ValidateBase(traslado.Base, trasladoValidation.GetBaseTotal(_monedaDecimales));
        ValidateImpuesto(traslado.Impuesto);
        ValidateTipoFactor(traslado.TipoFactor);
        
        if (traslado.TipoFactor == "Exento")
        {
            // Si el traslado tiene el TipoFactor 'Exento' solo deben existir los atributos Base, Impuesto y TipoFactor.
            if (!string.IsNullOrWhiteSpace(traslado.TasaOCuota) && !string.IsNullOrWhiteSpace(traslado.Importe))
            {
                _context.AddError(
                    code: "CFDI40213",
                    section: _section,
                    message: "En el caso de que sólo existan conceptos Traslado con TipoFactor Exento, en este nodo solo deben" +
                             " existir los atributos Base, Impuesto y TipoFactor.");
                return;
            }
        }
        else
        {
            ValidateTasaOCuota(traslado);
            ValidateImporte(traslado.Importe, trasladoValidation.GetImporteTotal(_monedaDecimales));
        }
        
    }

    private void ValidateUniqueImpuesto(ImpuestoT impuesto)
    {
        // Crear key para validar que exista solo un registro con la misma combinación de impuesto, factor y tasa por cada traslado.
        var key = $"{impuesto.Impuesto}_{impuesto.TipoFactor}_{impuesto.TasaOCuota}";
        // Debe haber solo un registro por cada tipo de impuesto retenido.
        if (_impuestoRegistrado.Contains(key))
        {
            _context.AddError(
                code: "CFDI40218",
                section: _section,
                message: "Debe haber sólo un registro con la misma combinación de impuesto, factor y tasa por cada traslado.");
            return;
        }
        _impuestoRegistrado.Add(key); 
    }


    private void ValidateBase(string? baseString, decimal baseTotal)
    {
        // Base es un atributo requerido
        if (string.IsNullOrWhiteSpace(baseString))
        {
            _context.AddError(
                code: "CFDI40999",
                section: _section,
                message: "El campo Base es requerido, no puede estar vació.");
            return;
        }
        var @base = decimal.Parse(baseString ?? "0");
        var countDecimals = ValidateHelper.CountDecimalPlaces(@base);
        if (countDecimals > _monedaDecimales)
        {
            _context.AddError(
                code: "CFDI40214",
                section: _section,
                message: $"El valor de Base debe tener hasta la cantidad de decimales que soporte la moneda. Valor" +
                         $" registrado {baseString}. Decimales permitidos por la moneda {_monedaDecimales}.");
            return;
        }
        
        if (@base != baseTotal)
        {
            _context.AddError(
                code: "CFDI40216",
                section: _section,
                message: "El importe del campo Base correspondiente a Traslado no es igual al redondeo de la suma de" +
                         $" los importes de las bases trasladados registrados en los conceptos. Valor registrado {@base}." +
                         $" Valor esperado {baseTotal}.");
            return;
        }
    }

    private void ValidateImpuesto(string impuesto)
    {
        // Validar Impuesto
        if (!CatalogosComprobante.c_Impuesto.ContainsKey(impuesto))
        {
            _context.AddError(
                code: "CFDI40217",
                section: _section,
                message: "El campo Impuesto no contiene un valor del catálogo c_Impuesto.");
            return;
        }
        if (impuesto == "ISR")
        {
            _context.AddError(
                code: "CFDI40999",
                section: _section,
                message: "El Impuesto '001' correspondiente a 'ISR' no esta permitido en el nodo Traslado");
            return;
        }
        
    }

    private void ValidateTasaOCuota(ImpuestoT traslado)
    {
        var tasaOCuotaString = traslado.TasaOCuota;
        if(string.IsNullOrEmpty(tasaOCuotaString)) return;
        var impuesto = CatalogosComprobante.c_Impuesto[traslado.Impuesto];
        var tasaOCuotaList = ValidateHelper.GetListTasaOCuota(impuesto, traslado: true);
        var tasaOCuota = decimal.Parse(tasaOCuotaString);
        if (tasaOCuotaList.Count > 0 && traslado is { TasaOCuota: not null, TipoFactor: not null } &&
            !ValidateHelper.ExistTasaOCuota(
                tasaOCuotaList, 
                tasaOCuota,
                traslado.TipoFactor))
        {
            _context.AddError(
                code: "CFDI40219",
                section: _section,
                message: "El valor del campo TasaOCuota que corresponde a Traslado, no contiene un valor del catálogo" +
                         " c_TasaOcuota o se encuentra fuera de rango.");
            return;
        }
    }
    
    private void ValidateTipoFactor(string? tipoFactor)
    {
        if (string.IsNullOrEmpty(tipoFactor))
        {
            _context.AddWarning(
                section: _section,
                message: "El campo TipoFactor no puede ser nulo ni vacio.");
            return;
        }

        if (!CatalogosComprobante.c_TipoFactor.Contains(tipoFactor))
        {
            _context.AddWarning(
                section: _section,
                message: $"El campo TipoFactor no contiene ningun valor del catalogo c_TipoFactor. Valor registrado {tipoFactor}");
            return;
        }
    }
    
    private void ValidateImporte(string? importeString, decimal importeTotal)
    {
        if (string.IsNullOrEmpty(importeString))return;
        var importe = decimal.Parse(importeString);
        var decimalesImporte = ValidateHelper.CountDecimalPlaces(importe);
        if (decimalesImporte > _monedaDecimales)
        {
            _context.AddError(
                code: "CFDI40220",
                section: _section,
                message: "El valor del campo Importe correspondiente a Traslado debe tener hasta la cantidad de" +
                         $" decimales que soporte la moneda. Valor registrado {importeString} con {decimalesImporte} " +
                         $"decimales. La moneda soporta solo hasta {_monedaDecimales} decimales.");
            return;
        }

        if (importe != importeTotal)
        {
            _context.AddError(
                code: "CFDI40221",
                section: _section,
                message: "El campo Importe correspondiente a Traslado no es igual al redondeo de la suma de los " +
                         "importes de los impuestos trasladados registrados en los conceptos donde el impuesto del " +
                         "concepto sea igual al campo impuesto de este elemento y la TasaOCuota del concepto sea igual" +
                         $" al campo TasaOCuota de este elemento. Valor registrado {importe}. Valor esperado {importeTotal}.");
            return;
        }
        _totalTraslados = DecimalOperator.Suma(_totalTraslados, importe);
    }
 
}