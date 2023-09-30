namespace EscaladaBot.Services.Models;

public class Problem
{
    public int Id { get; private set; }
    public Guid FileId { get; private set; }
    public string Author { get; private set; }
    public DateTime Timestamp { get; private set; }
}