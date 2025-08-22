using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Azure.Identity;
using System;

var builder = FunctionsApplication.CreateBuilder(args);

builder.ConfigureFunctionsWebApplication();

builder.Services
    .AddApplicationInsightsTelemetryWorkerService()
    .ConfigureFunctionsApplicationInsights();

builder.Configuration
    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
    .AddEnvironmentVariables();

var environment = Environment.GetEnvironmentVariable("AZURE_FUNCTIONS_ENVIRONMENT")
                  ?? Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT")
                  ?? "Production";

if (string.Equals(environment, "Production", StringComparison.OrdinalIgnoreCase))
{
    var keyVaultName = builder.Configuration["KeyVaultName"];
    if (!string.IsNullOrWhiteSpace(keyVaultName))
    {
        var uri = new Uri($"https://{keyVaultName}.vault.azure.net/");
        builder.Configuration.AddAzureKeyVault(uri, new DefaultAzureCredential());
    }
}

builder.Build().Run();
