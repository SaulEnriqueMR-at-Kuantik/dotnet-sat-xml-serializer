using KPac.Application.Validator;
using KpacModels.Shared.Models.Comprobante;
using KpacModels.Shared.XmlProcessing.Validator.Catalogos;

namespace KpacModels.Shared.XmlProcessing.Validator.Comprobante.ImpuestosValidate;

public class RetencionesValidator
{
    // Contexto global para el validador
    private ValidatorContext _context;
    // Suma de importes Totales
    private decimal _totalRetenidos;
    // Número de decimales que tiene la moneda
    private decimal _monedaDecimales;
    // Lista para ir registrando los impuestos y validar que solo se agregue un tipo de impuesto
    private List<string> _impuestoRegistrado = [];
    // Helper para obtener el total de Importes según su tipo de Impuesto
    private ImpuestosHelper _impuestosHelper;
    public RetencionesValidator(ValidatorContext context)
    {
        _context = context;
        _impuestosHelper = new ImpuestosHelper(context);
        var monedaDecimalesString = _context.GetValue("monedaDecimales");
        _monedaDecimales = int.Parse(monedaDecimalesString ?? "0");
    }
    public void ValidateRetenciones(List<ImpuestoR>? retenciones, string? totalImpuestosRetenidos)
    {
        // Sí existen retenciones y no existen totalRetenidos -> CFDI40206
        if (retenciones is { Count: > 0 } && string.IsNullOrEmpty(totalImpuestosRetenidos))
        {
            _context.AddError(
                code: "CFDI40206",
                section: "Comprobante -> Impuestos",
                message: "Tiene registrado retenciones, por lo que debe agregar el campo TotalImpuestosRetenidos " +
                         "para expresar la suma de importes.");
        }
        // si existe totalRetenidos y retenciones es nulo o vació -> CFDI40206
        if (!string.IsNullOrEmpty(totalImpuestosRetenidos) && retenciones is { Count: 0 } )
        {
            _context.AddError(
                code: "CFDI40206",
                section: "Comprobante -> Impuestos",
                message: "Tiene registrado el campo TotalImpuestosRetenidos para expresar la suma de importes, pero" +
                         " no tiene retenciones registradas, debe agregar retenciones o eliminar el valor del campo.");
        }
        if ((retenciones is null || retenciones.Count == 0) && _context.CountRetencion() > 0)
        {
            _context.AddWarning(
                section: "Comprobante -> Impuestos",
                message: "Existen Traslados de Impuestos Concepto que no estan registrados en Impuestos.");
            return;
        }
        if(retenciones == null || retenciones.Count == 0) return;
        var count = retenciones.Count;
        for (int i = 0; i < count; i++)
        {
            var retencion = retenciones[i];
            ValidateRetencionSingle(retencion, i + 1);
        }
        
        // Validar que no tengan valores la lista de totales Retención
        // Si tiene valores quiere decir que hay Retenciones Concepto que no están registradas en Impuestos
        if (_context.CountRetencion() > 0)
        {
            _context.AddWarning(
                section: "Comprobante -> Impuestos",
                message: "Existen Retenciones de Impuestos Concepto que no estan registrados en Impuestos.");
        }
        
        // Validar que _totalRetenciones sea igual a totalRetenidos, si no son iguales -> CFDI40203
        var totalRetenidos = decimal.Parse(totalImpuestosRetenidos ?? "0");
        if (_totalRetenidos != totalRetenidos)
        {
            _context.AddError(
                code: "CFDI40203",
                section: "Comprobante -> Impuestos",
                message: " El valor del campo TotalImpuestosRetenidos debe ser igual a la suma de los importes " +
                         $"registrados en el elemento hijo Retencion. Valor registrado {totalRetenidos}. Valor" +
                         $" esperado {_totalRetenidos}");
        }
        _impuestoRegistrado.Clear();
    }

    private void ValidateRetencionSingle(ImpuestoR retencion, int i)
    {
        var section = $"Comprobante -> Impuestos -> {i}. Retención";
        ValidateImpuesto(retencion, section);
        ValidateImporte(retencion, section);
    }

    private void ValidateImpuesto(ImpuestoR retencion, string section)
    {

        var impuesto = retencion.Impuesto;

        // El campo Impuesto no contiene un valor del catálogo c_Impuesto.
        if (!CatalogosComprobante.c_Impuesto.ContainsKey(impuesto))
        {
            _context.AddError(
                code: "CFDI40207",
                section: section,
                message: "El campo Impuesto no contiene un valor del catálogo c_Impuesto.");
            return;
        }

        // Debe haber solo un registro por cada tipo de impuesto retenido.
        if (_impuestoRegistrado.Contains(impuesto))
        {
            _context.AddError(
                code: "CFDI40208",
                section: section,
                message: "Debe haber sólo un registro por cada tipo de impuesto retenido.");
            return;
        }
        _impuestoRegistrado.Add(impuesto);
    }

    private void ValidateImporte(ImpuestoR retencion, string section)
    {
        var importe = decimal.Parse(retencion.Importe ?? "0");
        
        // El número de decimales de importe debe tener hasta la cantidad de decimales que soporta la moneda.
        var countDecimales = ValidateHelper.CountDecimalPlaces(importe);
        if (countDecimales > _monedaDecimales)
        {
            _context.AddError(
                code: "CFDI40210",
                section: section,
                message: "El valor del campo Importe correspondiente a Retención debe tener hasta la cantidad de " +
                         $"decimales que soporte la moneda. Valor registrado {importe}. Número de decimales de la Moneda {_monedaDecimales}.");
        }
        // Obtener el total de impuestos según su tipo de impuesto.
        var totalImpuestosConcepto = _impuestosHelper.GetRetencion(retencion.Impuesto);
        // Validar que el total de impuesto concepto redondeado al número de decimales que soporta la moneda, sea igual que el importe registrado.
        if (decimal.Parse(totalImpuestosConcepto) != importe)
        {
            _context.AddError(
                code: "CFDI40211",
                section: section,
                message: "El campo Importe correspondiente a Retención no es igual al redondeo de la suma de los " +
                         "importes de los impuestos retenidos registrados en los conceptos donde el impuesto sea igual" +
                         $" al campo impuesto de este elemento. Valor registrado {importe}. Valor esperado {totalImpuestosConcepto}");
        }
        // Sí va bien se suma el importe al Total Retenidos.
        _totalRetenidos = DecimalOperator.Suma(_totalRetenidos, importe);
    }
}