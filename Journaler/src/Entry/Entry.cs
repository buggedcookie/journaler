namespace Journaler.Entry;

public abstract class Entry
{
    public virtual string Name { get; protected set; } = "Undefined Entry Name";
    public virtual string DisplayName { get; protected set; } = "Undefined Entry Display Name";
    public virtual string Description { get; protected set; } = "Undefined Entry Description";
    public virtual string Path { get; protected set; } = ""; 
    // How about a SourceProvider? > Works with any kind of input URL, DB, File, DIR, JSON, XML, Remote etc...
    public virtual bool IsDirty { get; protected set; } = false;
    
    
    public virtual Entry ChangeName(string newName)
    {
        IsDirty = true;
        Name = newName;

        return this;
    }
    public virtual Entry ChangeDisplayName(string newDisplayName)
    {
        IsDirty = true;
        DisplayName = newDisplayName;

        return this;
    }

    public virtual Entry ChangeDescription(string newDescription)
    {
        IsDirty = true;
        Description = newDescription;
        
        return this;
    }

    public virtual Entry ChangePath(string newPath)
    {
        IsDirty = true;
        Path = newPath;
        
        return this;
    }
    public virtual Entry MarkClean()
    { 
        IsDirty = false;
        return this;
    }
}