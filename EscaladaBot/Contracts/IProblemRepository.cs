using EscaladaBot.Services.Models;

namespace EscaladaBot.Contracts;

public interface ITraceRepository
{
    Task<int> AddTrace(CreateTraceRequest request);
}