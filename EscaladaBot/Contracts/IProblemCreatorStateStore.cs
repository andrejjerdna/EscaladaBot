using EscaladaBot.Services;
using EscaladaBot.Services.Models;

namespace EscaladaBot.Contracts;

public interface IProblemCreatorStateStore
{
    Task<ProblemCreatorState> GetState(long chatId);
    Task SetState(long chatId, TraceCreatorState state);
    Task Remove(long chatId);
    Task AddFileFolderId(long chatId, Guid id);
    Task CommitState(long chatId);
}