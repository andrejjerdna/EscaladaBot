using EscaladaBot.Contracts;
using EscaladaBot.Services.Models;

namespace EscaladaBot.Persistence;

public sealed class VoiceRepository : IVoiceRepository
{
    private readonly Dictionary<Guid, AddVoiceModel> _models = new();
    
    public Guid AddVoice(AddVoiceModel model)
    {
        var id = Guid.NewGuid();
        _models.Add(id, model);
        return id;
    }

    public AddVoiceModel? GetVoiceModel(Guid id)
    {
        return _models.TryGetValue(id, out var model)
            ? model
            : null;
    }
}