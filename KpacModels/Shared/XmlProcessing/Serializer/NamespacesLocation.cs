using System.Text;
using System.Xml.Serialization;
using KpacModels.Shared.Models.Comprobante;
using KpacModels.Shared.Models.Constants;
using KpacModels.Shared.Models.Retenciones;

namespace KpacModels.Shared.XmlProcessing.Serializer;

public static class NamespacesLocation
{
    
    /// <summary>
    /// Construcción de los namespaces de a cuerdo los complementos que existan en el comprobante.
    /// </summary>
    public static (XmlSerializerNamespaces namespaces, string schemaLocations)  BuildNameSpacesComprobante(Complemento? complemento)
    {
        var namespaces = new XmlSerializerNamespaces();
        var schemaLocations = new StringBuilder();
        namespaces.Add("", Namespaces.CfdiLocation);
        namespaces.Add("xsi", Namespaces.XsiLocation);
        namespaces.Add("cfdi", Namespaces.CfdiLocation);
        schemaLocations.Append(Namespaces.CfdiSchemaLocation);
        if (complemento != null)
        {
            if (complemento.TimbreFiscalDigital != null)
            {
                namespaces.Add("tfd", Namespaces.TfdLocation);
                schemaLocations.Append(Namespaces.TfdSchemaLocation);
            }

            if (complemento.ComercioExterior != null && complemento.ComercioExterior.Count() > 0)
            {
                namespaces.Add("cce20", Namespaces.ComercioExterior20);
                schemaLocations.Append(Namespaces.SchemaComExt);
            }

            if (complemento.Pagos != null && complemento.Pagos.Count() > 0)
            {
                namespaces.Add("pago20", Namespaces.Pagos20);
                schemaLocations.Append(Namespaces.SchemaPagos);
            }

            if (complemento.Nomina != null && complemento.Nomina.Count() > 0)
            {
                namespaces.Add("nomina12", Namespaces.Nomina12);
                schemaLocations.Append(Namespaces.SchemaNomina);
            }

            if (complemento.ImpuestosLocales != null && complemento.ImpuestosLocales.Count() > 0)
            {
                namespaces.Add("implocal", Namespaces.ImpuestosLocales10);
                schemaLocations.Append(Namespaces.SchemaImpuestos);
            }

            if (complemento.CartaPorte != null && complemento.CartaPorte.Count() > 0)
            {
                namespaces.Add("cartaporte31", Namespaces.CartaPorte31);
                schemaLocations.Append(Namespaces.SchemaCartaPorte);
            }
            
            if (complemento.LeyendasFiscales != null && complemento.LeyendasFiscales.Count() > 0)
            {
                namespaces.Add("leyendasFisc", Namespaces.Leyendas10);
                schemaLocations.Append(Namespaces.SchemaLeyendas);
            }
        }
        
        return (namespaces, schemaLocations.ToString());
    }
    
    /// <summary>
    /// Construccion de los namespaces segun los complementos que existan en retenciones.
    /// </summary>
    public static (XmlSerializerNamespaces namespaces, string schemaLocations)  BuildNameSpacesRetenciones(Complementos20? complemeto)
    {
        var namespaces = new XmlSerializerNamespaces();
        var schemaLocations = new StringBuilder();
        namespaces.Add("", Namespaces.CfdiLocation);
        namespaces.Add("xsi", Namespaces.XsiLocation);
        namespaces.Add("retenciones", Namespaces.Retenciones20);
        schemaLocations.Append(Namespaces.SchemaRetenciones20);
        if (complemeto != null)
        {
            var tfd = complemeto.TimbreFiscalDigital;
            if (tfd != null)
            {
                namespaces.Add("tfd", Namespaces.TfdLocation);
                schemaLocations.Append(Namespaces.TfdSchemaLocation);
            }

            var dividendos = complemeto.Dividendos;
            if (complemeto.Dividendos != null && complemeto.Dividendos.Count() > 0)
            {
                namespaces.Add("dividendos", Namespaces.Dividendos10);
                schemaLocations.Append(Namespaces.SchemaDividendos);
            }

            if (complemeto.EnajenacionDeAcciones != null &&  complemeto.EnajenacionDeAcciones.Count() > 0)
            {
                namespaces.Add("enajenaciondeacciones", Namespaces.EnajenacionAcciones10);
                schemaLocations.Append(Namespaces.SchemaEnajenacionAcciones);
            }

            if (complemeto.Intereses != null  && complemeto.Intereses.Count() > 0)
            {
                namespaces.Add("intereses", Namespaces.Intereses10);
                schemaLocations.Append(Namespaces.SchemaIntereses);
            }

            if (complemeto.PagosAExtranjeros != null  && complemeto.PagosAExtranjeros.Count() > 0)
            {
                namespaces.Add("pagosaextranjeros", Namespaces.PagosAExtranjeros);
                schemaLocations.Append(Namespaces.SchemaPagosAExtranjeros);
            }
        }
        
        return (namespaces, schemaLocations.ToString());
    }
    
}  


