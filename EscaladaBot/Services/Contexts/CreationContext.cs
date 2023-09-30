using EscaladaApi.Contracts;
using EscaladaBot.Contracts;
using EscaladaBot.Services.BotCommands;

namespace EscaladaBot.Services.Contexts;

public sealed class CreationContext : IContext
{
    private readonly ICommandBuilder _commandBuilder;
    private readonly IProblemCreatorStateStore _problemCreatorStateStore;

    public CreationContext(IProblemCreatorStateStore problemCreatorStateStore, ICommandBuilder commandBuilder)
    {
        _problemCreatorStateStore = problemCreatorStateStore;
        _commandBuilder = commandBuilder;
    }

    public async Task<IReadOnlyCollection<IBotCommand>> GetCurrentCommands(long chatId)
    {
        var state = await _problemCreatorStateStore.GetState(chatId);

        return state.State switch
        {
            TraceCreatorState.NotExists
                => new List<IBotCommand>
                {
                    _commandBuilder.GetCommand<AddImagesStartCommand>()
                },

            TraceCreatorState.Creation
                => new List<IBotCommand>
                {
                    _commandBuilder.GetCommand<AddImagesCommand>(),
                    _commandBuilder.GetCommand<CreateCommand>(),
                    _commandBuilder.GetCommand<EndCommand>()
                },
            _ => throw new Exception("Unknown state")
        };
    }
}