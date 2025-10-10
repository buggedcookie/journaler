using System.Text;

namespace journaler;

internal class Program
{
    private static void Main(string[] args)
    {
        Console.ForegroundColor = ConsoleColor.White;

        // TODO: CREATE TIMELINE SYSTEM
        
        AppConfig config = new ConfigurationHandler().LoadConfiguration("./config.json");
        Console.WriteLine(config.paths.TableOfContentTemplate);

        JournalEntryFactory.LoadConfigBecauseImLazy(config);
        List<JournalEntry> journalEntries = JournalEntryFactory.GetEntriesFromDirectory(config.paths.SubjectsEntriesDirectory);

        string entriesList = "";
        foreach (var entry in journalEntries)
        {
            entriesList += entry.FolderName + ",";
        }
        Console.WriteLine($"{entriesList}");

        var tableOfContent = BuildTableOfContent(ref journalEntries, config);
        Console.WriteLine(tableOfContent);

        string tocFilePath = config.paths.TableOfContentFile;
        string tocFileTemplatePath = config.paths.TableOfContentTemplate;
        bool tocTemplateFileExist = File.Exists(tocFileTemplatePath);
        bool tocFileExist = File.Exists(tocFilePath);
        string tocFiletext = "";
        string fileTemplateContent = "";

        Console.ForegroundColor = ConsoleColor.Red;
        if (!tocFileExist && !tocTemplateFileExist) Console.WriteLine($"Error: {tocFilePath} and {tocFileTemplatePath} paths are required");
        Console.ResetColor();
        fileTemplateContent = File.ReadAllText(tocFileTemplatePath);
            
        tocFiletext = fileTemplateContent;
        File.WriteAllText(tocFilePath, tocFiletext);

        
        Console.WriteLine("Generated table of content file at: " + config.paths.TableOfContentFile);
        tocFiletext = tocFiletext.Replace("{TOC}", tableOfContent);
        File.WriteAllText(tocFilePath, tocFiletext);
        
        
            // TODO:
            // ATTEMPT TO CREATE FROM FILE TEMPLATE IF MISSING
            // REPLACE {TOC} by the actual content
            // TIMELINE
            // FOR THE FUTURE:
            // DYNAMIC TEMPLATES
            // Crossing my fingers it just works lmao

            /* TODO:
             * - Table Of Content Generation
             * - Timeline Generation
             */
    }

    static string BuildTableOfContent(ref List<JournalEntry> journalEntries, AppConfig appConfig)
    {

        StringBuilder stringBuilder = new();
        
        var groupedEntries = journalEntries.OrderBy(c => c.CategoryDisplayName).GroupBy(entry => entry.CategoryDisplayName);

        foreach (var entryByCategory in groupedEntries)
        {
            stringBuilder.AppendLine("\n<details>");
            stringBuilder.AppendLine($"<summary> Category: {entryByCategory.Key} — ❮ SUBJECTS: {entryByCategory.Count()} ❯ </summary>");
            stringBuilder.AppendLine("\n| Table of Contents | Update Date |\n| -------- | -------- |");
            
            foreach (var entryBySubject in entryByCategory.OrderBy(c => c.SubjectDisplayName).Select(t => t))
            {
                
                var articlesCount = entryBySubject.Articles.Length;
                stringBuilder.AppendLine($"| \U0001F4DA {entryBySubject.SubjectDisplayName} ❮ ARTICLES: {articlesCount} ❯");

                foreach (var article in entryBySubject.Articles)
                {
                    
                    string path = appConfig.paths.UseGithubPath ? article.FilePath.Replace("../", String.Empty) : article.FilePath;
                    stringBuilder.AppendLine($"| &nbsp;&nbsp;&nbsp; \U0001F4C4 [`{article.Title}`]({path}) | {article.UpdateDate:d} |");
                }
            }
            stringBuilder.AppendLine("</details>");
            
        }
        
        return stringBuilder.ToString();
    }
}
