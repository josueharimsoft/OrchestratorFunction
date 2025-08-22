using Microsoft.Azure.Functions.Worker;
using Microsoft.DurableTask.Client;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using TransactionOrchestration.Models;

namespace TransactionOrchestration
{
    public class TransactionStarter
    {
        private readonly ILogger _logger;

        public TransactionStarter(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<TransactionStarter>();
        }

        [Function("ServiceBusTransactionStarter")]
        public async Task RunAsync(
            [ServiceBusTrigger("transactionqueue", Connection = "ServiceBusConnection")] string messageBody,
            FunctionContext executionContext,
            [DurableClient] DurableTaskClient client)
        {
            _logger.LogInformation("Service Bus queue message received: {Message}", messageBody);

            TransactionMessage? msg;
            try
            {
                msg = JsonSerializer.Deserialize<TransactionMessage>(messageBody);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to deserialize Service Bus message body.");
                return;
            }

            if (msg is null)
            {
                _logger.LogWarning("Message body is null after deserialization.");
                return;
            }

            string instanceId = await client.ScheduleNewOrchestrationInstanceAsync(
                nameof(TransactionOrchestrator), msg);

            _logger.LogInformation("Started orchestration (Service Bus) with ID = {InstanceId} for Tx={TransactionId}", instanceId, msg.TransactionId);
        }
    }
}
