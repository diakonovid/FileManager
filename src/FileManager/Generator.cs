using FileManager.Interfaces;
using Microsoft.Extensions.Logging;

namespace FileManager;

/// <summary>
/// Implementation of file generator
/// </summary>
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

    /// <summary>
    /// Creates a random file. File line generation algorithm can be overridden
    /// by changing the GenerationOptions's property.
    /// </summary>
    /// <param name="linesNumber">Number of lines in the file to be created</param>
    /// <param name="cancellationToken">Cancel file creation</param>
    /// <exception cref="ArgumentException">Lines number validation</exception>
    /// <exception cref="OperationCanceledException">Requested cancellation</exception>
    /// <returns>Absolute path for the created file</returns>
    public string Create(int linesNumber, CancellationToken cancellationToken)
    {
        _logger?.LogInformation("File generation started");
        
        try
        {
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
        catch (Exception e)
        {
            _logger?.LogError(e, "Error detected");
            throw;
        }
    }
}