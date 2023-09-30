using EscaladaBot.Services.Models;

namespace EscaladaBot.Contracts;

public interface IRegisterRepository
{
    Guid AddRegistry(RegisterModel model);
    RegisterModel? GetRegisterModel(Guid id);
}