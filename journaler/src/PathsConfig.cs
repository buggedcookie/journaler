using System.Text.Json.Serialization;

namespace journaler;

[JsonSerializable(typeof(PathsConfig))]
public class PathsConfig
{
    [JsonPropertyName("subjects-entries-directory")] 
    public string SubjectsEntriesDirectory { get; set; }

    [JsonPropertyName("table-of-content-template")]
    public string TableOfContentTemplate { get; set; } = "./templates/table-of-content.md.template";

    [JsonPropertyName("table-of-content-file")]
    public string TableOfContentFile { get; set; } = "../README.md";

    [JsonPropertyName("timeline-template")]
    public string TimelineTemplate { get; set; }  = "./templates/timeline.md.template";

    [JsonPropertyName("timeline-file")] 
    public string TimelineFile { get; set; } = "../TIMELINE.md";
}