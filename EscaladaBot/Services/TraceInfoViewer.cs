using EscaladaApi.Contracts;
using EscaladaBot.Contracts;

namespace EscaladaBot.Services;

public class TraceInfoViewer : ITraceInfoViewer
{
    public Task<bool> ViewInfo(long chatId, string? message)
    {
        return Task.FromResult(false);
    }
}