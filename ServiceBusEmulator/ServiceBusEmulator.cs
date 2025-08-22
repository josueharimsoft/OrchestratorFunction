using System;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Extensions.DurableTask;
using Microsoft.DurableTask.Client;
using Microsoft.Extensions.Logging;
using TransactionOrchestration.Models;

namespace ServiceBusEmulator;

// This function emulates an Azure Service Bus by using Azurite Storage Queues locally.
// It listens to the storage queue 'transactionqueue' and starts the TransactionOrchestrator.
public class ServiceBusEmulator
{
    private readonly ILogger<ServiceBusEmulator> _logger;

    public ServiceBusEmulator(ILogger<ServiceBusEmulator> logger)
    {
        _logger = logger;
    }

    [Function("ServiceBusEmulator")]    
    public async Task RunAsync(
        [QueueTrigger("transactionqueue")] string messageBody,
        FunctionContext context,
        [DurableClient] DurableTaskClient client)
    {
        _logger.LogInformation("Queue message received: {Message}", messageBody);

        TransactionMessage? msg;
        try
        {
            msg = JsonSerializer.Deserialize<TransactionMessage>(messageBody);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to deserialize queue message body.");
            return;
        }

        if (msg is null)
        {
            _logger.LogWarning("Message body is null after deserialization.");
            return;
        }

        string instanceId = await client.ScheduleNewOrchestrationInstanceAsync(
            nameof(TransactionOrchestration.TransactionOrchestrator), msg);

        _logger.LogInformation("Started orchestration with ID = {InstanceId} for Tx={TransactionId}", instanceId, msg.TransactionId);
    }
}