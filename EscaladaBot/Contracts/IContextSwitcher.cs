using EscaladaBot.Services.Models;

namespace EscaladaBot.Contracts;

public interface IContextSwitcher
{
    Task<IContext> GetContext(ContextData data);
}