using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

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

        static Article GetArticleMetadata(string articleFileDirectory)
        {

            if(!File.Exists(articleFileDirectory)) Console.WriteLine("Directory not found: " + articleFileDirectory);
            
            string fileFullContent = File.ReadAllText(articleFileDirectory);

            Article? articleMetadata = null;
            
            bool hasMetadata = TryParseMetadataFromFile(fileFullContent, articleFileDirectory, out articleMetadata, out var hasUpdatedContent);
            //if(!hasMetadata) AssumeArticleInfo(articleFileDirectory);
            

            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(hasUpdatedContent);
            Console.ResetColor();
            
            WriteMetadata(articleFileDirectory, articleMetadata, hasUpdatedContent);
            Console.ResetColor();
            return articleMetadata!;
            
            /*
             * <!--{
             * @Title="Custom Title";
             * @Description="Custom Description";
             * @Author="BuggedCookie";
             * 
             * @PublishDate="2025-10-4";
             * @UpdateDate="2025-10-08";
             * @ContentHash="HashOfTheContentHere"
             * }-->
             *
             * TODO:
             * - Remove any \t \n \b andd spaces etc...
             * - Split by ; for different instruction, each instruction starts with @
             * - Make sure the block starts with <!--{ and ends with }-->
             *
             * > MetaInitiator = { , MetaTerminator = }, FieldInitiator = @, FieldName, Assignment, Value "", FieldTerminator
             *
             * TODO - To calculate hash, we must remove the whole meta block to avoid... problems
             */
        }

        static void WriteMetadata(string articleFileDirectory, Article articleMetadata, bool overrideUpdateDate = false)
        {
            if (!File.Exists(articleFileDirectory)) Console.WriteLine("File not found: " + articleFileDirectory);

            var cleanedContent = GetArticleContentOnly(articleFileDirectory);

            var updatedContent =  articleMetadata.GetMetaString(overrideUpdateDate).TrimEnd() + "\n" + cleanedContent.TrimEnd().TrimStart();;
            
            

            File.WriteAllBytes(articleFileDirectory, Encoding.UTF8.GetBytes(updatedContent));
            
        }
        private static readonly string RegexBlockSelection = @"<!--\{\s*(.*?)\s*\}-->";
        static bool TryParseMetadataFromFile(string fileFullContent, string filePath, out Article? articleMetadata, out bool hasDifferentContent)
        {
            
            Regex regex = new Regex(RegexBlockSelection, RegexOptions.Singleline);
            Match match = regex.Match(fileFullContent);
            hasDifferentContent = false;
            string contentNoMetadata = RemoveArticleInfoFromContent(fileFullContent);
            string contentNoMetadataHash = CalculateArticleContentHash(contentNoMetadata);
            string contentBlock = match.Groups[1].Value;
            Console.WriteLine(fileFullContent);
            string title = "undefined";
            string description = "undefined";
            string author = "undefined";

            string contentHash = "undefined";
            DateTime publishDate = DateTime.MinValue;
            DateTime updateDate = DateTime.MinValue;
            
            articleMetadata = new Article()
            {
                Title = title,
                Description = description,
                Author = author,
                PublishDate = publishDate,
                UpdateDate = updateDate,
                FilePath = filePath,
                ContentHash = contentHash
            };
            articleMetadata = AssumeArticleInfo(filePath);
            
            if (!match.Success) return false;

            string sanitizedMetadata = SanitizeMetadata(contentBlock);

            var kvp = sanitizedMetadata.Split("@",  StringSplitOptions.RemoveEmptyEntries)
                .Select(s => s.Split('='))
                .ToDictionary(t => t[0], t => t[1]);
            //Title=Something, Description=Something


            bool hasPublishDate = false;
            bool hasUpdateDate = false;
            bool hasContentHash = false;

            bool hasMinimumMetaRequirement = false;

            //foreach (var kvpPair in kvp)
            //{
            //    Console.WriteLine(kvpPair.Key + " | " + kvpPair.Value);
            //}
            foreach (var k in kvp.Keys)
            {
                switch (k.ToLower())
                {
                    case "title":
                        kvp.TryGetValue(k, out var t);
                        title = string.IsNullOrEmpty(t) ? GetArticleDisplayName((filePath)) : t;
                        break;
                    case "description":
                        kvp.TryGetValue(k, out var d);
                        description = d ?? "Undefined description";
                        break;
                    case "author":
                        kvp.TryGetValue(k, out var a);
                        author = a ?? "Undefined author";
                        break;
                    case "contenthash":
                        kvp.TryGetValue(k, out var c);
                        var calculatedHash = CalculateArticleContentHash(contentNoMetadata);
                        hasDifferentContent = c != calculatedHash;
                        contentHash = calculatedHash;
                        hasContentHash = true;
                        break;
                    case "publishdate":
                        kvp.TryGetValue(k, out var p);
                        publishDate = (DateTime.TryParse(p, out DateTime dtPublish) ? dtPublish : DateTime.Now);
                        hasPublishDate = true;
                        break;
                    case "updatedate":
                        kvp.TryGetValue(k, out var u);
                        updateDate = DateTime.TryParse(u, out DateTime dtUpdate ) ? dtUpdate : DateTime.Now; 
                        hasUpdateDate = true;
                        break;
                }
                
                
            }
            
            articleMetadata = new Article()
            {
                Title = title,
                Description = description,
                Author = author,
                PublishDate = publishDate,
                UpdateDate = updateDate,
                FilePath = filePath,
                ContentHash = contentHash
            };
            hasMinimumMetaRequirement = hasContentHash && hasUpdateDate && hasPublishDate;
            return hasMinimumMetaRequirement;
        }
        static string SanitizeMetadata(string input)
        {
            var sb = new StringBuilder();
            bool insideQuotes = false;

            foreach (char c in input)
            {
                if (c == '"')
                {
                    insideQuotes = !insideQuotes; // toggle
                    continue; // remove quotes
                }

                if (!insideQuotes)
                {
                    if (c == ' ' || c == '\t' || c == '\n' || c == '\r' || c == ';')
                        continue; // skip whitespace and semicolons outside quotes
                }

                sb.Append(c);
            }

            return sb.ToString().TrimEnd().TrimStart();
        }

        static Article AssumeArticleInfo(string articleFileDirectory)
        {
            string title = GetArticleDisplayName(GetFolderNameFromDirectory(articleFileDirectory));
            string description = "Not Defined";
            string author = "Not Defined";

            DateTime publishDate = DateTime.Today;
            DateTime updateDate = DateTime.Today;

            string contentHash = CalculateArticleContentHash(GetArticleContentOnly(articleFileDirectory));
            return new Article()
            {
                Title = title,
                Description = description,
                Author = author,
                PublishDate = publishDate,
                UpdateDate = updateDate,
                ContentHash = contentHash,
                FilePath = articleFileDirectory
            };
        }

        static string CalculateArticleContentHash(string fileContent)
        {
            string sanitizedContent = RemoveArticleInfoFromContent(fileContent).TrimEnd().TrimStart();
            // Spent to fucking hours on that fucking shit, wanna know why?
            // TrimEnd and TrimStart, WELL APPPARENTTLYY for (not) stupid reasons if I DON'T new line character be fcking it up half the time
            // Made me wonder for 2 hours... why it was so random...
            using var sha256 = SHA256.Create();
            byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(sanitizedContent));

            StringBuilder stringBuilder = new ();

            foreach (var b in bytes){
                stringBuilder.Append(b.ToString("x2"));
                Console.Write($"{b:X2} ");
            }
            return stringBuilder.ToString();
        }

        static string RemoveArticleInfoFromContent(string content)
        {
            return Regex.Replace(content, RegexBlockSelection, "", RegexOptions.Singleline);
        }
        static string GetArticleContentOnly(string articleFileDirectory)
        {
            if(!File.Exists(articleFileDirectory)) Console.WriteLine("File not found: " + articleFileDirectory);
            
            var fileFullContent = File.ReadAllText(articleFileDirectory);
            
            return Regex.Replace(fileFullContent, RegexBlockSelection, "", RegexOptions.Singleline);
        }
        
        static List<Article> GetArticles()
        {
            var files = Directory.GetFiles(FolderDirectory);

            List<Article> articles = new();
            for (int i = 0; i < files.Length; i++)
            {
                Article article = GetArticleMetadata(files[i]);
                articles.Add(article);
            }

            return articles;
        }
    }
    