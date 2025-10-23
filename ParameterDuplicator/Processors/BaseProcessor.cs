using Microsoft.CodeAnalysis.CSharp;
using ParameterDuplicator.Rewriters;
using ParameterDuplicator.Utils;

namespace ParameterDuplicator.Processors;

public abstract class BaseProcessor
{
    private readonly CodeRewriter _rewriter = new  CodeRewriter();
    public abstract CSharpSyntaxNode ProcessSyntaxTree(string inputPath);
    
    public void ProcessAndSaveSyntaxTree(string inputPath)
    {
        var processedRoot = ProcessSyntaxTree(inputPath);
        FileHelper.RecordOutputFile(processedRoot,  inputPath);
    }
    
}