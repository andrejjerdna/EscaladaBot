namespace EscaladaBot.Contracts;

public interface IFileRepository
{
    Task<Guid> AddFile(string filePath);
    Task<string> GetFilePath(Guid id);
}