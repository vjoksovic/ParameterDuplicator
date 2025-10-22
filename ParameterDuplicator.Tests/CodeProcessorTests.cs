using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using ParameterDuplicator.Rewriters;
using ParameterDuplicator.Services;
using ParameterDuplicator.Utils;

namespace ParameterDuplicator.Tests;

[TestFixture]
public class CodeProcessorTests
{
    private CodeProcessor _processor;
    private CodeRewriter _rewriter;

    [SetUp]
    public void Setup()
    {
        _rewriter = new CodeRewriter();
        _processor = new CodeProcessor(_rewriter);
    }
    
    [Test]
    public void ProcessSyntaxTree_WithValidFile_ShouldReturnProcessedSyntaxNode()
    {
        const string testFileName = "DataService.cs";
        CreateTestInputFile(testFileName);

        try
        {
            var result = _processor.ProcessSyntaxTree(testFileName);
            
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.InstanceOf<CSharpSyntaxNode>());
            
            // Verify the processing worked by checking for the processed class name
            var classes = result.DescendantNodes().OfType<ClassDeclarationSyntax>().ToList();
            Assert.That(classes.Count, Is.EqualTo(1));
            Assert.That(classes[0].Identifier.ValueText, Is.EqualTo(Constants.PROCESSED_PREFIX + "DataService"));
        }
        finally
        {
            CleanupTestFile(testFileName);
        }
    }

    [Test]
    public void ProcessSyntaxTree_WithNonExistentFile_ShouldThrowFileNotFoundException()
    {
        Assert.Throws<FileNotFoundException>(() => _processor.ProcessSyntaxTree("NonExistentFile.cs"));
    }

    [Test]
    public void ProcessSyntaxTree_WithEmptyFileName_ShouldThrowArgumentException()
    {
        Assert.Throws<ArgumentException>(() => _processor.ProcessSyntaxTree(""));
        Assert.Throws<ArgumentException>(() => _processor.ProcessSyntaxTree("   "));
    }

    [Test]
    public void ProcessSyntaxTree_WithNullFileName_ShouldThrowArgumentException()
    {
        Assert.Throws<ArgumentException>(() => _processor.ProcessSyntaxTree(null!));
    }

    [Test]
    public void ProcessAndSaveSyntaxTree_WithValidFile_ShouldProcessAndSaveFile()
    {
        const string testFileName = "TestService.cs";
        CreateTestInputFile(testFileName);

        try
        {
            _processor.ProcessAndSaveSyntaxTree(testFileName);
            
            var outputPath = Constants.OUTPUT_FILE_PATH + Constants.PROCESSED_PREFIX + testFileName;
            Assert.That(File.Exists(outputPath), Is.True);

            var outputContent = File.ReadAllText(outputPath);
            Assert.That(outputContent, Does.Contain(Constants.PROCESSED_PREFIX + "TestService"));
            Assert.That(outputContent, Does.Contain("id" + Constants.METHOD_PARAMETER_SUFFIX));
        }
        finally
        {
            CleanupTestFile(testFileName);
            CleanupTestOutputFile(testFileName);
        }
    }

    [Test]
    public void ProcessAndSaveSyntaxTree_WithNonExistentFile_ShouldThrowFileNotFoundException()
    {
        Assert.Throws<FileNotFoundException>(() => _processor.ProcessAndSaveSyntaxTree("NonExistentFile.cs"));
    }

    [Test]
    public void ProcessAndSaveSyntaxTree_WithEmptyFileName_ShouldThrowArgumentException()
    {
        Assert.Throws<ArgumentException>(() => _processor.ProcessAndSaveSyntaxTree(""));
        Assert.Throws<ArgumentException>(() => _processor.ProcessAndSaveSyntaxTree("   "));
    }

    [Test]
    public void ProcessAndSaveSyntaxTree_WithNullFileName_ShouldThrowArgumentException()
    {
        Assert.Throws<ArgumentException>(() => _processor.ProcessAndSaveSyntaxTree(null!));
    }

    [Test]
    public void ProcessSyntaxTree_ShouldPreserveMethodSignaturesCorrectly()
    {
        // Arrange
        const string testFileName = "ComplexService.cs";
        const string testCode = @"
        public class ComplexService
        {
            public void SingleParam(int id) { }
            public void MultipleParams(int id, string name) { }
            public void NoParams() { }
            public async Task<string> AsyncMethod(int id) { return ""test""; }
        }";
        CreateTestInputFile(testFileName, testCode);

        try
        {
            var result = _processor.ProcessSyntaxTree(testFileName);
            
            var methods = result.DescendantNodes().OfType<MethodDeclarationSyntax>().ToList();
            Assert.That(methods.Count, Is.EqualTo(4));

            // Single parameter method should be duplicated
            var singleParamMethod = methods.First(m => m.Identifier.ValueText == "SingleParam");
            Assert.That(singleParamMethod.ParameterList.Parameters.Count, Is.EqualTo(2));
            Assert.That(singleParamMethod.ParameterList.Parameters[0].Identifier.ValueText, Is.EqualTo("id"));
            Assert.That(singleParamMethod.ParameterList.Parameters[1].Identifier.ValueText, Is.EqualTo("id" + Constants.METHOD_PARAMETER_SUFFIX));

            // Multiple parameters method should remain unchanged
            var multipleParamsMethod = methods.First(m => m.Identifier.ValueText == "MultipleParams");
            Assert.That(multipleParamsMethod.ParameterList.Parameters.Count, Is.EqualTo(2));
            Assert.That(multipleParamsMethod.ParameterList.Parameters[0].Identifier.ValueText, Is.EqualTo("id"));
            Assert.That(multipleParamsMethod.ParameterList.Parameters[1].Identifier.ValueText, Is.EqualTo("name"));

            // No parameters method should remain unchanged
            var noParamsMethod = methods.First(m => m.Identifier.ValueText == "NoParams");
            Assert.That(noParamsMethod.ParameterList.Parameters.Count, Is.EqualTo(0));

            // Async method should be processed
            var asyncMethod = methods.First(m => m.Identifier.ValueText == "AsyncMethod");
            Assert.That(asyncMethod.ParameterList.Parameters.Count, Is.EqualTo(2));
            Assert.That(asyncMethod.ParameterList.Parameters[0].Identifier.ValueText, Is.EqualTo("id"));
            Assert.That(asyncMethod.ParameterList.Parameters[1].Identifier.ValueText, Is.EqualTo("id" + Constants.METHOD_PARAMETER_SUFFIX));
        }
        finally
        {
            CleanupTestFile(testFileName);
        }
    }

    private void CreateTestInputFile(string fileName, string content = null)
    {
        var inputDir = Constants.INPUT_FILE_PATH;
        if (!Directory.Exists(inputDir))
        {
            Directory.CreateDirectory(inputDir);
        }

        var defaultContent = @"
        public class " + fileName.Replace(".cs", "") + @"
        {
            public async Task FetchDataAsync(int id)
            {
                await Task.Delay(1000);
            }

            private void LogError(string error)
            {
                System.Console.WriteLine(error);
            }

            public void Reset() { }
        }";

        File.WriteAllText(Path.Combine(inputDir, fileName), content ?? defaultContent);
    }

    private void CleanupTestFile(string fileName)
    {
        var inputPath = Path.Combine(Constants.INPUT_FILE_PATH, fileName);
        if (File.Exists(inputPath))
        {
            File.Delete(inputPath);
        }
    }

    private void CleanupTestOutputFile(string fileName)
    {
        var outputPath = Constants.OUTPUT_FILE_PATH + Constants.PROCESSED_PREFIX + fileName;
        if (File.Exists(outputPath))
        {
            File.Delete(outputPath);
        }
    }
}
