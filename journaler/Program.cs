namespace journaler;

internal class Program
{
    private static void Main(string[] args)
    {
        string entriesRootDirectory = "./Test";

        List<JournalEntry> journalEntries = new();
        if(!Directory.Exists(entriesRootDirectory)) Console.WriteLine("Directory not found: " + entriesRootDirectory);

        foreach (string folderPath in Directory.EnumerateDirectories(entriesRootDirectory))
        {
            var journalEntry = JournalEntryFactory.CreateEntry(folderPath);
            journalEntries.Add(journalEntry);
        }
        
        Console.WriteLine("Found {0} journal entries", journalEntries.Count);

        foreach (JournalEntry entry in journalEntries)
        {
            Console.WriteLine($"{entry.CategoryDisplayName} | {entry.SubjectDisplayName} | {entry.FolderName}");
        }
    }


    public record JournalEntry(
        string FolderDirectory,
        string FolderName,
        string CategoryDisplayName,
        string SubjectDisplayName,
        Article[] Articles
    );

    /*
     * Everything here assumes best case scenario and no mistakes
     * It's terrible, and unflexible.
     * TODO: Make the system better in the future
     */

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

    public static class JournalEntryFactory
    {

        public static JournalEntry CreateEntry(string entryFolderDirectory)
        {
            FolderDirectory = entryFolderDirectory;
            FolderName = GetFolderNameFromDirectory(entryFolderDirectory);

            CategoryDisplayName = GetCategoryDisplayName(FolderName);
            SubjectDisplayName = GetSubjectDisplayName(FolderName);
            Articles = GetArticles().ToArray();

            return new JournalEntry(FolderDirectory, FolderName, CategoryDisplayName, SubjectDisplayName, Articles);
        }

        static string FolderDirectory { get; set; }
        static string FolderName { get; set; }
        static string CategoryDisplayName { get; set; }
        static string SubjectDisplayName { get;  set; }
        static Article[] Articles { get; set; }


        static string GetFolderNameFromDirectory(string folderDirectory)
        {
            return folderDirectory
                .Split("/")
                .Last();
        }

        static string GetCategoryDisplayName(string folder)
        {
            return string.Join("", folder
                .Split("-").First());
            //.Select(s => char.ToUpper(s[0]) + s.Substring(1)));
        }

        static string GetSubjectDisplayName(string folder)
        {
            return string.Join(" ",
                folder.Split("-", StringSplitOptions.RemoveEmptyEntries).Skip(1)
                    .Select(s => char.ToUpper(s[0]) + s.Substring(1)));
        }

        static string GetArticleDisplayName(string folder)
        {
            return string.Join(" ",
                    folder.Split("-", StringSplitOptions.RemoveEmptyEntries)
                        .Select(s => char.ToUpper(s[0]) + s.Substring(1)))
                .Split(".")
                .First();
        }
        // "os", "arch", "linux.md" => Arch Linux

        static List<Article> GetArticles()
        {
            var files = Directory.GetFiles(FolderDirectory);

            List<Article> articles = new();
            for (int i = 0; i < files.Length - 1; i++)
            {
                Article article = new Article
                {
                    Title = GetArticleDisplayName(files[i]
                        .Split("/",
                            StringSplitOptions.RemoveEmptyEntries)
                        .Last()),
                    PublishDate = DateTime.Today,
                    UpdateDate = DateTime.Today,
                    FilePath = files[i],
                    Description = "null",
                    Author = "null",
                    ContentHash = "null"
                };
                articles.Add(article);
            }

            return articles;
        }
    }

    /* NOTE#1:
     * - Get entries's root folder => Get all subfolder's directories
     * - Get the end of those directories just get the folder's name
     *  > EX: "dev-journaler", "os-arch-linux", "dev-csharp", "hw-soldering", "hw-computers"
     * - Get the folder category: Split by "-" get the first element > "dev", "os", "dev", "hw", "hw"
     * - Get the folder subject: "journaler", "arch linux", "csharp", "soldering", "computers"
     * - Make it prettier: Capitalize and use aliases
     *  > "dev" => "Development", "hw" => "Hardware", "arch linux" => "Arch Linux" etc...
     * - Go inside the subject's folder => Get all articles (md's file)
     *  > "day-1.md" or "how-to-install-arch-linux.md", "variables-in-csharp.md", "correct-way-to-apply-solder.md", "building-my-first-computer-from-scratch.md"
     * - Make it prettier too: Replace dashes by spaces, remove file extension, capitalize
     *  > For precision, we don't modify the file's name for anything previously, we save that in the memory as a "prettified" version
     *  > So we can use it for the automatic generation of the Table Of Content.
     *
     * NOTE#1:
     * - Previously, we kept a path reference in memory for each article directly, probably via a class
     *  > Because in the future I want to add metadata in each markdown file <!-- Like This For Example --> (this won't be visible by the renderer)
     *  > But for now it's only to create the link to the "subjectFolderPath + /articleFile"
     *  > In the future I might allow for "dynamic" templates, TLDR; you could customize your template how you want it to be generated
     *  > What this would mean is you wouldn't be force to generate a "Table Of Content" you could add conditions and text/variables to generate the table of content more accurately to what you want.
     *  > See : templates/table-of-content.md.template to see what the table of content would look like
     *  - Oh and a timeline file, fully based on when something is done
     * NOTE-COMMANDS:
     * - journaler --update-all
     *  > This will regenerate the whole table of content automatically, based on the "config.json"
     * > For now this is the only command that exist, as I said I may add other commands in the future if the system gets more complex to personalize etc...
     * - journaler --update-all --from-path="../../entries"
     *  > This could work too, but I might not include it for now.
     *
     * NOTE-TO-MYSELF:
     * - Just make it work, STOP WANTING TO MAKE EVERYTHING AT ONCE
     */
}