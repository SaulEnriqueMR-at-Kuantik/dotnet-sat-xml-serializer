namespace KpacModels.Shared.Models.Core;

public class FileXml
{
    public FileXml(string content, string nameFile)
    {
        Content = content;
        Key = string.Empty;
        NameFile = nameFile;
    }

    public FileXml(string content)
    {
        Content = content;
        Key = "xml";
        NameFile = "factura_coor";
    }

    public FileXml(string content, string key, string nameFile)
    {
        Content = content;
        Key = key;
        NameFile = nameFile;
    }
    /// <summary>
    /// Nombre de la llave en la petición
    /// </summary>
    public string Key { get; set; }
    
    /// <summary>
    /// Contenido del xml
    /// </summary>
    public string Content { get; set; }
    
    /// <summary>
    /// Nombre del archivo en la petición
    /// </summary>
    public string NameFile { get; set; }
}