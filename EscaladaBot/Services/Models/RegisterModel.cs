namespace EscaladaBot.Services.Models;

public record RegisterModel(long ChatId,
    string User,
    string Login,
    DateTime DateTime);