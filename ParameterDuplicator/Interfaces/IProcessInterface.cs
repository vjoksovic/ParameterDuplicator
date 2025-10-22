using Microsoft.CodeAnalysis.CSharp;

namespace ParameterDuplicator.Interfaces;

public interface IProcessInterface
{

    // Process the syntax tree of the input file
    CSharpSyntaxNode ProcessSyntaxTree(string inputPath);
    
    // Process the syntax tree of the input file and save the output to the output file
    public void ProcessAndSaveSyntaxTree(string inputFilePath);
    
}