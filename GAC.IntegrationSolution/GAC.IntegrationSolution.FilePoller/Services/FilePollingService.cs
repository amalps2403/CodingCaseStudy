using Cronos;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using GAC.IntegrationSolution.FilePoller.Models;
using System.Net.Http;
using System.Text;
using System.Xml.Serialization;
using Newtonsoft.Json;
using GAC.IntegrationSolution.FilePoller.Settings;
using Microsoft.Extensions.Options;
using GAC.IntegrationSolution.FilePoller.DTOs;
using System.Runtime;
using Polly;

namespace GAC.IntegrationSolution.FilePoller.Services
{
    public class FilePollingService : BackgroundService
    {
        private readonly ILogger<FilePollingService> _logger;
        private readonly FilePollingSettings _settings;
        private readonly WmsApiSettings _apiSettings;
        private readonly CronExpression _cron;
        private DateTime _nextRun;
        private readonly IHttpClientFactory _httpClientFactory;

        private readonly string _watchFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "SampleFiles");


        public FilePollingService(
             ILogger<FilePollingService> logger,
             IOptions<FilePollingSettings> settings,
             IOptions<WmsApiSettings> apiSettings,
             IHttpClientFactory httpClientFactory)
        {
            _logger = logger;
            _settings = settings.Value;
            _apiSettings = apiSettings.Value;
            _cron = CronExpression.Parse(_settings.CronSchedule);
            _nextRun = DateTime.UtcNow;
            _httpClientFactory = httpClientFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Polling service started");
            while (!stoppingToken.IsCancellationRequested)
            {
                var now = DateTime.UtcNow;
                if (_nextRun <= now)
                {
                    await PollAndProcessFilesAsync();
                    _nextRun = _cron.GetNextOccurrence(DateTime.UtcNow) ?? DateTime.UtcNow.AddMinutes(1);
                }
                await Task.Delay(5000, stoppingToken);
            }
        }

        //private async Task PollAndProcessFilesAsync()
        //{
        //    var fullPath = Path.Combine(AppContext.BaseDirectory, _settings.WatchFolder);
        //    if (!Directory.Exists(fullPath))
        //    {
        //        _logger.LogWarning("Watch folder not found: {Folder}", _settings.WatchFolder);
        //        return;
        //    }
        //    _settings.WatchFolder = fullPath;

        //    var files = Directory.GetFiles(_settings.WatchFolder, "*.xml");
        //    foreach (var file in files)
        //    {
        //        try
        //        {
        //            _logger.LogInformation("Processing: {file}", file);
        //            using var stream = File.OpenRead(file);
        //            var serializer = new XmlSerializer(typeof(PurchaseOrderDto));
        //            var po = (PurchaseOrderDto?)serializer.Deserialize(stream);

        //            if (po != null)
        //            {
        //                var json = System.Text.Json.JsonSerializer.Serialize(po);
        //                var client = _httpClientFactory.CreateClient();
        //                var content = new StringContent(json, Encoding.UTF8, "application/json");
        //                var response = await client.PostAsync($"{_apiSettings.Endpoint}wms/orders", content);
        //                response.EnsureSuccessStatusCode();

        //                _logger.LogInformation("Posted successfully: {file}", file);
        //            }
        //            File.Move(file, file + ".done");
        //        }
        //        catch (Exception ex)
        //        {
        //            _logger.LogError(ex, "Error processing file: {file}", file);
        //        }
        //    }
        //}


        private async Task PollAndProcessFilesAsync()
        {
            var fullPath = Path.Combine(AppContext.BaseDirectory, _settings.WatchFolder);
            if (!Directory.Exists(fullPath))
            {
                _logger.LogWarning("Watch folder not found: {Folder}", _settings.WatchFolder);
                return;
            }
            _settings.WatchFolder = fullPath;


            // instead of hardcoding the retry count we can change it to a configuration value
            var retryPolicy = Policy
                .Handle<HttpRequestException>()
                .OrResult<HttpResponseMessage>(r => !r.IsSuccessStatusCode)
                .WaitAndRetryAsync(
                    retryCount: 3,
                    sleepDurationProvider: attempt => TimeSpan.FromSeconds(Math.Pow(2, attempt)),
                    onRetry: (outcome, timespan, attempt, context) =>
                    {
                        _logger.LogWarning("Retry {Attempt} for file due to: {Reason}", attempt,
                            outcome.Exception?.Message ?? outcome.Result?.StatusCode.ToString());
                    });

            var files = Directory.GetFiles(_settings.WatchFolder, "*.xml");
            foreach (var file in files)
            {
                try
                {
                    _logger.LogInformation("Processing: {file}", file);
                    using var stream = File.OpenRead(file);
                    var serializer = new XmlSerializer(typeof(PurchaseOrderDto));
                    var po = (PurchaseOrderDto?)serializer.Deserialize(stream);

                    if (po != null)
                    {
                        var json = System.Text.Json.JsonSerializer.Serialize(po);
                        var client = _httpClientFactory.CreateClient();
                        var content = new StringContent(json, Encoding.UTF8, "application/json");

                        var response = await retryPolicy.ExecuteAsync(() =>
                            client.PostAsync($"{_apiSettings.Endpoint}wms/orders", content));

                        if (response.IsSuccessStatusCode)
                        {
                            _logger.LogInformation("Posted successfully: {file}", file);
                            // commenting for testing purpose
                            // File.Move(file, file + ".done", overwrite: true);
                        }
                        else
                        {
                            throw new HttpRequestException($"API returned {response.StatusCode}");
                        }
                        _logger.LogInformation("Posted successfully: {file}", file);
                    }

                    File.Move(file, file + ".done");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error processing file: {file}", file);
                }
            }
        }
    }
}

