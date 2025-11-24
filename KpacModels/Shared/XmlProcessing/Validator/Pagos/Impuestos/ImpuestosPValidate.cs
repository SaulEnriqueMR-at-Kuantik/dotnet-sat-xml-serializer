using System.Globalization;
using KPac.Application.Validator;
using KpacModels.Shared.Models.Comprobante.Complementos.Pagos;
using KpacModels.Shared.XmlProcessing.Validator.Catalogos;

namespace KpacModels.Shared.XmlProcessing.Validator.Pagos.Impuestos;

public class ImpuestosPValidate
{

    // Lista para ir registrando los impuestos y validar que solo se agregue un tipo de impuesto
    private List<string> _impuestoRegistrados = [];

    private readonly ImpuestosHelper _impuestosHelper;
    
    private readonly ValidatorContext _context;

    private string _section;
    
    private int _decimalesMoneda;
    public ImpuestosPValidate(ValidatorContext context)
    {
        _impuestosHelper = new ImpuestosHelper(context);
        _context = context;
        _decimalesMoneda = int.Parse(context.GetValue("monedaDecimales") ?? "0");
        _section = string.Empty;
    }
    
    public void Validate(ImpuestosP impuestosP, int noPago)
    {
        var retenciones = impuestosP.Retenciones;
        
        if (retenciones != null && retenciones.Count > 0)
        {
            var count = retenciones.Count;
            for (int i = 0; i < count; i++)
            {
                var retencion = retenciones[i];
                _section = $"Comprobante -> Complemento -> Pagos -> {noPago}. Pago -> Impuestos -> {i + 1}. Retención";
                ValidateRetencion(retencion);
            }
        }
        
        // Validar que no tengan valores la lista de totales Retención
        // Si tiene valores quiere decir que hay Retenciones Concepto que no están registradas en Impuestos
        if (_context.CountRetencion() > 0)
        {
            _context.AddWarning(
                section: $"Comprobante -> Complemento -> Pagos -> {noPago}. Pago -> Impuestos",
                message: "Existen Retenciones de Impuestos Concepto que no estan registrados en Impuestos.");
        }
        
        _impuestoRegistrados.Clear();
        
        var traslados = impuestosP.Traslados;
        if (traslados != null && traslados.Count > 0)
        {
            var count = traslados.Count;
            for (int i = 0; i < count; i++)
            {
                var traslado = traslados[i];
                _section = $"Comprobante -> Complemento -> Pagos -> {noPago}. Pago -> Impuestos -> {i + 1}. Traslado";
                ValidateTraslado(traslado);
            }
        }
        _context.ClearRetenciones();
        _context.ClearTraslados();
        
    }
    
    private void ValidateRetencion(RetencionP retencion)
    {
       
        ValidateImpuestoRetencion(retencion);
        ValidateImporteRetencion(retencion);
    }

    private void ValidateImpuestoRetencion(RetencionP retencion)
    {
        // Validar impuesto
        var impuesto = retencion.Impuesto;

        // El campo Impuesto no contiene un valor del catálogo c_Impuesto.
        if (!CatalogosComprobante.c_Impuesto.ContainsKey(impuesto))
        {
            _context.AddError(
                code: "CRP20262",
                section: _section,
                message: "El campo ImpuestoP no contiene un valor del catálogo c_Impuesto.");
            return;
        }

        // Debe haber solo un registro por cada tipo de impuesto retenido.
        if (_impuestoRegistrados.Contains(impuesto))
        {
            _context.AddError(
                code: "CRP20263",
                section: _section,
                message: "Debe haber sólo un registro por cada tipo de impuesto retenido.");
            return;
        }
        _impuestoRegistrados.Add(impuesto);
    }

    private void ValidateImporteRetencion(RetencionP retencion)
    {
        var importe = decimal.Parse(retencion.Importe);
        
        // Obtener el total de impuestos según su tipo de impuesto.
        var totalImpuestosDocto = decimal.Parse(_impuestosHelper.GetRetencion(retencion.Impuesto));
        // Validar que el total de impuesto concepto sea igual que el importe registrado.
        var importeDecimals = ValidateHelper.CountDecimalPlaces(importe);
        var importeTruncate = DecimalOperatorLimites.TruncarDecimal(totalImpuestosDocto, importeDecimals);
        if (importeTruncate != importe)
        {
            _context.AddError(
                code: "CRP20265",
                section: _section,
                message: "El campo Importe correspondiente a Retención no es igual al redondeo de la suma de los " +
                         "importes de los impuestos retenidos registrados en los documentos relacionados donde el impuesto sea igual" +
                         $" al campo ImpuestoP de este elemento. Valor registrado {importe}. Valor esperado {importeTruncate}");
        }
    }

    private void ValidateTraslado(TrasladoP traslado)
    {
        
        var trasladoValidation = _impuestosHelper.GetTraslado(traslado);
        
        //Validar 
        if (trasladoValidation == null)
        {
            _context.AddWarning(
                section: _section,
                message: "El impuesto declarado no existe en el nodo Impuestos Docto Relacionados.");
            return;
        }
        ValidateUniqueImpuestoTraslado(traslado);
        ValidateBaseTraslado(traslado, trasladoValidation.GetBaseTotal());
        ValidateImpuestoTraslado(traslado);
        ValidateTipoFactorTraslado(traslado.TipoFactor);
        
        if (traslado.TipoFactor == "Exento")
        {
            // Si el traslado tiene el TipoFactor 'Exento' solo deben existir los atributos Base, Impuesto y TipoFactor.
            if (!string.IsNullOrWhiteSpace(traslado.TasaOCuota) && !string.IsNullOrWhiteSpace(traslado.Importe))
            {
                _context.AddError(
                    code: "CRP20266",
                    section: _section,
                    message: "En el caso de que sólo existan conceptos Traslado con TipoFactorDR Exento, en este nodo solo deben" +
                             " existir los atributos BaseP, ImpuestoP y TipoFactorP.");
                return;
            }
        }
        else
        {
            ValidateTasaOCuotaTraslado(traslado);
            ValidateImporteTraslado(traslado.Importe, trasladoValidation.GetImporteTotal());
        }

        
        
    }

    private void ValidateUniqueImpuestoTraslado(TrasladoP traslado)
    {
        // Crear key para validar que exista solo un registro con la misma combinación de impuesto, factor y tasa por cada traslado.
        var key = $"{traslado.Impuesto}_{traslado.TipoFactor}_{traslado.TasaOCuota}";
        // Debe haber solo un registro por cada tipo de impuesto traslado.
        if (_impuestoRegistrados.Contains(key))
        {
            _context.AddError(
                code: "CRP20271",
                section: _section,
                message: "Debe haber sólo un registro con la misma combinación de impuesto, factor y tasa por cada traslado.");
            return;
        }
        _impuestoRegistrados.Add(key);
    }

    private void ValidateBaseTraslado(TrasladoP traslado, decimal baseTotal)
    {
        var baseString = traslado.Base;
        if (string.IsNullOrWhiteSpace(baseString))
        {
            _context.AddError(
                code: "CRP20999",
                section: _section,
                message: "El campo BaseP es requerido, no puede estar vació.");
        }
        var @base = decimal.Parse(baseString);
        var baseDecimals = ValidateHelper.CountDecimalPlaces(@base);
        var baseTotalTruncate = DecimalOperatorLimites.TruncarDecimal(baseTotal, baseDecimals);
        if (@base != baseTotalTruncate)
        {
            _context.AddError(
                code: "CRP20268",
                section: _section,
                message: "El importe del campo BaseP correspondiente a TrasladoP no es igual al redondeo de la suma de" +
                         $" los importes de las bases trasladados registrados en los conceptos. Valor registrado {@base}." +
                         $" Valor esperado {baseTotalTruncate}.");
            return;
        }
    }

    private void ValidateImpuestoTraslado(TrasladoP traslado)
    {
        var impuesto = traslado.Impuesto;

        // El campo Impuesto no contiene un valor del catálogo c_Impuesto.
        if (!CatalogosComprobante.c_Impuesto.ContainsKey(impuesto))
        {
            _context.AddError(
                code: "CRP20270",
                section: _section,
                message: "El campo ImpuestoP no contiene un valor del catálogo c_Impuesto.");
            return;
        }
    }
    private void ValidateTipoFactorTraslado(string tipoFactor)
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

    private void ValidateTasaOCuotaTraslado(TrasladoP traslado)
    {

        var tasaOCuotaString = traslado.TasaOCuota;
        if(string.IsNullOrEmpty(tasaOCuotaString)) return;
        var impuesto = CatalogosComprobante.c_Impuesto[traslado.Impuesto];
        var tasaOCuotaList = ValidateHelper.GetListTasaOCuota(impuesto, traslado: true);
        var tasaOCuota = decimal.Parse(tasaOCuotaString, CultureInfo.InvariantCulture);
        if (tasaOCuotaList.Count > 0 && traslado is { TasaOCuota: not null } &&
            !ValidateHelper.ExistTasaOCuota(
                tasaOCuotaList, 
                tasaOCuota,
                traslado.TipoFactor))
        {
            _context.AddError(
                code: "CRP20272",
                section: _section,
                message: "El valor del campo TasaOCuota que corresponde a Traslado, no contiene un valor del catálogo" +
                         " c_TasaOcuota o se encuentra fuera de rango.");
            return;
        }
    }
    
    private void ValidateImporteTraslado(string? importeString, decimal importeTotal)
    {
        if (string.IsNullOrEmpty(importeString))return;
        var importe = decimal.Parse(importeString);
        var importeDecimals = ValidateHelper.CountDecimalPlaces(importe);
        var importeTotalTruncate = DecimalOperatorLimites.TruncarDecimal(importeTotal, importeDecimals);
        if (importe != importeTotalTruncate)
        {
            _context.AddError(
                code: "CRP20274",
                section: _section,
                message: "El campo Importe correspondiente a Traslado no es igual al redondeo de la suma de los " +
                         "importes de los impuestos trasladados registrados en el documento relacionado donde el impuesto del " +
                         "concepto sea igual al campo ImpuestoP de este elemento y la TasaOCuotaP del concepto sea igual" +
                         $" al campo TasaOCuotaP de este elemento. Valor registrado {importe}. Valor esperado {importeTotalTruncate}.");
            return;
        }
    }
}