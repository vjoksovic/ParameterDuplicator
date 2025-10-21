using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace ParameterDuplicator.Rewriters;

public class CodeRewriter: CSharpSyntaxRewriter
{
    public override SyntaxNode? VisitMethodDeclaration(MethodDeclarationSyntax node)
    {
        if (node.ParameterList.Parameters.Count == 1)
        {
            var originalParam = node.ParameterList.Parameters[0];
            var newParam = SyntaxFactory.Parameter(SyntaxFactory.Identifier(originalParam.Identifier+"2")).
                WithType(originalParam.Type);
            var newParamList = node.ParameterList.AddParameters(newParam);
            node = node.WithParameterList(newParamList);
        }

        return base.VisitMethodDeclaration(node);
    }
    
    public override SyntaxNode? VisitClassDeclaration(ClassDeclarationSyntax node)
    {
        var newName = "Processed" + node.Identifier.ToString();
        var newIdentifier = SyntaxFactory.Identifier(newName);
        var processedNode = node.WithIdentifier(newIdentifier);
            
        return base.VisitClassDeclaration(processedNode);
    }
}