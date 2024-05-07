
using DaprClient = Dapr.Client.DaprClient;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddEnvironmentVariables();

builder.Services.AddDaprClient();

var app = builder.Build();

app.UseHttpsRedirection();

app.MapGet("/status", async (IConfiguration configuration, DaprClient client) =>
{
    var cachedMessage = await client.GetStateAsync<string>("redis", "message");
    if (cachedMessage != null)
    {
        return new Result { Message = cachedMessage };
    }

    var version = configuration.GetValue<string>("VERSION");
    var message = $"Hello from version '{version}' on '{Environment.MachineName}' at '{DateTime.Now}'";

    await client.SaveStateAsync("redis", "message", message);

    return new Result { Message = message };
});

app.Run();

public class Result
{
    public string Message { get; set; }
}