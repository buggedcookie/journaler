using System.Security.Cryptography;
using System.Text;
using Journaler.Source;

namespace Journaler.Entry;

public sealed class Article : Entry
{
    public IContentSourceProvider Source { get; private set; }
    public string Author { get; private set; }
    public DateTime PublishDate { get; private set; }
    public DateTime UpdateDate { get; private set; }
    public byte[] ContentHash { get; private set; }

    public Article(string name, string displayName, string description, string path, string author, DateTime publishDate, DateTime updateDate, byte[] contentHash, IContentSourceProvider source)
    {
        Name =  name;
        DisplayName =  displayName;
        Description = description;
        Path = path;
        Author = author;
        PublishDate = publishDate;
        UpdateDate = updateDate;
        ContentHash = contentHash;
        Source = source;
    }
    
    
    public Article Touch()
    {
        IsDirty = true;
        UpdateDate =  DateTime.Now;
        return this;
    }
    public Article ChangeUpdateDate(DateTime newUpdateDate)
    {
        IsDirty = true;
        UpdateDate =  newUpdateDate;

        return this;
    }

    public string? GetContent() => Source.Exists() ? Source.GetContent() : null;
    
    public Article ComputeContentHash(out byte[]? contentHash)
    {
        if (!Source.Exists())
        {
            contentHash = null;
            return this;
        }
        
        var content = Source.GetContent();
        byte[] bytes = Encoding.UTF8.GetBytes(content);

        SHA256 sha256 = SHA256.Create();
        
        contentHash = sha256.ComputeHash(bytes);
        
        return this;
    }

    public override string ToString()
    {
        StringBuilder builder = new();
        
        builder.AppendLine("Name: " + Name);
        builder.AppendLine("DisplayName: " + DisplayName);
        builder.AppendLine("Description: " + Description);
        builder.AppendLine("Author: " + Author);
        builder.AppendLine("Publish Date: " + PublishDate);
        builder.AppendLine("Update Date: " + UpdateDate);
        builder.AppendLine("Content Hash: " + BitConverter.ToString(ContentHash).Replace("-", ""));
        builder.AppendLine("Source: " + Source);
        return builder.ToString();
    }
    public Article UpdateHash(byte[] newContentHash)
    {
        IsDirty = true;
        ContentHash = newContentHash;
        return this;
    }
}