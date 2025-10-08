namespace journaler;

public record Article
{
    public required string Title { get; init; }
    public required DateTime PublishDate { get; init; }
    public required DateTime UpdateDate { get; init; }
    public required string FilePath { get; init; }
    public required string Description { get; init; }
    public required string Author { get; init; }
    public required string ContentHash { get; init; }
}