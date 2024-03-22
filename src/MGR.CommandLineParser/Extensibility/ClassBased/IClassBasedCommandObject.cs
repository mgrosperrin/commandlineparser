using MGR.CommandLineParser.Command;

namespace MGR.CommandLineParser.Extensibility.ClassBased;

/// <summary>
/// Represents an interface that allow accessing to the raw <see cref="ICommandHandler{TCommandData}"/> instance.
/// </summary>
public interface IClassBasedCommandObject<TCommandHandler, TCommandData>
    where TCommandHandler : class, ICommandHandler<TCommandData>
where TCommandData : CommandData, new()
{
    /// <summary>
    /// Gets the raw <see cref="ICommandHandler{TCommandData}"/> instance for the <see cref="ICommandObject"/>.
    /// </summary>
    TCommandHandler Command { get; }
    /// <summary>
    /// 
    /// </summary>
    TCommandData CommandData { get; }
}