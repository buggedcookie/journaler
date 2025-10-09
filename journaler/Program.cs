using System.Text;

namespace journaler;

internal class Program
{
    private static void Main(string[] args)
    {
        Console.ForegroundColor = ConsoleColor.White;

        string entriesRootDirectory = "./Test";
        
        var journalEntries = JournalEntryFactory.GetEntriesFromDirectory(entriesRootDirectory);
        
        var groupedEntries = journalEntries.OrderBy(c => c.CategoryDisplayName).GroupBy(entry => entry.CategoryDisplayName);

        string tableOfContent = "";

        StringBuilder stringBuilder = new();
        foreach (var entryByCategory in groupedEntries)
        {
            stringBuilder.AppendLine("\n<details>");
            stringBuilder.AppendLine($"<summary> Category: {entryByCategory.Key} | **❮ SUBJECTS: {entryByCategory.Count()} ❯** </summary>");
            stringBuilder.AppendLine("\n| Table of Contents | Update Date |\n| -------- | -------- |");
            
            foreach (var entryBySubject in entryByCategory.OrderBy(c => c.SubjectDisplayName).Select(t => t))
            {
                
                var articlesCount = entryBySubject.Articles.Length;
                stringBuilder.AppendLine($"| \U0001F4DA {entryBySubject.SubjectDisplayName} ❮ ARTICLES: {articlesCount} ❯");

                foreach (var article in entryBySubject.Articles)
                {
                    
                    stringBuilder.AppendLine($"| &nbsp;&nbsp;&nbsp; `\U0001F4C4 {article.Title}` | {article.UpdateDate:d} |");
                }
            }
            stringBuilder.AppendLine("</details>");
        }
        
        tableOfContent = "Bannaaaaaa"; //stringBuilder.ToString();
        Console.WriteLine(tableOfContent);
        
        
        /* TODO:
         * - Table Of Content Generation
         * - Timeline Generation
         * - Metadata Reading/Writing
         *  > PublishDate, UpdateDate, ContentHash
         *  > Title, Description, Author
         *
         * 
         */
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