
using DaprExample.Worker;


var builder = Host.CreateApplicationBuilder(args);
builder.Configuration.AddEnvironmentVariables();
builder.Services.AddHostedService<Worker>();
builder.Services.AddDaprClient();

var host = builder.Build();
host.Run();