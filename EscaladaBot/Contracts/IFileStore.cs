namespace EscaladaBot.Contracts;

public interface IFileStore
{
    Task<Guid> SaveFile(Stream stream, Guid folderId, string filePath);
    Task<IReadOnlyCollection<Stream>> GetFiles(Guid folderId);
}