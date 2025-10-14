using Journaler.Source;

namespace Journaler;

class Program
{
    static void Main(string[] args)
    {
        LocalDirectorySourceOrigin ldso = new LocalDirectorySourceOrigin(99, "local");
        string rootDir = "./Test";
        if (Directory.Exists(rootDir))
        {
            var result = ldso.BuildSourceTree(rootDir);
            PrintSourceNode(result.Tree.RootNode);
        }
    }

    static void PrintSourceNode(SourceNode node, string indent = "", bool isLast = true)
    {
        string branch = isLast ? "└─" : "├─";

        Console.WriteLine($"{indent}{branch} {node.Name} ({node.Parent?.Name})");

        string childIndent = indent + (isLast ? "  " : "│ ");

        for (int i = 0; i < node.Children.Count; i++)
        {
            bool lastChild = i == node.Children.Count - 1;
            PrintSourceNode(node.Children[i], childIndent, lastChild);
        }
    }

}



// MultiSource something

class SourceTreeFactory()
{
    
}

public record SourceTreeResult(SourceTree? Tree, bool Success, string? ErrorMessage);

public record SourceTree(ISourceOriginStrategy SourceOrigin, SourceNode RootNode);