using EscaladaBot.Contracts;
using EscaladaBot.Services.BotCommands;

namespace EscaladaBot.Services.Contexts;

public sealed class VoiceContext : IContext
{
    private readonly ICommandBuilder _commandBuilder;

    public VoiceContext(ICommandBuilder commandBuilder)
    {
        _commandBuilder = commandBuilder;
    }

    public async Task<IReadOnlyCollection<IBotCommand>> GetCurrentCommands(long chatId)
    {
        return await Task.FromResult(new List<IBotCommand>
        {
            _commandBuilder.GetCommand<AddVoiceCommand>()
        });
    }
}