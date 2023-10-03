namespace EscaladaBot.Contracts;

public interface IAdminRepository
{
    Task<IReadOnlyCollection<long>> GetAdmins();
}