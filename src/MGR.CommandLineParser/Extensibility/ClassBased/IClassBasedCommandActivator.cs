using MGR.CommandLineParser.Command;

namespace MGR.CommandLineParser.Extensibility.ClassBased;

/// <summary>
///     Defines the contract for the activator of <see cref="ICommandHandler{TCommandData}" />.
/// </summary>
public interface IClassBasedCommandActivator
{
    /// <summary>
    ///     Activates (create an instance) of a <typeparamref name="TCommandHandler"/>
    /// </summary>
    /// <returns>The command.</returns>
    TCommandHandler ActivateCommand<TCommandHandler, TCommandData>()
        where TCommandHandler : class, ICommandHandler<TCommandData>
        where TCommandData : CommandData, new();
}