using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace EscaladaBot.Services.Models;

public record ContextData(long ChatId,
    string? Message,
    string? BotCommand,
    MessageType? MessageType,
    Message? CallbackQueryMessage);