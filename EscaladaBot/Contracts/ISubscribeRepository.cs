namespace EscaladaBot.Contracts;

public interface ISubscribeRepository
{
    Task Subscribe(long chatId, string user);
    Task Unsubscribe(long chatId);
    Task<IReadOnlyCollection<long>> GetAll();
}