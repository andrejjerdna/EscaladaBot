using EscaladaApi.Contracts;
using EscaladaBot.Contracts;
using EscaladaBot.Services.BotCommands;

namespace EscaladaBot.Services;

public class CreationContext : IContext
{
    private readonly IProblemRepository _problemRepository;
    private readonly IProblemCreatorStateStore _problemCreatorStateStore;

    public CreationContext(IProblemRepository problemRepository, IProblemCreatorStateStore problemCreatorStateStore)
    {
        _problemRepository = problemRepository;
        _problemCreatorStateStore = problemCreatorStateStore;
    }

    public async Task<IReadOnlyCollection<IBotCommand>> GetCurrentCommands(long chatId)
    {
            var state = await _problemCreatorStateStore.GetState(chatId);
                
            return state.State switch
            {
                TraceCreatorState.Creation
                    => new List<IBotCommand>
                    {
                        new AddImagesStartCommand()
                    },

                TraceCreatorState.Created
                    => new List<IBotCommand>
                    {
                        new AddImagesCommand(_problemCreatorStateStore),
                        new CreateCommand(_problemRepository, _problemCreatorStateStore),
                        new EndCommand(_problemCreatorStateStore)
                    },
                _ => throw new Exception("Unknown state")
            };
    }

    public async Task CommitState(long chatId)
    {
            var traceCreatorState = await _problemCreatorStateStore.GetState(chatId);

            switch (traceCreatorState.State)
            {
                case TraceCreatorState.Creation:
                    await _problemCreatorStateStore.SetState(chatId, TraceCreatorState.Created);
                    break;
                case TraceCreatorState.Created:
                    await _problemCreatorStateStore.SetState(chatId,TraceCreatorState.Described);
                    break;
                default:
                    await _problemCreatorStateStore.Remove(chatId);
                    break;
            }
    }

    public async Task Remove(long chatId)
    {
        await _problemCreatorStateStore.Remove(chatId);
    }
}