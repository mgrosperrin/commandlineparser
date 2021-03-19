using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using MGR.CommandLineParser.Extensibility.ClassBased;

namespace MGR.CommandLineParser.Extensibility.Command
{
    /// <summary>
    ///     Define a command provider.
    /// </summary>
    /// <remarks>
    /// This is the starting point to implement another way to define commands.
    /// </remarks>
    public interface ICommandTypeProvider
    {
        /// <summary>
        ///     Returns all commands types.
        /// </summary>
        [SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate")]
        Task<IEnumerable<ICommandType>> GetAllCommandTypes();

        /// <summary>
        ///     Retrieve the <see cref="ClassBasedCommandType" /> of the command with the specified <paramref name="commandName" />.
        /// </summary>
        /// <param name="commandName">The command name.</param>
        /// <returns>
        ///     The <see cref="ClassBasedCommandType" /> of the command with the specified <paramref name="commandName" /> or null if
        ///     the command's type is not found.
        /// </returns>
        Task<ICommandType> GetCommandType(string commandName);
    }
}