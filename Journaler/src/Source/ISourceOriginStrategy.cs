namespace Journaler.Source;

public interface ISourceOriginStrategy
{
    public event Action<SourceTreeResult>? OnSuccess;
    public event Action<SourceTreeResult>? OnFailure;
    public string Name { get; init; }

    public SourceTreeResult GetSourceTree(string source);
    public bool CanHandleSource(string source) => true;
}