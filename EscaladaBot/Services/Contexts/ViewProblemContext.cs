using EscaladaBot.Contracts;
using EscaladaBot.Services.BotCommands;

namespace EscaladaBot.Services.Contexts;

public sealed class ViewProblemContext : IContext
{
    private readonly ICommandBuilder _commandBuilder;

    public ViewProblemContext(ICommandBuilder commandBuilder)
    {
        _commandBuilder = commandBuilder;
    }

    public async Task<IReadOnlyCollection<IBotCommand>> GetCurrentCommands(long chatId)
    {
        return await Task.FromResult(new List<IBotCommand>
        {
            _commandBuilder.GetCommand<ViewProblemCommand>()
        });
    }
}