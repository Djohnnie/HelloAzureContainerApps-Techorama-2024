namespace SimpleExample.Worker;

public class Worker : BackgroundService
{
    private readonly IConfiguration _configuration;
    private readonly ILogger<Worker> _logger;

    public Worker(
        IConfiguration configuration,
        ILogger<Worker> logger)
    {
        _configuration = configuration;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            var statusEndpointUri = _configuration.GetValue<string>("STATUS_ENDPOINT_URI");

            using var httpClient = new HttpClient();
            var statusMessage = await httpClient.GetStringAsync(statusEndpointUri, stoppingToken);

            _logger.LogInformation(statusMessage, DateTimeOffset.Now);

            await Task.Delay(1000, stoppingToken);
        }
    }
}