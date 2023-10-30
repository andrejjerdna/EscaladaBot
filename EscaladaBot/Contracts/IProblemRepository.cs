using EscaladaBot.Services.Models;

namespace EscaladaBot.Contracts;

public interface IProblemRepository
{
    Task<int> AddTrace(CreateTraceRequest request);
    Task<Problem?> GetTrace(int problemId);
    Task<IReadOnlyCollection<Problem>> GetAllProblems();
}    