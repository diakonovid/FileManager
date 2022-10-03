using System.Runtime.CompilerServices;
using FileManager.Interfaces;
using Microsoft.Extensions.Logging;

[assembly: InternalsVisibleTo("FileManager.Tests")]
namespace FileManager;

/// <summary>
/// Implementation of external file sorting
/// </summary>
public class ExternalFileSorter : IFileSorter
{
    private readonly SortingOptions _options;
    private readonly ILogger? _logger;

    public ExternalFileSorter() : this(new SortingOptions())
    {
    }
    
    public ExternalFileSorter(SortingOptions options)
    {
        _options = options;
        _logger = _options.Logger;
    }
    
    /// <summary>
    /// Sorts the lines of a file. The sort compares the lines to each
    /// other using the IComparable interface, which can be overridden
    /// by changing SortingOptions's property.
    /// </summary>
    /// <param name="fileName">Absolute or relative path for the specified file</param>
    /// <param name="cancellationToken">Sort cancellation token</param>
    /// <exception cref="ArgumentException">File validation</exception>
    /// <exception cref="OperationCanceledException">Requested cancellation</exception>
    /// <returns>Absolute path for the sorted file</returns>
    public string Sort(string fileName, CancellationToken cancellationToken)
    {
        try
        {
            var files = Split(fileName, cancellationToken);
            return Merge(files, cancellationToken);
        }
        catch (Exception e)
        {
            _logger?.LogError(e, "Error detected");
            throw;
        }
    }
    
    internal List<string> Split(string fileName, CancellationToken cancellationToken)
    {
        _logger?.LogInformation("Split started");
        
        if (string.IsNullOrWhiteSpace(fileName))
        {
            throw new ArgumentException("Empty file name");
        }
        
        var list = new List<string>();
        
        var lines = new string[_options.MaxLinesNumberPerFile];
        
        using var reader = new StreamReader(fileName);

        var nameWithoutExtension = Path.GetFileNameWithoutExtension(fileName);
        var extension = Path.GetExtension(fileName);

        var linePointer = 0;
        while (!reader.EndOfStream)
        {
            cancellationToken.ThrowIfCancellationRequested();
            
            lines[linePointer] = reader.ReadLine();
            linePointer++;
            if (linePointer == _options.MaxLinesNumberPerFile || reader.EndOfStream)
            {
                var sortedLines = lines.AsParallel().OrderBy(x=>x, _options.Comparer);
                
                var partFileName = $"{nameWithoutExtension}_{list.Count + 1}{extension}";
                var partFilepath = Path.Combine(_options.OutputDirectory, partFileName);
                WriteAllLines(partFilepath, sortedLines, linePointer);
                
                list.Add(partFilepath);
                linePointer = 0;
            }
        }

        return list;
    }
    
    internal string Merge(List<string> files, CancellationToken cancellationToken)
    {
        _logger?.LogInformation("Merge started");

        if (files.Count == 0)
        {
            throw new ArgumentException("Empty files list");
        }
        
        var fileName = Path.Combine(_options.OutputDirectory, $"result_{Guid.NewGuid():D}.txt");

        if (files.Count == 1)
        { 
            File.Move(files[0], fileName);
            File.Delete(files[0]);
            return fileName;
        }
        
        var queue = new PriorityQueue<int, string?>(_options.Comparer);
        var readers = new StreamReader[files.Count];

        for (var i = 0; i < files.Count; i++)
        {
            readers[i] = new StreamReader(files[i]);
            queue.Enqueue(i, readers[i].ReadLine());
        }

        try
        {
            using var writer = new StreamWriter(fileName);

            while (queue.TryDequeue(out var item, out var key))
            {
                cancellationToken.ThrowIfCancellationRequested();
                
                writer.WriteLine(key);

                if (!readers[item].EndOfStream)
                {
                    var line = readers[item].ReadLine();
                    queue.Enqueue(item, line);
                }
            }
        }
        finally
        {
            for (var i = 0; i < files.Count; i++)
            {
                readers[i].Dispose();
                File.Delete(files[i]);
            }
        }

        return fileName;
    }

    private void WriteAllLines(string fileName, IEnumerable<string> lines, int pointer)
    {
        using var writer = new StreamWriter(fileName);
        foreach (var line in lines)
        {
            if (pointer <= 0)
            {
                break;
            }
            writer.WriteLine(line);
            pointer--;
        }
        _logger?.LogInformation("Temporary file {Name} created", fileName);
    }
}