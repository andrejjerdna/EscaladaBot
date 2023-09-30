using EscaladaBot.Contracts;

namespace EscaladaBot.Services;

public sealed class FileRepository : IFileRepository
{
    public Task<Guid> AddFile(string filePath)
    {
        throw new NotImplementedException();
    }

    public Task<string> GetFilePath(Guid id)
    {
        throw new NotImplementedException();
    }
}