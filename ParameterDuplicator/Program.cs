using ParameterDuplicator.Rewriters;
using ParameterDuplicator.Processors;

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
    
    var rewriter = new CodeRewriter();
    var processor = new CodeProcessor(rewriter);
    
    processor.ProcessAndSaveSyntaxTree(filePath);
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
