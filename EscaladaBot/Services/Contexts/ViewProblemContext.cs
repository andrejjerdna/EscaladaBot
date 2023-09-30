using EscaladaApi.Contracts;
using EscaladaBot.Contracts;

namespace EscaladaBot.Services;

public class ViewProblemContext : IContext
{
    public Task<IReadOnlyCollection<IBotCommand>> GetCurrentCommands(long chatId)
    {
        throw new NotImplementedException();
    }

    public Task CommitState(long chatId)
    {
        throw new NotImplementedException();
    }

    public Task Remove(long chatId)
    {
        throw new NotImplementedException();
    }
}