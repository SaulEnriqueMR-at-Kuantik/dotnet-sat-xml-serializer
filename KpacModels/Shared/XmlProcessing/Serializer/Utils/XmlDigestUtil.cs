using System.Xml;
using System.Xml.Xsl;

namespace KpacModels.Shared.XmlProcessing.Serializer.Utils;

public static class XmlDigestUtil
{
    public static string DigestXml(string xmlString, string xsltPath)
    {
        // Step 1: Load the XML string into an XmlDocument
        var xmlDocument = new XmlDocument();
        xmlDocument.LoadXml(xmlString);

        // Step 2: Apply the XSLT transformation
        return TransformXml(xmlDocument, xsltPath);
    }

    private static string TransformXml(XmlDocument xmlDocument, string xsltPath)
    {
        
        // Load the XSLT
        var xslt = new XslCompiledTransform();
        
        // Configurar XmlUrlResolver para permitir el acceso a archivos locales
        XmlUrlResolver resolver = new XmlUrlResolver();
        resolver.Credentials = System.Net.CredentialCache.DefaultCredentials;

        
        xslt.Load(xsltPath, XsltSettings.Default, resolver);


        // Use StringWriter to store the XML in a string
        using (StringWriter stringWriter = new StringWriter())
        {
            using (var writer = XmlWriter.Create(stringWriter, xslt.OutputSettings))
            {
                xslt.Transform(xmlDocument, writer);
            }

            return stringWriter.ToString();
        }

    }
    
}