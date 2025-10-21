using Microsoft.CodeAnalysis.CSharp;

namespace ParameterDuplicator.Utils;

public class FileHelper
{
    private static CSharpSyntaxTree GetSyntaxTree(string path)
    {
        var inputFilePath = "./Examples/InputFiles/" + path;
        if (string.IsNullOrWhiteSpace(path))
            throw new ArgumentException("Path cannot be null or empty", nameof(path));
        if (!File.Exists(inputFilePath))
            throw new FileNotFoundException($"File not found: {path}", inputFilePath);
        var code =  File.ReadAllText(inputFilePath);
        return (CSharpSyntaxTree)CSharpSyntaxTree.ParseText(code);
    }

    public static CSharpSyntaxNode GetSyntaxTreeRoot(string path)
    {
        return GetSyntaxTree(path).GetRoot();
    }

    public static void RecordOutputFile(CSharpSyntaxNode processedNode, string inputFilePath)
    {
        File.WriteAllText("./Examples/OutputFiles/Processed"+inputFilePath, processedNode.ToFullString());
    }
}