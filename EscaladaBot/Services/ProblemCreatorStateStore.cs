using EscaladaBot.Contracts;
using EscaladaBot.Services.Models;

namespace EscaladaBot.Services;

public sealed class ProblemCreatorStateStore : IProblemCreatorStateStore
{
    private readonly Dictionary<long, ProblemCreatorState> _problemCreatorStates = new();
    private readonly SemaphoreSlim _semaphore = new(1, 1);

    public async Task<ProblemCreatorState> GetState(long chatId)
    {
        try
        {
            await _semaphore.WaitAsync();

            if (_problemCreatorStates.TryGetValue(chatId, out var problemCreatorState))
            {
                return problemCreatorState;
            }
            
            var state = new ProblemCreatorState(chatId);
            _problemCreatorStates[chatId] = state;

            return state;
        }
        finally
        {
            _semaphore.Release();
        }
    }

    public async Task SetState(long chatId, TraceCreatorState state)
    {
        try
        {
            await _semaphore.WaitAsync();

            if (_problemCreatorStates.TryGetValue(chatId, out var problemCreatorState))
            {
                _problemCreatorStates[chatId] = problemCreatorState.SetState(state);
            }
        }
        finally
        {
            _semaphore.Release();
        }
    }

    public async Task Remove(long chatId)
    {
        try
        {
            await _semaphore.WaitAsync();

            if (_problemCreatorStates.TryGetValue(chatId, out var _))
            {
                _problemCreatorStates.Remove(chatId);
            }
        }
        finally
        {
            _semaphore.Release();
        }
    }
    
    public async Task AddFileFolderId(long chatId, Guid id)
    {
        try
        {
            await _semaphore.WaitAsync();

            if (_problemCreatorStates.TryGetValue(chatId, out var problemCreatorState))
            {
                _problemCreatorStates[chatId] = problemCreatorState.SetFolderId(id);
            }
        }
        finally
        {
            _semaphore.Release();
        }
    }
    
    public async Task CommitState(long chatId)
    {
        var traceCreatorState = await GetState(chatId);

        switch (traceCreatorState.State)
        {
            case TraceCreatorState.NotExists:
                await SetState(chatId, TraceCreatorState.Creation);
                break;
            case TraceCreatorState.Creation:
                await SetState(chatId, TraceCreatorState.Created);
                break;
            case TraceCreatorState.Created:
                await SetState(chatId,TraceCreatorState.Described);
                break;
            default:
                await Remove(chatId);
                break;
        }
    }
}