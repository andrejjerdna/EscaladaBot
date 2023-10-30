using System.Text;
using EscaladaApi.Contracts;
using EscaladaBot.Contracts;
using EscaladaBot.Services.Extensions;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace EscaladaBot.Services.BotCommands;

public class ViewStatisticsCommand : IBotCommand
{
    private readonly IStatisticsRepository _statisticsRepository;

    public ViewStatisticsCommand(IStatisticsRepository statisticsRepository)
    {
        _statisticsRepository = statisticsRepository;
    }

    public string Name => nameof(ViewStatisticsCommand);
    public async Task<bool> Run(ITelegramBotClient botClient, Update update)
    {
        var chatId = update?.Message?.Chat.Id;
        
        if(!chatId.HasValue)
            return false;

        var problems = await _statisticsRepository.GetPopularProblems(10);
       
        await botClient.SendTextMessageAsync(
            chatId: chatId,
            text: GetMessage(problems),
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

    private string GetMessage(IReadOnlyCollection<(long problemId, int voiceCount)>? problems)
    {
        if (problems == null || !problems.Any())
        {
            return "Нет трасс с голосами";
        }

        var stringBuilder = new StringBuilder();
        
        stringBuilder.AppendLine($"Трасса - Кол-во. голосов");

        foreach (var problem in problems)
        {
            stringBuilder.AppendLine($"{problem.problemId} - {problem.voiceCount}");
        }

        return stringBuilder.ToString();
    }
}