using System.IO.Compression;
using System.Text;
using KpacModels.Shared.Models.Core;

namespace KpacModels.Shared.Helper;

public class ZipHelper
{
    public static async Task<byte[]> CreateZipFromXmlListAsync(List<FileXml> listXml)
    {
        using var memoryStream = new MemoryStream();

        // Crea el archivo ZIP en memoria
        using (var archive = new ZipArchive(memoryStream, ZipArchiveMode.Create, true))
        {
            foreach (var xmlData in listXml)
            {
                string fileName = $"{xmlData.NameFile}.xml";

                var entry = archive.CreateEntry(fileName, CompressionLevel.Optimal);

                // Escribir el XML como texto dentro del ZIP
                await using var entryStream = entry.Open();
                await using var writer = new StreamWriter(entryStream, Encoding.UTF8);
                await writer.WriteAsync(xmlData.Content);
            }
        }

        // Devuelve el ZIP como arreglo de bytes
        return memoryStream.ToArray();
    }

    public static async Task<byte[]> CreateZipAsync(byte[] xmlBytes, byte[] pdfBytes, string uuid)
    {
        using var memoryStream = new MemoryStream();

        // Crear el ZIP en memoria
        using (var archive = new ZipArchive(memoryStream, ZipArchiveMode.Create, true))
        {
            if (xmlBytes != null && xmlBytes.Length > 0)
            {
                var xmlEntry = archive.CreateEntry($"{uuid}.xml", CompressionLevel.Optimal);
                await using (var entryStream = xmlEntry.Open())
                {
                    await entryStream.WriteAsync(xmlBytes, 0, xmlBytes.Length);
                }
            }

            if (pdfBytes != null && pdfBytes.Length > 0)
            {
                var pdfEntry = archive.CreateEntry($"{uuid}.pdf", CompressionLevel.Optimal);
                await using (var entryStream = pdfEntry.Open())
                {
                    await entryStream.WriteAsync(pdfBytes, 0, pdfBytes.Length);
                }
            }
        }

        return memoryStream.ToArray();
    }
}