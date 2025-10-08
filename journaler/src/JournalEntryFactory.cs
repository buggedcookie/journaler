namespace journaler;
    
/*
 * Everything here assumes best case scenario and no mistakes
 * It's terrible, and unflexible.
 * TODO: Make the system better in the future
 */
public static class JournalEntryFactory
    {

        public static JournalEntry CreateEntryFromDirectory(string entryFolderDirectory)
        {
            FolderDirectory = entryFolderDirectory;
            FolderName = GetFolderNameFromDirectory(entryFolderDirectory);

            CategoryDisplayName = GetCategoryDisplayName(FolderName);
            SubjectDisplayName = GetSubjectDisplayName(FolderName);
            Articles = GetArticles().ToArray();

            return new JournalEntry(FolderDirectory, FolderName, CategoryDisplayName, SubjectDisplayName, Articles);
        }

        public static List<JournalEntry> GetEntriesFromDirectory(string entryFolderDirectory)
        {
            List<JournalEntry> journalEntries = new();
            if (!Directory.Exists(entryFolderDirectory)) Console.WriteLine("Directory not found: " + entryFolderDirectory);

            foreach (string folderPath in Directory.EnumerateDirectories(entryFolderDirectory))
            {
                var journalEntry = CreateEntryFromDirectory(folderPath);
                journalEntries.Add(journalEntry);
            }

            return journalEntries;

        }

        private static string FolderDirectory { get; set; } = "";
        private static string FolderName { get; set; } = "";
        private static string CategoryDisplayName { get; set; } = "";
        private static string SubjectDisplayName { get; set; } = "";
        private static Article[] Articles { get; set; } = [];


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
            for (int i = 0; i < files.Length; i++)
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