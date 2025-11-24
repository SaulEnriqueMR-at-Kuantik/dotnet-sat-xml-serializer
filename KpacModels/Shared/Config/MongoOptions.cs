namespace KpacModels.Shared.Config;

public class MongoOptions
{
    public const string Key = "Mongo";
    public string ConnectionUrl { get; set; } = string.Empty;
    public string DatabaseName { get; set; } = string.Empty;
}