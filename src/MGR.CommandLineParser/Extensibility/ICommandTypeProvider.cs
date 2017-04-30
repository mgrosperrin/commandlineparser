using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using MGR.CommandLineParser.Command;
using MGR.CommandLineParser.Extensibility.Command;

namespace MGR.CommandLineParser.Extensibility
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
        ///     Retrive the <see cref="CommandType" /> of the command with the specified <paramref name="commandName" />.
        /// </summary>
        /// <param name="commandName">The command name.</param>
        /// <returns>
        ///     The <see cref="CommandType" /> of the command with the specified <paramref name="commandName" /> or null if
        ///     the command's type is not found.
        /// </returns>
        ICommandType GetCommandType(string commandName);
        /// <summary>
        /// Retrive the <see cref="CommandType" /> of the command of the specified <typeparamref name="TCommand"/>.
        /// </summary>
        /// <typeparam name="TCommand">The type of the command.</typeparam>
        /// <returns></returns>
        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        ICommandType GetCommandType<TCommand>() where TCommand : ICommand;
    }
}
