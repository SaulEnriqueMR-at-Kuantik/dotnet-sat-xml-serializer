using System.Globalization;
using KPac.Application.Formatter;
using KPac.Application.Validator;
using KPac.Application.Validator.Catalogos;
using KpacModels.Shared.Models.Comprobante;
using KpacModels.Shared.XmlProcessing.Validator;
using KpacModels.Shared.XmlProcessing.Validator.Catalogos;

namespace KpacModels.Shared.XmlProcessing.Formatter.Comprobante.ConceptosFormat;

public class ConceptoFormatter
{
    private FormatContext _context;
    private Concepto _concepto = new();
    private int _noConcepto;
    private string _tipoComprobante = string.Empty;

    public ConceptoFormatter(FormatContext context)
    {
        _context = context;
    }
    
    public void Format(Concepto concepto, int noConcepto)
    {
        _tipoComprobante = _context.GetValue("tipoComprobante") ?? string.Empty;
        _concepto = concepto;
        _noConcepto = noConcepto;
        FormatClaveProdServ();
        FormatCantidad();
        FormatValorUnitario();
        FormatImporte();
        FormatDescuento();
        FormatObjetoImp();
        FormatImpuestos();
    }

    

    private void FormatClaveProdServ()
    {
        if (string.IsNullOrEmpty(_concepto.ClaveProdServ))
            _concepto.ClaveProdServ = "01010101";
    }
    
    private void FormatCantidad()
    {
        var cantidad = DecimalOperatorLimites.TruncarDecimal(_concepto.SrcCantidad ?? 0, 6);
        _concepto.Cantidad = cantidad.ToString(CultureInfo.InvariantCulture);
    }
    
    private void FormatValorUnitario()
    {
        if (_tipoComprobante == "P")
        {
            _concepto.ValorUnitario = "0";
            return;
        }

        if (_concepto.SrcValorUnitario != null)
        {
            var valorUnitario = DecimalOperatorLimites.TruncarDecimal(_concepto.SrcValorUnitario ?? 0, 6);
            _concepto.ValorUnitario = valorUnitario.ToString(CultureInfo.InvariantCulture);
        }

    }
    
    private void FormatImporte()
    {
        var valorUnitario = decimal.Parse(_concepto.ValorUnitario);
        var cantidad = decimal.Parse(_concepto.Cantidad);
        var importe = valorUnitario * cantidad;
        var errorMessage = $"El campo Importe del Concepto No. {_noConcepto} no es valido. Favor de revisarlo.";
        _concepto.SrcImporte = importe;
        _concepto.Importe = FormatHelper.ConvertStringToDecimalSatString(importe.ToString(CultureInfo.InvariantCulture), errorMessage);
        _context.AddImporteToSubtotal(importe);
    }
    
    private void FormatDescuento()
    {
         if(_concepto.SrcDescuento is null) 
             return;
         var decimalesImporte = ValidateHelper.CountDecimalPlaces(_concepto.SrcImporte);
         var descuento = DecimalOperatorLimites.TruncarDecimal(_concepto.SrcDescuento ?? decimal.Zero, decimalesImporte);
         _concepto.Descuento = descuento.ToString(CultureInfo.InvariantCulture);
         if(_concepto.Descuento is null)
             return;
         AddDescuento(_concepto.Descuento);
         
    }

    private void FormatObjetoImp()
    {
        //throw new NotImplementedException();
    }

    private void FormatImpuestos()
    {
        // Si el atributo ObjetoImpuesto es "01", "03", "04" o "05" el nodo hijo Impuestos del nodo concepto no debe existir.
        if (CatalogosComprobante.c_ObjetoImpNoImpuesto.Contains(_concepto.ObjetoImpuesto))
        {
            _concepto.Impuestos?.Clean();
            return;
        }
        // Si ObjetoImpuesto es "06" o "08" solo pueden existir Retenciones con tipo impuesto "001"
        if (_concepto.ObjetoImpuesto is "06" or "08")
        {
            // En caso de que existan Traslados se limpian
            _concepto.Impuestos?.Traslados?.Clear();
            // Sí hay retenciones obtener solo las retenciones con tipo impuesto "001"
            var retencionesValid = _concepto.Impuestos?.Retenciones?
                .Where(r => r.Impuesto == "001")
                .ToList();
            if (retencionesValid != null)
            {
                _concepto.Impuestos?.Retenciones?.Clear();
                _concepto.Impuestos?.Retenciones?.AddRange(retencionesValid);
            }
        }
        // Si ObjetoImpuesto es "07"" solo pueden existir Retenciones con tipo impuesto "001", y es obligatorio al menos un Traslado con tipo Impuesto "003"
        if (_concepto.ObjetoImpuesto is "07")
        {
            // Sí hay traslados, obtener solo los traslados con tipo impuesto "003"
            var trasladosValid = _concepto.Impuestos?.Traslados?
                .Where(r => r.Impuesto == "003")
                .ToList();
            if (trasladosValid == null || trasladosValid.Count == 0)
            {
                _context.AddError(
                    section: $"Comprobante -> {_noConcepto}. Concepto",
                    message: "Si ObjetoImpuesto contiene el valor '07' es obligatorio incluir al menos un Traslado con tipo Impuesto '003'.");
                return;
            }
            _concepto.Impuestos?.Traslados?.Clear();
            _concepto.Impuestos?.Traslados?.AddRange(trasladosValid);
            
            // Sí hay retenciones obtener solo las retenciones con tipo impuesto "001"
            var retencionesValid = _concepto.Impuestos?.Retenciones?
                .Where(r => r.Impuesto == "001")
                .ToList();
            if (retencionesValid != null)
            {
                _concepto.Impuestos?.Retenciones?.Clear();
                _concepto.Impuestos?.Retenciones?.AddRange(retencionesValid);
            }
        }
        // Si el atributo ObjetoImpuesto contiene el valor "02" el nodo hijo Impuestos del nodo concepto debe existir.
        if (_concepto.ObjetoImpuesto is "02" && _concepto.Impuestos is null)
        {
            _context.AddError(
                section: $"Comprobante -> {_noConcepto}. Concepto",
                message: "Si ObjetoImpuesto contiene el valor '02' el nodo hijo Impuestos del nodo Concepto debe existir.");
            return;
        }

        ImpuestosConceptoFormat impuestosFormat = new(_context);
        
        var traslados = _concepto.Impuestos?.Traslados;
        if (traslados != null)
        {
            var count = traslados.Count;
            for (int i = 0; i < count; i++)
            {
                var traslado = traslados[i];
                var section = $"Comprobante -> {_noConcepto}. Concepto -> Impuesto -> {i + 1}. Traslado";
                //var message =
                    //$"El valor del campo TasaOCuota que corresponde a Traslado no contiene un valor del catálogo " +
                    //$"c_TasaOcuota o se encuentra fuera de rango. Valor registrado: {traslado.TasaOCuota}";
                //impuestosFormat.ValidateTasaOCuota(impuesto: traslado, section: section, message: message);
                impuestosFormat.FormatImpuesto(impuesto: traslado, section: section);
            }
            _context.AddImpuestosTraslados(traslados);
        }
        
        var retenciones = _concepto.Impuestos?.Retenciones;
        if (retenciones != null)
        {
            var count = retenciones.Count;
            for (int i = 0; i < count; i++)
            {
                var retencion = retenciones[i];
                var section = $"Comprobante -> {_noConcepto}. Concepto -> Impuesto -> {i + 1}. Retención";
                impuestosFormat.FormatImpuesto(impuesto: retencion, section: section);
            }
            _context.AddImpuestosRetenciones(retenciones);
        }
    }     

    private void AddDescuento(string descuento)
    {
        var descuentoConcepto = decimal.Parse(descuento);
        var descuentoContext = _context.GetValue("descuento") ?? "0";
        var descuentoResult = DecimalOperator.Suma(decimal.Parse(descuentoContext), descuentoConcepto);
        _context.AddValue("descuento", descuentoResult.ToString(CultureInfo.InvariantCulture));
    }
}