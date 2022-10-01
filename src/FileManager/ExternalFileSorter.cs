using System.Runtime.CompilerServices;
using FileManager.Interfaces;
using Microsoft.Extensions.Logging;

[assembly: InternalsVisibleTo("FileManager.Tests")]
namespace FileManager;

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
                var partFileName = $"{nameWithoutExtension}_{list.Count + 1}{extension}";
                var partFilepath = Path.Combine(_options.OutputDirectory, partFileName);
                
                list.Add(partFilepath);
                Array.Sort(lines,0, linePointer, _options.Comparer);
                WriteAllLines(partFilepath, lines);
                
                linePointer = 0;
                _logger?.LogInformation("Temporary file {Name} created", partFileName);
            }
        }

        return list;
    }
    
    internal string Merge(List<string> files, CancellationToken cancellationToken)
    {
        _logger?.LogInformation("Merge started");

        if (files.Count == 0)
        {
            throw new AggregateException("Empty files list");
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

    private void WriteAllLines(string fileName, IEnumerable<string> lines)
    {
        using var writer = new StreamWriter(fileName);
        foreach (var line in lines)
        {
            writer.WriteLine(line);
        }
    }
}