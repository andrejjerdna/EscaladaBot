using EscaladaBot.Contracts;
using EscaladaBot.Services.BotCommands;

namespace EscaladaBot.Services.Contexts;

public sealed class RegisterContext : IContext
{
    private readonly ICommandBuilder _commandBuilder;

    public RegisterContext(ICommandBuilder commandBuilder)
    {
        _commandBuilder = commandBuilder;
    }
    
    public async Task<IReadOnlyCollection<IBotCommand>> GetCurrentCommands(long chatId)
    {
        return await Task.FromResult(new List<IBotCommand>
        {
            _commandBuilder.GetCommand<GetDatesCommand>()
        });
    }
}