using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using ParameterDuplicator.Utils;

namespace ParameterDuplicator.Rewriters;

public class CodeRewriter: CSharpSyntaxRewriter
{
    // Rewrite the method declaration to duplicate the parameter
    public override SyntaxNode? VisitMethodDeclaration(MethodDeclarationSyntax node)
    {
        // If the method has only one parameter, duplicate it
        if (node.ParameterList.Parameters.Count == 1)
        {
            var originalParam = node.ParameterList.Parameters[0];
            var newParam = SyntaxFactory.Parameter(
                SyntaxFactory.Identifier(originalParam.Identifier+Constants.METHOD_PARAMETER_SUFFIX)).
                WithType(originalParam.Type);
            var newParamList = node.ParameterList.AddParameters(newParam);
            node = node.WithParameterList(newParamList);
        }

        return base.VisitMethodDeclaration(node);
    }
    
    // Rewrite the class declaration to add the "Processed" prefix to the class name
    public override SyntaxNode? VisitClassDeclaration(ClassDeclarationSyntax node)
    {
        var newName = Constants.PROCESSED_PREFIX + node.Identifier.ToString();
        var newIdentifier = SyntaxFactory.Identifier(newName);
        var processedNode = node.WithIdentifier(newIdentifier);
            
        return base.VisitClassDeclaration(processedNode);
    }
}