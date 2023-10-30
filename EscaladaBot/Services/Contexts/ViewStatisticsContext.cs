using EscaladaApi.Contracts;
using EscaladaBot.Contracts;
using EscaladaBot.Services.BotCommands;

namespace EscaladaBot.Services.Contexts;

public sealed class ViewStatisticsContext : IContext
{
    private readonly ICommandBuilder _commandBuilder;

    public ViewStatisticsContext(ICommandBuilder commandBuilder)
    {
        _commandBuilder = commandBuilder;
    }

    public async Task<IReadOnlyCollection<IBotCommand>> GetCurrentCommands(long chatId)
    {
        return await Task.FromResult(new List<IBotCommand>
        {
            _commandBuilder.GetCommand<ViewStatisticsCommand>()
        });
    }
}