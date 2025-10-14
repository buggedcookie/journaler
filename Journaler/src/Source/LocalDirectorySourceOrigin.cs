using System.Runtime.CompilerServices;

namespace Journaler.Source;

public class LocalDirectorySourceOrigin : ISourceOriginStrategy
{
    public event Action<SourceTreeResult>? OnSuccess;
    public event Action<SourceTreeResult>? OnFailure;
    public string Name { get; init; }

    public readonly int MaxRecursionDepth;

    public LocalDirectorySourceOrigin(int maxRecursionDepth, string sourceName)
    {
        Name  = sourceName;
        MaxRecursionDepth = maxRecursionDepth;
    }
    
    public SourceTreeResult GetSourceTree(string sourceRootDirectory)
    {
        try
        {
            BuildSourceTree(sourceRootDirectory);
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            return new SourceTreeResult(null, false, e.Message);
        }

        return null;
    }

    public SourceTreeResult BuildSourceTree(string sourceRootDirectory)
    {
        if (!Directory.Exists(sourceRootDirectory))
            return new SourceTreeResult(null, false, "Directory not found");

        var rootNode = new SourceNode(
            Path.GetFileName(sourceRootDirectory),
            sourceRootDirectory,
            null,
            new List<SourceNode>()
        );

        var stack = new Stack<(string path, SourceNode parent, int depth)>();
        stack.Push((sourceRootDirectory, rootNode, 0));
        
        while (stack.Count > 0)
        {
            var (currentPath, parentNode, depth) = stack.Pop();

            if (depth >= MaxRecursionDepth)
                continue; // skip going deeper

            try
            {
                // Add directories
                foreach (var subDir in Directory.GetDirectories(currentPath))
                {
                    var currentNode = new SourceNode(
                        Path.GetFileName(subDir),
                        subDir,
                        parentNode,
                        new List<SourceNode>()
                    );

                    parentNode.AddSourceNode(currentNode);
                    stack.Push((subDir, currentNode, depth + 1));
                }
                
                    foreach (var file in Directory.GetFiles(currentPath))
                    {
                        var fileNode = new SourceNode(
                            Path.GetFileName(file),
                            file,
                            parentNode,
                            new List<SourceNode>()
                        );
                        parentNode.AddSourceNode(fileNode);
                    }
                
            }
            catch (UnauthorizedAccessException)
            {
                OnFailure?.Invoke(new SourceTreeResult(null, false, "Access denied"));
            }
            catch (DirectoryNotFoundException)
            {
                OnFailure?.Invoke(new SourceTreeResult(null, false, "Directory not found"));
            }
        }
        var sT = new SourceTreeResult(new SourceTree(this, rootNode), true, "");
        OnSuccess?.Invoke(sT);
        return sT;
    }
}