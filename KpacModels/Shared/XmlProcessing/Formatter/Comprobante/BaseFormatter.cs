using System.Globalization;
using KPac.Application.Formatter;
using KPac.Application.Validator.Catalogos;
using KpacModels.Shared.Models.Comprobante;
using KpacModels.Shared.XmlProcessing.Validator.Catalogos;

namespace KpacModels.Shared.XmlProcessing.Formatter.Comprobante;

public class BaseFormatter
{
    private FormatContext _context;
    private Comprobante40 _comprobante;
    private decimal _total = decimal.Zero;
    
    public BaseFormatter(FormatContext context)
    {
        _context = context;
    }
    
    public void Format(Comprobante40 comprobante)
    {
        _total = decimal.Zero;
        _comprobante = comprobante;
        if (comprobante.InformacionGlobal != null)
            comprobante.TipoComprobante = "I";
        ValidarCamposMinimos();
        comprobante.Version = "4.0";
        comprobante.Fecha = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss");
        FormatFormaPago();
        FormatSubtotal();
        FormatMetodoPago();
        FormatExportacion();
        FormatDescuento();
        FormatTotal();
        FormatMoneda();
    }

    

    private void ValidarCamposMinimos()
    {
        
        if (!CatalogosComprobante.c_TipoDeComprobante.Contains(_comprobante.TipoComprobante ?? string.Empty))
        {
            _context.AddError(
                section: "Comprobante",
                message: "El campo TipoComprobante no contiene un valor del catalogo c_TipoDeComprobante.",
                messageDetail: "Valores aceptadas por el catalogo: P (Pago), T (Traslado), N (Nomina), E (Egreso), I (Ingreso).");
            return;
        }

        if (_comprobante.TipoComprobante is "I" or "E" && _comprobante.FormaPago == null)
        {
            _context.AddError(
                section: "Comprobante",
                message: "El campo FormaPago es obligatorio.",
                messageDetail: "El campo FormaPago es obligatorio cuando TipoComprobante es I (Ingreso) o E (Egreso).");
            return;
        }
    }
     
    private void FormatFormaPago()
    {
        if (_comprobante.TipoComprobante is "N" or "T" or "P")
        {
            _comprobante.FormaPago = null;
            _comprobante.CondicionesPago = null;
            return;
        }

        if (_comprobante.FormaPago == "99")
        {
            _comprobante.MetodoPago = "PPD";
            return;
        }

        if (_comprobante.MetodoPago == "PPD")
        {
            _comprobante.FormaPago = "99";
            return;
        }
    }
    
    private void FormatSubtotal()
    {
        if (_comprobante.TipoComprobante is "T" or "P")
        {
            _comprobante.Subtotal = "0";
            return;
        }
  
        if (_comprobante.Conceptos == null || _comprobante.Conceptos.Count() == 0)
        {
            _context.AddError( 
                section: "Comprobante -> Conceptos",
                message: "Debe existir al menos un Concepto registrado.",
                messageDetail: "Cuando el TipoDeComprobante es I (Ingreso), E (Egreso) o N (Nomina), debe existir al menos un Concepto registrado.");
            return;
        }
        var subtotal = _context.GetSubtotal().ToString(CultureInfo.InvariantCulture);
        _comprobante.Subtotal = subtotal;
        _total += decimal.Parse(subtotal, CultureInfo.InvariantCulture);
    }
    
    private void FormatMetodoPago()
    {
        //  MetodoPago no debe existir si TipoDeComprobante es T o P.
        if (_comprobante.TipoComprobante is "T" or "P")
        {
            _comprobante.MetodoPago = null;
        }
        // Cuando el nodo InformacionGlobal está presente, MetodoPago debe ser "PUE".
        var informacionGlobal = _comprobante.InformacionGlobal;
        if (informacionGlobal != null)
            _comprobante.MetodoPago = "PUE";
        
    }
    
    private void FormatExportacion()
    {
        // Si existe el complemento Comercio Exterior, Exportación debe ser 02
        var complemento = _comprobante.Complemento;
        if(complemento != null)
            if (complemento.ComercioExterior != null && complemento.ComercioExterior.Count > 0)
                _comprobante.Exportacion = "02";
        
        // Si esta presente Información Global, Exportación debe ser 01
        var informacionGlobal = _comprobante.InformacionGlobal;
        if (informacionGlobal != null)
            _comprobante.Exportacion = "01";

    }
    
    private void FormatDescuento()
    {
        var descuentoString = _context.GetValue("descuento");
        if (descuentoString != null)
        {
            _comprobante.Descuento = descuentoString;
            var descuento = decimal.Parse(descuentoString, CultureInfo.InvariantCulture);
            _total -= descuento;
            
        }
    }
    
    private void FormatTotal()
    {
        
        var totalImpuestosTrasladosString = _context.GetValue("totalImpuestosTraslados");
        if (totalImpuestosTrasladosString != null)
        {
            var totalImpuestosTraslados = decimal.Parse(totalImpuestosTrasladosString, CultureInfo.InvariantCulture);
            _total += totalImpuestosTraslados;
        }
        var totalImpuestosRetencionesString = _context.GetValue("totalImpuestosRetenciones");
        if (totalImpuestosRetencionesString != null)
        {
            var totalImpuestosRetenciones = decimal.Parse(totalImpuestosRetencionesString, CultureInfo.InvariantCulture);
            _total -= totalImpuestosRetenciones;
        }
        _comprobante.Total = _total.ToString(CultureInfo.InvariantCulture);
    }
    
    private void FormatMoneda()
    {
        if (_comprobante.Moneda == null)
            _comprobante.Moneda = "MXN";
    }


}