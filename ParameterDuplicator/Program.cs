using ParameterDuplicator.Interfaces;
using ParameterDuplicator.Rewriters;
using ParameterDuplicator.Services;
using ParameterDuplicator.Utils;

try
{
    // Check if the input file is provided
    if (args.Length < 1 || string.IsNullOrWhiteSpace(args[0]))
    {
        Console.WriteLine("Usage: dotnet run <input-file>");
        Console.WriteLine("Example: dotnet run DataService.cs");
        return;
    }

    var filePath = args[0];
    Console.WriteLine($"Processing file: {filePath}");

    // Create a new CodeRewriter instance
    var rewriter = new CodeRewriter();
    // Create a new IProcessInterface implementation with the rewriter
    var processor = new CodeProcessor(rewriter);

    // Process the syntax tree of the input file and save the output to the output file
    processor.ProcessAndSaveSyntaxTree(filePath);
    
    Console.WriteLine($"Successfully processed {filePath}");
    Console.WriteLine($"Output saved to: ./Examples/OutputFiles/Processed{filePath}");
}
catch (ArgumentException ex)
{
    Console.WriteLine($"Invalid argument: {ex.Message}");
    Environment.Exit(1);
}
catch (FileNotFoundException ex)
{
    Console.WriteLine($"File not found: {ex.Message}");
    Environment.Exit(1);
}
catch (InvalidOperationException ex)
{
    Console.WriteLine($"Processing error: {ex.Message}");
    Environment.Exit(1);
}
catch (Exception ex)
{
    Console.WriteLine($"Unexpected error: {ex.Message}");
    Environment.Exit(1);
}
