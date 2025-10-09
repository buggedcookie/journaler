using System.Text.Json.Serialization;
using journaler;

public class AppConfig
{
    [JsonPropertyName("paths")] public PathsConfig paths { get; set; } = new();

    [JsonPropertyName("aliases")]
    public Dictionary<string, string> Aliases { get; set; } = new Dictionary<string, string>();
}