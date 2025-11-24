namespace KpacModels.Shared.Models.Core;

public class Warning
{
    public Warning(string section, string message, string? messageDetail = null)
    {
        Section = section;
        Message = message;
        MessageDetail = messageDetail;
    }
    public string Section { get; set; }
    
    public string Message { get; set; }
    
    public string? MessageDetail { get; set; }
}