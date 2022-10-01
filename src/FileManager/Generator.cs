using FileManager.Interfaces;
using Microsoft.Extensions.Logging;

namespace FileManager;

public class Generator : IFileGenerator
{
    private readonly GenerationOptions _options;
    private readonly ILogger? _logger;

    public Generator() : this(new GenerationOptions())
    {
    }
    
    public Generator(GenerationOptions options)
    {
        _options = options;
        _logger = options.Logger;
    }

    public string Create(int linesNumber, CancellationToken cancellationToken)
    {
        _logger?.LogInformation("File generation started");
        
        if (linesNumber < 1)
        {
            throw new ArgumentException("Lines number less than 1");
        }

        var filename = $"file_{Guid.NewGuid():D}.txt";
        var filepath = Path.Combine(_options.OutputDirectory, filename);
        
        using var writer = new StreamWriter(filepath);

        foreach (var line in _options.LineGenerator.NextLine(linesNumber))
        {
            cancellationToken.ThrowIfCancellationRequested();
            writer.WriteLine(line);
        }

        return filepath;
    }
}