namespace Journaler.Entry;

public class Category : Entry
{
    // public Category? parent; (for sub categories)
    
    public Subject[]  Subjects { get; private set; }
}