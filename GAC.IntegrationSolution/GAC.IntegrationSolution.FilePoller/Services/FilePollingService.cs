using Cronos;
using System.Text;
using System.Xml.Serialization;
using GAC.IntegrationSolution.FilePoller.Settings;
using Microsoft.Extensions.Options;
using GAC.IntegrationSolution.FilePoller.DTOs;
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


        /// <summary>
        /// Polls the watch folder for XML files, processes them, and sends data to the WMS API.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="HttpRequestException"></exception>
        private async Task PollAndProcessFilesAsync()
        {
            var fullPath = Path.Combine(AppContext.BaseDirectory, _settings.WatchFolder); // Combine the base directory with the watch folder path.
            if (!Directory.Exists(fullPath)) // Check if the watch folder exists.
            {
                _logger.LogWarning("Watch folder not found: {Folder}", _settings.WatchFolder); // Log a warning if the folder is missing.
                return; // Exit the method if the folder doesn't exist.
            }
            _settings.WatchFolder = fullPath; // Update the watch folder path to the full path.

            // Define a retry policy for handling transient HTTP errors.
            var retryPolicy = Policy
                .Handle<HttpRequestException>() // Handle HTTP request exceptions.
                .OrResult<HttpResponseMessage>(r => !r.IsSuccessStatusCode) // Handle unsuccessful HTTP responses.
                .WaitAndRetryAsync(
                    retryCount: 3, // Retry up to 3 times.
                    sleepDurationProvider: attempt => TimeSpan.FromSeconds(Math.Pow(2, attempt)), // Exponential backoff for retries.
                    onRetry: (outcome, timespan, attempt, context) =>
                    {
                        _logger.LogWarning("Retry {Attempt} for file due to: {Reason}", attempt,
                            outcome.Exception?.Message ?? outcome.Result?.StatusCode.ToString()); // Log each retry attempt.
                    });

            var files = Directory.GetFiles(_settings.WatchFolder, "*.xml"); // Get all XML files in the watch folder.
            foreach (var file in files) // Iterate through each file.
            {
                try
                {
                    _logger.LogInformation("Processing: {file}", file); // Log the file being processed.
                    using var stream = File.OpenRead(file); // Open the file for reading.
                    var serializer = new XmlSerializer(typeof(PurchaseOrderDto)); // Create an XML serializer for the PurchaseOrderDto type.
                    var po = (PurchaseOrderDto?)serializer.Deserialize(stream); // Deserialize the XML file into a PurchaseOrderDto object.

                    if (po != null) // Check if the deserialization was successful.
                    {
                        var json = System.Text.Json.JsonSerializer.Serialize(po); // Serialize the object to JSON.
                        var client = _httpClientFactory.CreateClient(); // Create an HTTP client instance.
                        var content = new StringContent(json, Encoding.UTF8, "application/json"); // Create the HTTP content with JSON payload.

                        var response = await retryPolicy.ExecuteAsync(() =>
                            client.PostAsync($"{_apiSettings.Endpoint}wms/orders", content)); // Send the JSON data to the WMS API with retry logic.

                        if (response.IsSuccessStatusCode) // Check if the API call was successful.
                        {
                            _logger.LogInformation("Posted successfully: {file}", file); // Log the success.
                                                                                         // File.Move(file, file + ".done", overwrite: true); // Uncomment to rename the file after successful processing.
                        }
                        else
                        {
                            throw new HttpRequestException($"API returned {response.StatusCode}"); // Throw an exception for unsuccessful responses.
                        }
                        _logger.LogInformation("Posted successfully: {file}", file); // Log the success again.
                    }

                    File.Move(file, file + ".done"); // Rename the file to mark it as processed.
                }
                catch (Exception ex) // Catch any exceptions during processing.
                {
                    _logger.LogError(ex, "Error processing file: {file}", file); // Log the error with the exception details.
                }
            }
        }
    }
}

