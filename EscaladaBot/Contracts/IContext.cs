using EscaladaApi.Contracts;

namespace EscaladaBot.Contracts;

public interface IContext
{
    Task<IReadOnlyCollection<IBotCommand>> GetCurrentCommands(long chatId);
}
