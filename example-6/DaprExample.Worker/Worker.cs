
using DaprClient = Dapr.Client.DaprClient;


namespace DaprExample.Worker;

public class Worker : BackgroundService
{
    private readonly DaprClient _daprClient;
    private readonly ILogger<Worker> _logger;

    public Worker(
        DaprClient daprClient,
        ILogger<Worker> logger)
    {
        _daprClient = daprClient;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                var statusMessage = await _daprClient.InvokeMethodAsync<Result>(HttpMethod.Get, "example6-api", "status", stoppingToken);

                _logger.LogInformation(statusMessage.Message, DateTimeOffset.Now);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }

            await Task.Delay(1000, stoppingToken);
        }
    }
}

public class Result
{
    public string Message { get; set; }
}