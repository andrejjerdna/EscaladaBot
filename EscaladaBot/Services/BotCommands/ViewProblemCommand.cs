using EscaladaApi.Contracts;
using EscaladaBot.Contracts;
using EscaladaBot.Services.Extensions;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace EscaladaBot.Services.BotCommands;

public sealed class ViewProblemCommand : IBotCommand
{
    private readonly IProblemViewer _problemViewer;

    public ViewProblemCommand(IProblemViewer problemViewer)
    {
        _problemViewer = problemViewer;
    }

    public string Name => nameof(ViewProblemCommand);
    
    public async Task<bool> Run(ITelegramBotClient botClient, Update update)
    {
        var chatId = update?.Message?.Chat.Id;
        
        if(!chatId.HasValue)
            return false;

        if(!int.TryParse(update?.Message?.Text, out var problemId))
            return false;

        await _problemViewer.ViewProblem(botClient, new []{ chatId.Value }, problemId, false);
        
        return true;
    }

    public async Task NotCompleted(ITelegramBotClient botClient, Update update)
    {
        var chatId = update?.Message?.Chat.Id;
        
        if(!chatId.HasValue)
            return;
        
        await botClient.SendMessage(chatId.Value, 
            "У удалось показать информацию по трассе.");
    }

    public async Task Error(ITelegramBotClient botClient, Update update)
    {
        var chatId = update?.Message?.Chat.Id;
        
        if(!chatId.HasValue)
            return;
       
        await botClient.SendErrorMessage(chatId.Value);
    }
}