using Microsoft.CodeAnalysis.CSharp;
using ParameterDuplicator.Rewriters;
using ParameterDuplicator.Utils;

namespace ParameterDuplicator.Processors;

// CodeProcessor is the service that processes the syntax tree of the input file
public class CodeProcessor : BaseProcessor
{
    private readonly CodeRewriter _rewriter;
    
    public CodeProcessor(CodeRewriter rewriter)
    {
        _rewriter = rewriter;
    }

    public override CSharpSyntaxNode ProcessSyntaxTree(string inputPath)
    {
        var root = FileHelper.GetSyntaxTreeRoot(inputPath);
        return (CSharpSyntaxNode) _rewriter.Visit(root);
    }
    
}