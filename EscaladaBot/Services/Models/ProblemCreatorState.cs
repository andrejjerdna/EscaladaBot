namespace EscaladaBot.Services.Models;

public sealed class ProblemCreatorState
{
    public ProblemCreatorState(long chatId)
    {
        ChatId = chatId;
    }
    public long ChatId { get; }
    public string Description { get; private set; }
    public Guid FolderId { get; private set; }
    public TraceCreatorState State { get; private set; }

    public bool IsEmpty => FolderId.Equals(default);

    public ProblemCreatorState SetDescription(string description)
    {
        Description = description;
        return this;
    }

    public ProblemCreatorState SetState(TraceCreatorState state)
    {
        State = state;
        return this;
    }
    
    public ProblemCreatorState SetFolderId(Guid folderId)
    {
        FolderId = folderId;
        return this;
    }
}
