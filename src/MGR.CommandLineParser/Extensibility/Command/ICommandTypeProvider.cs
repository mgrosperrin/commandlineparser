using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using MGR.CommandLineParser.Extensibility.ClassBased;

namespace MGR.CommandLineParser.Extensibility.Command
{
    /// <summary>
    ///     Define a command provider.
    /// </summary>
    /// <remarks>
    /// </remarks>
    public interface ICommandTypeProvider
    {
        /// <summary>
        ///     Returns all commands types.
        /// </summary>
        [SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate")]
        IEnumerable<ICommandType> GetAllCommandTypes();

        /// <summary>
        ///     Retrive the <see cref="ClassBasedCommandType" /> of the command with the specified <paramref name="commandName" />.
        /// </summary>
        /// <param name="commandName">The command name.</param>
        /// <returns>
        ///     The <see cref="ClassBasedCommandType" /> of the command with the specified <paramref name="commandName" /> or null if
        ///     the command's type is not found.
        /// </returns>
        ICommandType GetCommandType(string commandName);
    }
}
