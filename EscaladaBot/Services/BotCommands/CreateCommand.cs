using EscaladaApi.Contracts;
using EscaladaBot.Contracts;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace EscaladaBot.Services;

public sealed class CreateCommand : IBotCommand
{
    private readonly ITraceRepository _repository;

    public CreateCommand(ITraceRepository repository)
    {
        _repository = repository;
    }

    public Task Start(ITelegramBotClient botClient, Update update)
    {
        return Task.CompletedTask;
    }

    public async Task<bool> Create(ITelegramBotClient botClient, Update update)
    {
        var message = update.Message?.Text;

        if(string.IsNullOrWhiteSpace(message))
            return false;

        var command = update.Message?.Entities?.FirstOrDefault();
        
        if(command?.Type != MessageEntityType.BotCommand)
            return false;
        
        if(update.Message?.EntityValues?.FirstOrDefault() != "/addnew")
            return false;

        var newTraceId = await _repository.AddTrace(update.Message?.From?.Username ?? "ХЗ");

        if (newTraceId < 0)
            return false;
        
        await botClient.SendTextMessageAsync(
            chatId: update.Message.Chat.Id,
            text: $"Круто, у нас есть новая трасса! \nНомер трассы: {newTraceId}.");
        
        return true;
    }

    public async Task Error(ITelegramBotClient botClient, Update update)
    {
        await botClient.SendTextMessageAsync(
            chatId: update.Message.Chat.Id,
            text: $"К сожалению создать трассу не удалось((( Если не получитя еще раз, напиши: @andrejjerdna");
    }
}