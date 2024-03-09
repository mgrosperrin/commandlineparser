using MGR.CommandLineParser.Extensibility.Command;

namespace MGR.CommandLineParser.Command;
/// <summary>
/// Defines the base class for the command data.
/// </summary>
public class CommandData
{
    /// <summary>
    /// The list of arguments of the command.
    /// </summary>
    public IList<string> Arguments { get; } = [];
    /// <summary>
    /// Gets the <see cref="CommandType" /> of the command.
    /// </summary>
    internal ICommandType? CommandType { get; private set; }

    /// <summary>
    /// Configure the command with the <see cref="ICommandType" /> representing the command.
    /// </summary>
    /// <param name="commandType">The <see cref="CommandType" /> of the command.</param>
    internal void Configure(ICommandType commandType)
    {
        CommandType = commandType;
    }
}
