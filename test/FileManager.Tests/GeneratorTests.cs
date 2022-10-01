using FileManager.Interfaces;

namespace FileManager.Tests;

public class GeneratorTests
{
    private readonly ILineGenerator _lineGenerator;
        
    public GeneratorTests()
    {
        _lineGenerator = new LineGenerator(3, 5);
    }

    [Fact]
    public void CreateLines_Successful()
    {
        var lines = new List<string>();
        
        foreach (var line in _lineGenerator.NextLine(5))
        {
            Assert.Matches(@"^\d+\. .*$", line);
            lines.Add(line);
        }

        Assert.Equal(5, lines.Count);
    }
}