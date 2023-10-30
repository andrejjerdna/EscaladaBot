using EscaladaBot.Contracts;
using EscaladaBot.Services.Contexts;
using EscaladaBot.Services.Models;
using Telegram.Bot.Types.Enums;

namespace EscaladaBot.Services;

public sealed class ContextSwitcher : IContextSwitcher
{
    private readonly IProblemCreatorStateStore _problemCreatorStateStore;
    private readonly ICommandBuilder _commandBuilder;

    public ContextSwitcher(IProblemCreatorStateStore problemCreatorStateStore, 
        ICommandBuilder commandBuilder)
    {
        _problemCreatorStateStore = problemCreatorStateStore;
        _commandBuilder = commandBuilder;
    }

    public async Task<IContext> GetContext(ContextData data)
    {
        if (data.Message != null && long.TryParse(data.Message, out var _))
        {
            return _commandBuilder.GetContext<ViewProblemContext>();
        }
        
        if (data.BotCommand == "/start")
        {
            return _commandBuilder.GetContext<WelcomeContext>();
        }

        if (data.BotCommand == "/addnew")
        {
            return _commandBuilder.GetContext<CreationContext>();
        }
        
        if (data.BotCommand == "/want_to_come")
        {
            return _commandBuilder.GetContext<RegisterContext>();
        }
        
        if (data.BotCommand == "/add_voice")
        {
            return _commandBuilder.GetContext<VoiceContext>();
        }
        
        if (data.BotCommand == "/statistics")
        {
            return _commandBuilder.GetContext<ViewStatisticsContext>();
        }

        if (data.CallbackQueryMessage != null)
        {
            
        }

        var currentState = await _problemCreatorStateStore.GetState(data.ChatId);

        if (data.MessageType == MessageType.Photo && currentState.State == TraceCreatorState.Creation)
        {
            return _commandBuilder.GetContext<CreationContext>();
        }

        return _commandBuilder.GetContext<UnknownContext>();
    }
}