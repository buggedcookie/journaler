using System.Data.SqlTypes;
using System.IO.Compression;
using System.Text;
using Journaler.Entry;

namespace Journaler;

internal static class Program
{
    /*
     * Refactoring idea
     * classes: Category, Subject and Article maybe... Entry?
     *
     *
     * IEntry:
     *
     * Category:
     *  > Subject[] subjects
     *  > string name
     *  > string description
     *  > AddSubject, RemoveSubject, FindSubject, GetAllArticles(),
     * Subject:
     *  > Category parent
     *  > string name
     *  > string description
     *  > Article[] articles
     *  > AddArticle, RemoveArticle, FindArticle, GetLatestArticle, GetArticleBy(filterType)
     * Article:
     *  > name
     *  > description
     *  > author
     *  > publishDate
     *  > updateDate
     *  > isDirty
     *  > hash
     *  > Subject parent
     *  > ChangeDescription
     *  > ChangeName
     *  > ChangePublishDate
     *  > MarkUpdated
     *  > ComputeContentHash
     *  > Push
     */
    private static void Main(string[] args)
    {
        Category category = new();
        Subject subject = new(category, []);
        
    }
}