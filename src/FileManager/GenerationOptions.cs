using FileManager.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace FileManager;

public class GenerationOptions
{
    public GenerationOptions()
    {
        LineGenerator = new LineGenerator(MinWordsPerLine, MaxWordsPerLine);
    }
    
    public string OutputDirectory { get; init; } = Directory.GetCurrentDirectory();
    public int MaxWordsPerLine { get; init; } = 10;
    public int MinWordsPerLine { get; init; } = 3;
    public ILineGenerator LineGenerator { get; init; }
    
    public ILogger Logger { get; init; } = new NullLogger<IFileGenerator>();
}