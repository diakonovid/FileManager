namespace FileManager.Tests;

public class SorterTests
{
    private ExternalFileSorter _sorter;
        
    public SorterTests()
    {
        var options = new SortingOptions
        {
            MaxLinesNumberPerFile = 5
        };
        
        _sorter = new ExternalFileSorter(options);
    }
    
    [Fact]
    public void SplitFile_Successful()
    {
        const string filename = "Data/test.txt";

        var files = _sorter.Split(filename, CancellationToken.None);
        
        for (var i = 0; i < files.Count; i++)
        {
            var resultLines = File.ReadAllLines(files[i]);
            var expectedLines = File.ReadAllLines($"Data/test_{i+1}.txt");
            Assert.Equal(expectedLines[i], resultLines[i]);
        }
    }
    
    [Fact]
    public void MergeFiles_Successful()
    {
        var files = new List<string>
        {
            "Data/test_1.txt",
            "Data/test_2.txt"
        };
        
        var resultFileName = _sorter.Merge(files, CancellationToken.None);

        var resultLines = File.ReadAllLines(resultFileName);
        var expectedLines = File.ReadAllLines("Data/correct.txt");

        for (var i = 0; i < expectedLines.Length; i++)
        {
            Assert.Equal(expectedLines[i], resultLines[i]);
        }
    }
}