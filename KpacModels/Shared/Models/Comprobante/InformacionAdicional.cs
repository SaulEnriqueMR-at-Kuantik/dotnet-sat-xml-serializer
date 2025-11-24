using System.Text.Json.Serialization;

namespace KpacModels.Shared.Models.Comprobante;

public class InformacionAdicional
{
    [JsonPropertyName("StampedByKuantik")]
    public string? StampedByKuantik { get; set; }
    
    [JsonPropertyName("Comentario")]
    public string? Comentario { get; set; }
    
    [JsonPropertyName("Desgloce")]
    public string? Desgloce { get; set; }
    
    [JsonPropertyName("NotaPie")]
    public string? NotaPie { get; set; }
    
    [JsonPropertyName("IdProyecto")]
    public string? IdProyecto { get; set; }
    
    [JsonPropertyName("Categoria")]
    public string? Categoria { get; set; }
}