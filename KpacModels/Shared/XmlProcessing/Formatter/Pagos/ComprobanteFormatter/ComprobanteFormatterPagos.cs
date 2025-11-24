using KPac.Domain.Constants;
using KpacModels.Shared.Models.Comprobante;

namespace KpacModels.Shared.XmlProcessing.Formatter.Pagos.ComprobanteFormatter;

public class ComprobanteFormatterPagos
{
    private Comprobante40  _comprobante;
    public void Format(Comprobante40 root)
    {
        _comprobante = root;
        FormatBaseAttributes();
        FormatConceptos();
        FormatImpuestos();
    }
    
    private void FormatBaseAttributes()
    {
        _comprobante.Version = "4.0";
        
        if (_comprobante.Fecha == null)
        {
            _comprobante.Fecha = DateTime.Now.ToString(DateIsoFormats.ISO_8601);
        }

        // El valor del campo TipoDeComprobante debe ser "P"
        _comprobante.TipoComprobante = "P";
        
        // El valor del campo Exportacion debe ser "01"
        _comprobante.Exportacion = "01";
        
        // El valor del campo SubTotal debe ser cero "0".
        _comprobante.Subtotal = "0";
        
        // El valor del campo Moneda debe ser "XXX".
        _comprobante.Moneda = "XXX";
        
        // El campo FormaPago no se debe registrar en el CFDI.
        _comprobante.FormaPago = null;
        
        // El campo MetodoPago no se debe registrar en el CFDI.
        _comprobante.MetodoPago = null;
        
        // El campo CondicionesDePago no se debe registrar en el CFDI.
        _comprobante.CondicionesPago = null;
        
        // El campo Descuento no se debe registrar en el CFDI.
        _comprobante.Descuento = null;
        
        // El campo TipoCambio no se debe registrar en el CFDI.
        _comprobante.TipoCambio = null;
        
        // El valor del campo Total debe ser cero "0".
        _comprobante.Total = "0";
        
    }
    
    private void FormatConceptos()
    {
        var conceptos = _comprobante.Conceptos;
        
        if(conceptos == null)
            conceptos = [];
        
        var concepto = new Concepto()
        {
            ClaveProdServ = "84111506",
            NoIdentificacion = null,
            Cantidad = "1",
            ClaveUnidad = "ACT",
            Unidad = null,
            Descripcion = "Pago",
            ValorUnitario = "0",
            Importe = "0",
            Descuento = null,
            ObjetoImpuesto = "01"
        };
        conceptos.Clear();
        conceptos.Add(concepto);
        
        _comprobante.Conceptos = conceptos;
    }
    
    private void FormatImpuestos()
    {
        // No se debe registrar el apartado de Impuestos en el CFDI.
        _comprobante.Impuestos = null;
    }
}