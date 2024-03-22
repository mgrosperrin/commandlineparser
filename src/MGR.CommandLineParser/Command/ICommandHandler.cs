using System.Threading.Tasks;

namespace MGR.CommandLineParser.Command;

/// <summary>
/// Defines a command.
/// </summary>
public interface ICommandHandler<in TCommandData>
    where TCommandData : CommandData, new()
{
    /// <summary>
    /// Executes the command.
    /// </summary>
    /// <returns>Return 0 is everything was right, an negative error code otherwise.</returns>
    Task<int> ExecuteAsync(TCommandData commandData);
}