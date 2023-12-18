using EscaladaBot.Contracts;
using EscaladaBot.Services.BotCommands;

namespace EscaladaBot.Services.Contexts;

public sealed class UnknownContext : IContext
{
    private readonly ICommandBuilder _commandBuilder;

    public UnknownContext(ICommandBuilder commandBuilder)
    {
        _commandBuilder = commandBuilder;
    }

    public async Task<IReadOnlyCollection<IBotCommand>> GetCurrentCommands(long chatId)
    {
        return await Task.FromResult(new List<IBotCommand>
            {
                _commandBuilder.GetCommand<UnknownCommand>()
            });
    }
}