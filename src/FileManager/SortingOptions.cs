using FileManager.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace FileManager;

public class SortingOptions
{
    public string OutputDirectory { get; init; } = Directory.GetCurrentDirectory();
    public int MaxLinesNumberPerFile { get; init; } = 1000000;
    public IComparer<string?> Comparer { get; init; } = new LineComparer();
    public ILogger Logger { get; init; } = new NullLogger<IFileSorter>();
}