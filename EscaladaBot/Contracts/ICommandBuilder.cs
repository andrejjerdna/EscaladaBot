namespace EscaladaBot.Contracts;

public interface ICommandBuilder
{
    IBotCommand GetCommand<T>() where T : IBotCommand;
    IContext GetContext<T>() where T : IContext;
}