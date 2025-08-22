namespace TransactionOrchestration.Models;

public sealed record TransactionMessage(
    string TransactionId,
    int ProductId,
    int ChanelId,
    int LanguageId,
    string MessageId
);
