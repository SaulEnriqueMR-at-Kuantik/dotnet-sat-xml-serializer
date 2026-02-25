namespace KpacModels.Shared.Models.Core;

public class FileZip
{
    public FileZip(byte[] content, string nameFile, string nameField)
    {
        Content = content;
        NameFile = nameFile;
        NameField = nameField;
    }

    /// <summary>
    /// Contenido del xml
    /// </summary>
    public byte[] Content { get; set; }
    
    /// <summary>
    /// Nombre del archivo, ejemplo 1234.zip
    /// </summary>
    public string NameFile { get; set; }
    
    /// <summary>
    /// Nombre del archivo en el form data
    /// </summary>
    public string NameField { get; set; }
}