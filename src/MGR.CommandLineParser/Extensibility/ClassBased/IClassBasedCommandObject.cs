using MGR.CommandLineParser.Command;

namespace MGR.CommandLineParser.Extensibility.ClassBased;

/// <summary>
/// Represents an interface that allow accessing to the raw <see cref="ICommandHandler{TCommandData}"/> instance.
/// </summary>
/// <typeparam name="TCommandHandler">The type of the <see cref="ICommandHandler{TCommandData}"/>.</typeparam>
/// <typeparam name="TCommandData">The type of the <see cref="CommandData"/>.</typeparam>
public interface IClassBasedCommandObject<TCommandHandler, TCommandData>
    where TCommandHandler : class, ICommandHandler<TCommandData>
where TCommandData : CommandData, new()
{
    /// <summary>
    /// Gets the raw <see cref="ICommandHandler{TCommandData}"/> instance for the <see cref="ICommandObject"/>.
    /// </summary>
    TCommandHandler Command { get; }
    /// <summary>
    /// Gets the data associated with the current parsing session.
    /// </summary>
    TCommandData CommandData { get; }
}