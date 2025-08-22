using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Extensions.DurableTask;
using Microsoft.DurableTask;
using Microsoft.DurableTask.Client;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using TransactionOrchestration.Models;

namespace TransactionOrchestration;

public class TransactionOrchestrator
{
    [Function(nameof(TransactionOrchestrator))]
    public async Task<string> Run(
        [OrchestrationTrigger] TaskOrchestrationContext context)
    {
        var input = context.GetInput<TransactionMessage>();
        var logger = context.CreateReplaySafeLogger(nameof(TransactionOrchestrator));

        if (input is null)
        {
            logger.LogWarning("No input provided to orchestrator.");
            return "No input";
        }

        logger.LogInformation("Starting orchestration for TransactionId={TransactionId}, ProductId={ProductId}", input.TransactionId, input.ProductId);

        string result = input.ProductId switch
        {
            1 => await context.CallActivityAsync<string>(nameof(TransactionActivities.ProcessProductAActivity), input),
            2 => await context.CallActivityAsync<string>(nameof(TransactionActivities.ProcessProductBActivity), input),
            _ => await context.CallActivityAsync<string>(nameof(TransactionActivities.ProcessDefaultActivity), input)
        };

        logger.LogInformation("Completed orchestration for TransactionId={TransactionId} with result: {Result}", input.TransactionId, result);
        return result;
    }
}


