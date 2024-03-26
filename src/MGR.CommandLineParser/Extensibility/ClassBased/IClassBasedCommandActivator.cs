using MGR.CommandLineParser.Command;

namespace MGR.CommandLineParser.Extensibility.ClassBased;

/// <summary>
/// Defines the contract for the activator of <see cref="ICommandHandler{TCommandData}" />.
/// </summary>
public interface IClassBasedCommandActivator
{
    /// <summary>
    /// Activates (create an instance) of a <typeparamref name="TCommandHandler"/>.
    /// </summary>
    /// <typeparam name="TCommandHandler">The type of the command handler.</typeparam>
    /// <typeparam name="TCommandData">The type of the command data.</typeparam>
    /// <returns>The command handler.</returns>
    TCommandHandler ActivateCommand<TCommandHandler, TCommandData>()
        where TCommandHandler : class, ICommandHandler<TCommandData>
        where TCommandData : CommandData, new();
}