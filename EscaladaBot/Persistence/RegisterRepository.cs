using EscaladaBot.Contracts;
using EscaladaBot.Services.Models;

namespace EscaladaBot.Persistence;

public class RegisterRepository : IRegisterRepository
{
    private readonly Dictionary<Guid, RegisterModel> _models = new();
    
    public Guid AddRegistry(RegisterModel model)
    {
        var id = Guid.NewGuid();
        _models.Add(id, model);
        return id;
    }

    public RegisterModel? GetRegisterModel(Guid id)
    {
        return _models.TryGetValue(id, out var model)
            ? model
            : null;
    }
}