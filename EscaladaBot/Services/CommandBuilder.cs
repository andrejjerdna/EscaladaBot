using EscaladaBot.Contracts;
using Microsoft.Extensions.DependencyInjection;

namespace EscaladaBot.Services;

public sealed class CommandBuilder : ICommandBuilder
{
    private readonly IServiceProvider _serviceProvider;

    public CommandBuilder(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public IBotCommand GetCommand<T>() where T : IBotCommand
    {
        return _serviceProvider.GetService<T>()
            ?? throw new Exception("Command cannot be null");
    }
    
    public IContext GetContext<T>() where T : IContext
    {
        return _serviceProvider.GetService<T>() 
               ?? throw new Exception("Context cannot be null");
    }
}