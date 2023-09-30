namespace EscaladaBot.Services.Models;

public record CreateTraceRequest(
    string Author,
    Guid FileId,
    DateTime TimeStamp);