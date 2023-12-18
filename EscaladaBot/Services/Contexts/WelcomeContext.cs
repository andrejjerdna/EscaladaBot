using EscaladaBot.Contracts;
using EscaladaBot.Services.BotCommands;

namespace EscaladaBot.Services.Contexts;

public sealed class WelcomeContext : IContext
{
    private readonly ICommandBuilder _commandBuilder;

    public WelcomeContext(ICommandBuilder commandBuilder)
    {
        _commandBuilder = commandBuilder;
    }

    public async Task<IReadOnlyCollection<IBotCommand>> GetCurrentCommands(long chatId)
    {
        return await Task.FromResult(new List<IBotCommand>
        {
            _commandBuilder.GetCommand<WelcomeCommand>()
        });
    }
}