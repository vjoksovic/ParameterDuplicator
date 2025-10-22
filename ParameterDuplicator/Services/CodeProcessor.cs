using Microsoft.CodeAnalysis.CSharp;
using ParameterDuplicator.Rewriters;
using ParameterDuplicator.Utils;
using ParameterDuplicator.Interfaces;

namespace ParameterDuplicator.Services;

// CodeProcessor is the service that processes the syntax tree of the input file
// It implements the IProcessInterface interface
public class CodeProcessor : IProcessInterface
{
    private readonly CodeRewriter _rewriter;

    // Constructor to inject the rewriter
    public CodeProcessor(CodeRewriter rewriter)
    {
        _rewriter = rewriter;
    }

    public CSharpSyntaxNode ProcessSyntaxTree(string inputPath)
    {
        var root = FileHelper.GetSyntaxTreeRoot(inputPath);
        return (CSharpSyntaxNode) _rewriter.Visit(root);
    }

    public void ProcessAndSaveSyntaxTree(string inputFilePath)
    {
        var processedRoot = ProcessSyntaxTree(inputFilePath);
        FileHelper.RecordOutputFile(processedRoot,  inputFilePath);
    }
}