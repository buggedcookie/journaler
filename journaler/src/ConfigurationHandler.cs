using System.Text.Json;

namespace journaler;

public class ConfigurationHandler
{
    public string ConfigurationPath { get; private set; }
    
    public AppConfig LoadConfiguration(string filePath)
    {
        bool fileExist = File.Exists(filePath);
        ConfigurationPath = filePath;

        AppConfig? appConfig = new();
        if (fileExist)
        { 
            string json = File.ReadAllText(filePath);
            appConfig = JsonSerializer.Deserialize(json, MyJsonContext.Default.AppConfig);            
        }

        return appConfig ?? new AppConfig();
    }
}