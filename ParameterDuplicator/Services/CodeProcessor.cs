using Microsoft.CodeAnalysis.CSharp;
using ParameterDuplicator.Rewriters;
using ParameterDuplicator.Utils;

namespace ParameterDuplicator.Services;

public class CodeProcessor
{
    private readonly CodeRewriter _rewriter;

    public CodeProcessor(CodeRewriter rewriter)
    {
        _rewriter = rewriter;
    }

    private CSharpSyntaxNode ProcessSyntaxTree(String inputPath)
    {
        CSharpSyntaxNode root = FileHelper.GetSyntaxTreeRoot(inputPath);
        return (CSharpSyntaxNode) _rewriter.Visit(root);
    }

    public void ProcessAndSaveSyntaxTree(string inputFilePath)
    {
        CSharpSyntaxNode processedRoot = ProcessSyntaxTree(inputFilePath);
        FileHelper.RecordOutputFile(processedRoot,  inputFilePath);
    }
}