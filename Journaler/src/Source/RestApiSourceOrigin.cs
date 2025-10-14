namespace Journaler.Source;

public class RestApiSourceOrigin : ISourceOriginStrategy
{
    public event Action<SourceTreeResult>? OnSuccess;
    public event Action<SourceTreeResult>? OnFailure;
    public string Name { get; init; }
    public string Token { get; init; } 
    public string Url { get; init; }
    public RestApiSourceOrigin(string token, string url, string name)
    {
        Token = token;
        Url = url;
        Name = name;

        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("WARNING: RestAPISourceOrigin is only a test and is not implemented.");
        Console.ResetColor();
    }
    public SourceTreeResult GetSourceTree(string source)
    {
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine($"[FAKE] RestAPI: {Name}, Token: {Token}, Url: {Url}");
        
        Console.WriteLine("[FAKE] Connecting to rest api...");

        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("[FAKE] Connected");
        Console.ResetColor();

        var sT = new SourceTree(this, new SourceNode("test", "https://thisisafakeapi.fake/api/", null, new List<SourceNode>()));
        var sTr = new SourceTreeResult(sT, true, null);
        
        OnSuccess?.Invoke(sTr);
        return sTr;
    }
}