using EscaladaBot.Contracts;
using EscaladaBot.Services.Extensions;
using EscaladaBot.Services.Models;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace EscaladaBot.Services.BotCommands;

public sealed class CreateCommand : IBotCommand
{
    private readonly ISubscribeRepository _subscribeRepository;
    private readonly IProblemViewer _problemViewer;
    private readonly IProblemRepository _repository;
    private readonly IProblemCreatorStateStore _problemCreatorStateStore;

    public string Name => nameof(CreateCommand);
    
    public CreateCommand(IProblemRepository repository, 
        IProblemCreatorStateStore problemCreatorStateStore, 
        ISubscribeRepository subscribeRepository, 
        IProblemViewer problemViewer)
    {
        _repository = repository;
        _problemCreatorStateStore = problemCreatorStateStore;
        _subscribeRepository = subscribeRepository;
        _problemViewer = problemViewer;
    }

    public async Task<bool> Run(ITelegramBotClient botClient, Update update)
    {
        var chatId = update?.Message?.Chat.Id;
        
        if(!chatId.HasValue)
            return false;

        var state = await _problemCreatorStateStore.GetState(chatId.Value);

        var author = GetAuthor(update);

        var newTraceId = await _repository.AddTrace(
            new CreateTraceRequest(
                author,
                state.FolderId,
                DateTime.UtcNow));

        if (newTraceId < 0)
            return false;
        
        await _problemCreatorStateStore.CommitState(chatId.Value);

        await botClient.SendTextMessageAsync(
            chatId: chatId.Value,
            text: $"Круто, у нас есть новая трасса! \nНомер трассы: {newTraceId}.");

        var allSubscribers = await _subscribeRepository.GetAll();

        await _problemViewer.ViewProblem(botClient, allSubscribers, newTraceId, true);
        
        return true;
    }

    public async Task NotCompleted(ITelegramBotClient botClient, Update update)
    {
        var chatId = update?.Message?.Chat.Id;
        
        if(!chatId.HasValue)
            return;
        
        await _problemCreatorStateStore.Remove(chatId.Value);

        await botClient.SendMessage(chatId.Value, 
            "Сохранение трассы завершилось неудачей. Начни процесс создания трассы сначала.");
    }

    public async Task Error(ITelegramBotClient botClient, Update update)
    {
        var chatId = update?.Message?.Chat.Id;
        
        if(!chatId.HasValue)
            return;
        
        await _problemCreatorStateStore.Remove(chatId.Value);
        
        await botClient.SendErrorMessage(chatId.Value);
    }

    private static string GetAuthor(Update? update)
    {
        if( update?.Message?.From == null)
            return string.Empty;
        
        return update.Message.From.FirstName + " " + update.Message.From.LastName;
    }
}