namespace MGR.CommandLineParser;

/// <summary>
/// Represents the instance of the command.
/// </summary>
public interface ICommandObject
{
    /// <summary>
    /// Executes the command.
    /// </summary>
    /// <param name="cancellationToken">A cancellation token to stop processing the command.</param>
    /// <returns>The result of the command execution.</returns>
    Task<int> ExecuteAsync(CancellationToken cancellationToken);
}
