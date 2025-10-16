namespace Journaler.Entry;

public class Subject(Category parent, Article[] articles) : Entry
{
    public Category Parent { get; private set; } = parent;
    public Article[] Articles { get; private set; } = articles;

    Article[] GetAllArticles() => Articles;
    Category GetParent() => Parent;
    IEnumerable<Article> FindAllArticlesBy(Predicate<Article> condition) => Articles.Where(e => condition(e));
}