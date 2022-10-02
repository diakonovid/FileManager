using BenchmarkDotNet.Attributes;

namespace FileManager.Benchmark;

[SimpleJob]
[MemoryDiagnoser]
public class SorterTest
{
    [Params("100MB")]
    public string Size;

    private string _filename;

    [GlobalSetup]
    public void GlobalSetup()
    {
        _filename = $"Data/{Size}.txt";
    }

    [Benchmark]
    public string ExternalSorter()
    {
        var options = new SortingOptions();
        return new ExternalFileSorter(options).Sort(_filename, CancellationToken.None);
    }
}