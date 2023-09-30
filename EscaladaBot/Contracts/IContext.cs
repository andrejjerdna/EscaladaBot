using EscaladaApi.Contracts;

namespace EscaladaBot.Contracts;

public interface ICreationContext
{
    Task<IReadOnlyCollection<IBotCommand>> GetCurrentCommands(long chatId);
    Task CommitState(long chatId);
    Task Remove(long chatId);
}
