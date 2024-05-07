
namespace DaprExample.Worker;

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
            try
            {
                var statusEndpointUri = _configuration.GetValue<string>("STATUS_ENDPOINT_URI");

                using var httpClient = new HttpClient();
                var statusMessage = await httpClient.GetStringAsync(statusEndpointUri, stoppingToken);

                _logger.LogInformation(statusMessage, DateTimeOffset.Now);
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, exception.Message);
            }

            await Task.Delay(1000, stoppingToken);
        }
    }
}