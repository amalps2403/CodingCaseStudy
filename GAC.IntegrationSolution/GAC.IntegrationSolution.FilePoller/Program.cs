using GAC.IntegrationSolution.FilePoller.Services;
using GAC.IntegrationSolution.FilePoller.Settings;

;


var builder = Host.CreateDefaultBuilder(args)
    .ConfigureServices((context, services) =>
    {
        services.Configure<FilePollingSettings>(context.Configuration.GetSection("FilePollingSettings"));
        services.Configure<WmsApiSettings>(context.Configuration.GetSection("WmsApiSettings"));
        services.AddHttpClient();
        services.AddHostedService<FilePollingService>();
    });

await builder.RunConsoleAsync();