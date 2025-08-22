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

// Base config (local + env)
//builder.Configuration
//    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
//    .AddEnvironmentVariables();

//// Determine environment (Functions uses AZURE_FUNCTIONS_ENVIRONMENT / DOTNET_ENVIRONMENT)
//var environment = Environment.GetEnvironmentVariable("AZURE_FUNCTIONS_ENVIRONMENT")
//                  ?? Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT")
//                  ?? "Production"; // default if not set in Azure

//if (string.Equals(environment, "Production", StringComparison.OrdinalIgnoreCase))
//{
//    var keyVaultName = builder.Configuration["KeyVaultName"];
//    if (!string.IsNullOrWhiteSpace(keyVaultName))
//    {
//        var uri = new Uri($"https://{keyVaultName}.vault.azure.net/");
//        builder.Configuration.AddAzureKeyVault(uri, new DefaultAzureCredential());
//    }
//}
// Else (Development/Staging) we keep secrets like ServiceBusConnection from local.settings.json

builder.Build().Run();
