namespace EscaladaBot.Contracts;

public interface IRepositoryFactory
{
    Task<IRepositoryFactory> Build();
}