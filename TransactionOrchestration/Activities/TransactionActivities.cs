using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Extensions.DurableTask;
using Microsoft.Extensions.Logging;
using TransactionOrchestration.Models;

namespace TransactionOrchestration;

public static class TransactionActivities
{
    [Function(nameof(ProcessProductAActivity))]
    public static string ProcessProductAActivity([ActivityTrigger] TransactionMessage msg, FunctionContext ctx)
    {
        var logger = ctx.GetLogger(nameof(ProcessProductAActivity));
        logger.LogInformation("Processing Product A for Tx={TransactionId}", msg.TransactionId);
        // TODO: Implement Product A processing logic
        return $"ProductA processed for Tx={msg.TransactionId}";
    }

    [Function(nameof(ProcessProductBActivity))]
    public static string ProcessProductBActivity([ActivityTrigger] TransactionMessage msg, FunctionContext ctx)
    {
        var logger = ctx.GetLogger(nameof(ProcessProductBActivity));
        logger.LogInformation("Processing Product B for Tx={TransactionId}", msg.TransactionId);
        // TODO: Implement Product B processing logic
        return $"ProductB processed for Tx={msg.TransactionId}";
    }

    [Function(nameof(ProcessDefaultActivity))]
    public static string ProcessDefaultActivity([ActivityTrigger] TransactionMessage msg, FunctionContext ctx)
    {
        var logger = ctx.GetLogger(nameof(ProcessDefaultActivity));
        logger.LogInformation("Processing Default for ProductId={ProductId}, Tx={TransactionId}", msg.ProductId, msg.TransactionId);
        // TODO: Implement default processing
        return $"Default processed for ProductId={msg.ProductId} Tx={msg.TransactionId}";
    }
}
