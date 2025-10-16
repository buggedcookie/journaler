using Journaler.Source;

namespace Journaler.Entry;

public class EntryNode(string name, string description, string path, EntryType type, ISourceProvider sourceProvider)
{
    public string Name { get; private set; } = name;
    public string Description { get; private set; } = description;
    public string Path { get; private set; } = path; // will be replaced
    public ISourceProvider SourceProvider { get; private set; } = sourceProvider;
    
    
    public EntryType Type { get; private set; } = type;
    public List<EntryNode> Children { get; private set; }  = new List<EntryNode>();

    void AddChild(EntryNode child)
    {
        Children.Add(child);
    }

    IEnumerable<EntryNode> FindChildrenByType(EntryType entryType)
    {
        return Children.Where(e => e.Type == entryType);
    }
    IEnumerable<EntryNode> FindChildrenByName(string entryName)
    {
        return Children.Where(e => e.Name == entryName);
    }
    
    public IEnumerable<EntryNode> FindAllDescendants()
    {
        var stack = new Stack<EntryNode>();
        stack.Push(this);

        while (stack.Count > 0)
        {
            var current = stack.Pop();
            foreach (var child in current.Children)
            {
                yield return child;
                stack.Push(child);
            }
        }
    }
    public IEnumerable<EntryNode> FindAllDescendantsBy(Predicate<EntryNode> condition)
    {
        return FindAllDescendants().Where(e => condition(e));
    }
}