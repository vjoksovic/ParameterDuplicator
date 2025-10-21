using ParameterDuplicator.Rewriters;
using ParameterDuplicator.Services;
using ParameterDuplicator.Utils;

if (args.Length < 1)
{
    Console.WriteLine("Usage: dotnet run <input-file>");
    return;
}

var rewriter = new CodeRewriter();
var  processor = new CodeProcessor(rewriter);

var filePath = args[0];
processor.ProcessAndSaveSyntaxTree(filePath);
