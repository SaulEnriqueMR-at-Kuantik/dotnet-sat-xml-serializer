using System.Text;
using System.Xml;
using System.Xml.Serialization;
using KpacModels.Shared.Models.Comprobante;
using KpacModels.Shared.Models.Retenciones;
using KpacModels.Shared.Models.TimbreFiscalDigital;
using KpacModels.Shared.XmlProcessing.Serializer.Utils;

namespace KpacModels.Shared.XmlProcessing.Serializer;

public static class CfdiSerializer
{
    /// <summary>
    /// Parse Object Comprobante 4.0 to string XML
    /// </summary>
    /// <param name="comprobante">Object Comprobante 4.0</param>
    /// <returns>String Xml or error</returns>
    public static string? CreateXml(Comprobante40 comprobante)
    {
        
            try
            {
                using var writer = new Utf8StringWriter();
                var xmlWriter = XmlWriter.Create(writer);
                try
                {
                    var schemaLocations = SerializeXml(xmlWriter, null, comprobante);
                    var xmlCompleto = ConvertUtils.AddSchemaLocation(writer.ToString(), schemaLocations);
                    return xmlCompleto;
                }
                finally
                {
                    xmlWriter.Dispose();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return null;
        }

    /// <summary>
    /// Parse Object Retenciones 2.0 to string XML
    /// </summary>
    /// <param name="retenciones">Object Comprobante 4.0</param>
    /// <returns>String Xml or error</returns>
    public static (string? xml, string? error) CreateXml(Retenciones20 retenciones)
    {
        try
        {
            using var writer = new Utf8StringWriter();
            var xmlWriter = XmlWriter.Create(writer);
            try
            {
                var schemaLocations = SerializeXml(xmlWriter, retenciones, null);
                var xmlCompleto = ConvertUtils.AddSchemaLocation(writer.ToString(), schemaLocations);
                return (xmlCompleto, null);
            }
            finally
            {
                xmlWriter.Dispose();
            }
        }
        catch (Exception ex)
        {
            return (null, ex.Message);
        }
    }

    private static string SerializeXml(XmlWriter xmlWriter, Retenciones20? retenciones, Comprobante40? comprobante)
    {
        XmlSerializerNamespaces? namespaces = null;
        var schemaLocations = string.Empty;
        if (retenciones != null)
        {
            var serializer = new XmlSerializer(typeof(Retenciones20));
            //Crear los NameSpaces
            (namespaces, schemaLocations) = NamespacesLocation.BuildNameSpacesRetenciones(retenciones.Complemento);

            // Serializar el objeto a XML
            serializer.Serialize(xmlWriter, retenciones, namespaces);
        }
        if (comprobante != null)
        {
            var serializer = new XmlSerializer(typeof(Comprobante40));
            //Crear los NameSpaces
            (namespaces, schemaLocations) = NamespacesLocation.BuildNameSpacesComprobante(comprobante.Complemento);

            // Serializar el objeto a XML
            serializer.Serialize(xmlWriter, comprobante, namespaces);
        }
        return schemaLocations;
    }
    
    
    
    /// <summary>
    /// Funcion para de-serializar el XML de Timbre Fiscal Digital 1.1 en un Objeto tipo TimbreFiscalDigital11
    /// </summary>
    /// <param name="xmlString"> string del XML </param>
    /// <returns> Objeto tipo TimbreFiscalDigital11 </returns>
    public static TimbreFiscalDigital11? DeserializeXmlToTfd(string xmlString)
    {
        var serializer = new XmlSerializer(typeof(TimbreFiscalDigital11));
        using var reader = new StringReader(xmlString);
        return (TimbreFiscalDigital11)serializer.Deserialize(reader)!;
    }
    
    /// <summary>
    /// Funcion para de-serializar el XML de Comprobante 4.0 o  Retenciones 2.0
    /// </summary>
    /// <param name="xmlString"> string del XML </param>
    /// <returns> Objeto tipo TimbreFiscalDigital11 </returns>
    public static T? DeserializeXml<T>(string xmlString)
    {
        var serializer = new XmlSerializer(typeof(T));
        using var reader = new StringReader(xmlString);
        return (T)serializer.Deserialize(reader)!;
    }

    private class Utf8StringWriter : StringWriter
    {
        public override Encoding Encoding => Encoding.UTF8;
    }
}