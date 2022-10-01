using FileManager;
using FileManager.Interfaces;
using Microsoft.Extensions.Logging;
using Serilog;

using var loggerFactory = LoggerFactory.Create(builder =>
{
    Log.Logger = new LoggerConfiguration()
        .MinimumLevel.Debug()
        .WriteTo.Console(outputTemplate:"[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}")
        .CreateLogger();
    
    builder.AddSerilog(Log.Logger);
});

var logger = loggerFactory.CreateLogger<Program>();

if (args.Length != 2)
{
    logger.LogError("You must pass the filename and output directory");
    return;
}

var cts = new CancellationTokenSource();
Console.CancelKeyPress += (s, e) =>
{
    logger.LogInformation("Canceling...");
    cts.Cancel();
    e.Cancel = true;
};

try
{
    var options = new SortingOptions
    {
        Logger = loggerFactory.CreateLogger<IFileSorter>(),
        OutputDirectory= args[1]
    };
    
    IFileSorter sorter = new ExternalFileSorter(options);
    sorter.Sort(args[0], cts.Token);
    logger.LogInformation("Sorting completed");
}
catch (OperationCanceledException e)
{
    logger.LogInformation(e.Message);
}
catch (Exception e)
{
    logger.LogError(e,"Error detected");
}