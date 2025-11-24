using KPac.Application.Formatter;
using KPac.Application.Validator.Catalogos;
using KPac.Domain.Constants;
using KpacModels.Shared.Models.Comprobante;
using KpacModels.Shared.XmlProcessing.Validator;
using KpacModels.Shared.XmlProcessing.Validator.Catalogos;

namespace KpacModels.Shared.XmlProcessing.Formatter.Nomina.ComprobanteFormatter;

public class ComprobanteFormatterNomina
{
    
    private FormatContext _context;
    
    public ComprobanteFormatterNomina(FormatContext context)
    {
        _context = context;
    }

    public void Format(Comprobante40 root, SettingsFormatter? configuracion)
    {
        _context.SetSettings(configuracion);
        FormatBaseAtributes(root);
        FormatCfdiRelacionados(root.CfdisRelacionados);
        FormatEmisor(root.Emisor);
        FormatReceptor(root.Receptor);
        FormatConceptos(root);
    }

    private void FormatBaseAtributes(Comprobante40 root)
    {
        root.Version = "4.0";
        root.Fecha = DateTime.Now.ToString(DateIsoFormats.ISO_8601);
        root.FormaPago = null;
        root.CondicionesPago = null;
        root.Moneda = "MXN";
        root.TipoCambio = null;
        root.TipoComprobante = "N";
        root.Exportacion = "01";
        root.MetodoPago = "PUE";
        root.InformacionGlobal = null;
    }

    private void FormatCfdiRelacionados(List<CfdiRelacionado>? cfdisRelacionados)
    {
        if (cfdisRelacionados == null)
            return;
        var section = "Comprobante -> {0}. Cfdi Relacionados -> {1}. UUID";
        var message = "El UUID ingresado no cumple con el formato especificado";
        var count = cfdisRelacionados.Count;
        for(int i = 0; i < count; i++)
        {
            var cfdiRelacionado =  cfdisRelacionados[i];
            cfdiRelacionado.TipoRelacion = "04";
            var countUuids = cfdiRelacionado.UuidsRelacionados.Count;
            for (int j = 0; j < countUuids; j++)
            {
                var uuidRelacionado = cfdiRelacionado.UuidsRelacionados[j];
                if (!Guid.TryParse(uuidRelacionado.Uuid, out _))
                {
                    _context.AddError(string.Format(section, i + 1, j + 1), message);
                }
            }
        }
    }

    private void FormatEmisor(Emisor emisor)
    {
        var section = "Comprobante -> Emisor -> ";
        if (!RegexCatalog.IsRfcValid(emisor.Rfc))
        {
            _context.AddError($"{section} Rfc", "El campo Rfc del Emisor no tiene un formato valido.");
            return;
        }
        
        // Guardar la longitud del Rfc Emisor para saber si debe existir el campo CURP en Nomina.Emisor
        _context.AddValue("lengthRfcEmisor", emisor.Rfc.Length.ToString());

        emisor.FacAtrAdquirent = null;
        
        if(emisor.RegimenFiscal == null)
            return;

        if (!CatalogosComprobante.c_RegimenFiscal.Contains(emisor.RegimenFiscal))
        {
            _context.AddError($"{section} RegimenFiscal", "El valor del campo RegimenFiscal debe pertenecer al catalogo c_RegimenFiscal.");
        }
            
    }

    private void FormatReceptor(Receptor receptor)
    {
        var section = "Comprobante -> Receptor -> ";
        if (!RegexCatalog.IsRfcValid(receptor.Rfc))
        {
            _context.AddError($"{section} Rfc", "El campo Rfc del Receptor no tiene un formato valido.");
            return;
        }

        if (receptor.Rfc.Length != 13)
        {
            _context.AddError($"{section} Rfc", "El campo Rfc del Receptor debe ser persona física (longitud 13 caracteres).");
            return;
        }
        receptor.ResidenciaFiscal = null;
        receptor.NumRegIdTrib = null;
        receptor.RegimenFiscal = "605";
        receptor.UsoCfdi = "CN01";
    }

    private void FormatConceptos(Comprobante40 root)
    {
        var conceptos = root.Conceptos;
        if (conceptos is null)
            conceptos = [];
        else
            conceptos.Clear();
        
        var concepto = new Concepto()
        {
            ClaveProdServ = "84111505",
            NoIdentificacion = null,
            Cantidad = "1",
            ClaveUnidad = "ACT",
            Unidad = null,
            Descripcion = "Pago de nómina",
            ObjetoImpuesto = "01",
        };
        conceptos.Add(concepto);
        root.Conceptos = conceptos;
    }

    public void Format(Comprobante40 root)
    {
        var concepto = root.Conceptos?.FirstOrDefault();
        if (concepto == null)
        {
            return;
        }

        var totalPercepciones = _context.GetValue("totalPercepciones");
        var totalDeducciones = _context.GetValue("totalDeducciones");
        var totalOtrosPagos =  _context.GetValue("totalOtrosPagos");
        var valorUnitario = 0m;
        if(totalPercepciones != null)
            valorUnitario += decimal.Parse(totalPercepciones);
        if(totalOtrosPagos != null)
            valorUnitario 
                +=  decimal.Parse(totalOtrosPagos);
        concepto.ValorUnitario = valorUnitario.ToString("F2");
        concepto.Importe = valorUnitario.ToString("F2");
        if (totalDeducciones != null)
        {
            concepto.Descuento = totalDeducciones;
        }

        root.Subtotal = concepto.Importe;
        root.Descuento = concepto.Descuento;
        var subtotal = decimal.Parse(root.Subtotal);
        var descuento = decimal.Parse(root.Descuento ?? "0");
        root.Total = (subtotal - descuento).ToString("F2");
    }
}