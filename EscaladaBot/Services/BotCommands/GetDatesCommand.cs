using EscaladaApi.Contracts;
using EscaladaBot.Contracts;
using EscaladaBot.Services.Extensions;
using EscaladaBot.Services.Models;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace EscaladaBot.Services.BotCommands;

public sealed class GetDatesCommand : IBotCommand
{
    private readonly IRegisterRepository _registerRepository;

    public GetDatesCommand(IRegisterRepository registerRepository)
    {
        _registerRepository = registerRepository;
    }

    public string Name => nameof(GetDatesCommand);
    public async Task<bool> Run(ITelegramBotClient botClient, Update update)
    {
        var chatId = update?.Message?.Chat.Id;
        
        if(!chatId.HasValue)
            return false;

        var now = DateTime.UtcNow;

        var filtredDays = Enumerable.Range(0, 30)
            .Select(r => now.AddDays(r))
            .Where(d => d.DayOfWeek == DayOfWeek.Thursday)
            .Select(d 
                => InlineKeyboardButton.WithCallbackData(
                    text: GetBeautifyDate(d), 
                    callbackData: GetCommandDate(d, 
                        430055377, 
                        update.Message.From.FirstName + " " + update.Message.From.LastName,
                        update.Message.From.Username)))
            .Chunk(2)
            .ToArray();
        
        var inlineKeyboard = new InlineKeyboardMarkup(filtredDays);
        
        await botClient.SendTextMessageAsync(
            chatId: chatId,
            text: "В какой день ты хочешь прийти?",
            replyMarkup: inlineKeyboard,
            cancellationToken: CancellationToken.None);

        return true;
    }

    public Task NotCompleted(ITelegramBotClient botClient, Update update)
    {
        return Task.CompletedTask;
    }

    public async Task Error(ITelegramBotClient botClient, Update update)
    {
        var chatId = update?.Message?.Chat.Id;
        
        if(!chatId.HasValue)
            return;
       
        await botClient.SendErrorMessage(chatId.Value);
    }

    private static string GetBeautifyDate(DateTime dateTime)
    {
        return dateTime.ToString("M");
    }
    
    private string GetCommandDate(DateTime dateTime, long chatId, string user, string login)
    {
        var guid = _registerRepository.AddRegistry(new RegisterModel(chatId, user, login, dateTime));
        return guid.ToString();
    }
}