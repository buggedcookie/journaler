namespace Journaler.Source.Providers;

public class LocalFileSystemContentSourceProvider(string path) : IContentSourceProvider
{
    public string GetPath() => path;

    public bool Exists() => File.Exists(path);

    public string GetContent() => Exists() ? File.ReadAllText(path) : throw new FileNotFoundException();
    public Stream OpenReadStream() => File.OpenRead(path);
    public Stream OpenWriteStream() => File.OpenWrite(path);
}