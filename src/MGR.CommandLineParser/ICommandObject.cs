using System.Threading.Tasks;

namespace MGR.CommandLineParser
{
    /// <summary>
    /// Represents the instance of the command.
    /// </summary>
    public interface ICommandObject
    {
        /// <summary>
        /// Executes the command.
        /// </summary>
        /// <returns>The result of the command execution.</returns>
        Task<int> ExecuteAsync();
    }
}
