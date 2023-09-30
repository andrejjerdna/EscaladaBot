namespace EscaladaBot.Contracts;

public interface ITraceInfoViewer
{
    Task<bool> ViewInfo(long chatId, string? message);
}