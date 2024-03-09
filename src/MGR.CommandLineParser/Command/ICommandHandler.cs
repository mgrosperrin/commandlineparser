namespace MGR.CommandLineParser.Command;

/// <summary>
/// Defines the contract for the command handler.
/// </summary>
/// <typeparam name="TCommandData">The type of data the command handles.</typeparam>
public interface ICommandHandler<in TCommandData>
    where TCommandData : CommandData, new()
{
    /// <summary>
    /// Executes the command.
    /// </summary>
    /// <param name="commandData">The data provided to the command via the parsing.</param>
    /// <param name="cancellationToken">A cancellation token to stop processing the command.</param>
    /// <returns>Return 0 is everything was right, an negative error code otherwise.</returns>
    Task<int> ExecuteAsync(TCommandData commandData, CancellationToken cancellationToken);
}