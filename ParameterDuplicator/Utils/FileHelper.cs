using Microsoft.CodeAnalysis.CSharp;

namespace ParameterDuplicator.Utils;

public class FileHelper
{
    // Get the syntax tree of the input file
    private static CSharpSyntaxTree GetSyntaxTree(string path)
    {
        try
        {
            var inputFilePath = Constants.INPUT_FILE_PATH + path;
            if (string.IsNullOrWhiteSpace(path))
                throw new ArgumentException("Path cannot be null or empty", nameof(path));
            if (!File.Exists(inputFilePath))
                throw new FileNotFoundException($"File not found: {path}", inputFilePath);
            
            var code = File.ReadAllText(inputFilePath);
            if (string.IsNullOrWhiteSpace(code))
                throw new InvalidOperationException($"File is empty: {path}");
                
            return (CSharpSyntaxTree)CSharpSyntaxTree.ParseText(code);
        }
        catch (Exception ex) when (!(ex is ArgumentException || ex is FileNotFoundException || ex is InvalidOperationException))
        {
            throw new InvalidOperationException($"Error reading file {path}: {ex.Message}", ex);
        }
    }

    // Get the root of the syntax tree of the input file
    public static CSharpSyntaxNode GetSyntaxTreeRoot(string path)
    {
        return GetSyntaxTree(path).GetRoot();
    }

    // Record the output file
    public static void RecordOutputFile(CSharpSyntaxNode processedNode, string inputFilePath)
    {
        try
        {
            if (processedNode == null)
                throw new ArgumentNullException(nameof(processedNode), "Processed node cannot be null");
            if (string.IsNullOrWhiteSpace(inputFilePath))
                throw new ArgumentException("Input file path cannot be null or empty", nameof(inputFilePath));

            var outputFilePath = Constants.OUTPUT_FILE_PATH + Constants.PROCESSED_PREFIX + inputFilePath;
            
            // Ensure output directory exists
            var outputDir = Path.GetDirectoryName(outputFilePath);
            if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }
            
            File.WriteAllText(outputFilePath, processedNode.ToFullString());
        }
        catch (Exception ex) when (!(ex is ArgumentNullException || ex is ArgumentException))
        {
            throw new InvalidOperationException($"Error writing output file for {inputFilePath}: {ex.Message}", ex);
        }
    }
}