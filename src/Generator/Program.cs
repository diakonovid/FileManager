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
    var options = new GenerationOptions
    {
        OutputDirectory = args[1],
        Logger = loggerFactory.CreateLogger<IFileGenerator>()
    };

    var linesNumber = int.Parse(args[0]);
    IFileGenerator generator = new Generator(options);
    var filename = generator.Create(linesNumber, cts.Token);
    logger.LogInformation("Generated {0}", filename);
}
catch (Exception e)
{
    logger.LogError(e, "Error detected");
}