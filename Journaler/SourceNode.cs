namespace Journaler;

public class SourceNode(string name, string absolutePath, SourceNode? parent, List<SourceNode> children)
{
    public List<SourceNode>? Children { get; } = children;
    public string Name { get; } = name;
    public string AbsolutePath  { get;} = absolutePath;
    public SourceNode? Parent { get; } = parent;

    public List<SourceNode>? GetChildren()
    {
        return Children;
    }
    
    public void AddSourceNode(SourceNode sourceNode)
    {
        Children?.Add(sourceNode);
    }
}