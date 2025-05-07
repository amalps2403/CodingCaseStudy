using Cronos;
using GAC.IntegrationSolution.FilePoller.Helpers;
using GAC.IntegrationSolution.FilePoller.Models;
using GAC.IntegrationSolution.FilePoller.Settings;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Quartz;
using System;
using System.IO;
using System.Runtime;
using System.Threading;
using System.Threading.Tasks;

namespace GAC.IntegrationSolution.FilePoller
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly Cronos.CronExpression _cronExpression;
        private DateTime _nextRun;
        private readonly FilePollingSettings _settings;

        public Worker(ILogger<Worker> logger)
        {
            _logger = logger;
            _cronExpression = Cronos.CronExpression.Parse("*/1 * * * *"); // Every minute
            _nextRun = DateTime.UtcNow;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Worker started at: {time}", DateTimeOffset.Now);

            while (!stoppingToken.IsCancellationRequested)
            {
                var now = DateTime.UtcNow;
                if (_nextRun <= now)
                {
                    await PollDirectoryAsync();
                    _nextRun = _cronExpression.GetNextOccurrence(DateTime.UtcNow) ?? DateTime.UtcNow.AddMinutes(1);
                }

                await Task.Delay(5000, stoppingToken); // Check every 5 seconds
            }
        }

        private async Task PollDirectoryAsync()
        {

            var folderPath = Path.Combine(AppContext.BaseDirectory, _settings.WatchFolder);

            if (!Directory.Exists(folderPath))
            {
                _logger.LogWarning("Watch folder not found: {folder}", folderPath);
                return;
            }

            var xmlFiles = Directory.GetFiles(folderPath, "*.xml");
            foreach (var file in xmlFiles)
            {
                try
                {
                    _logger.LogInformation("Processing file: {file}", file);
                    var purchaseOrder = XmlHelper.Deserialize<PurchaseOrderXmlModel>(file);

                    // TODO: Transform to domain model and call REST API here

                    _logger.LogInformation("Parsed PO ID: {id}, Customer: {cust}", purchaseOrder.Id, purchaseOrder.Customer);

                    File.Move(file, file + ".processed"); // Rename to mark as processed
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Failed to process file: {file}", file);
                }
            }
        }
    }
}
