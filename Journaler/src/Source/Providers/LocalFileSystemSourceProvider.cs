namespace Journaler.Source.Providers;

public class LocalFileSystemSourceProvider(string path) : ISourceProvider
{
    public string GetPath() => path;
    public bool Exists() => Directory.Exists(path);
}