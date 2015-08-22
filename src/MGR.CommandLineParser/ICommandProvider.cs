using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using MGR.CommandLineParser.Command;

namespace MGR.CommandLineParser
{
    /// <summary>
    /// Define a command provider.
    /// </summary>
    /// <remarks>
    /// </remarks>
    public interface ICommandProvider
    {
        /// <summary>
        /// Retrieve and recreate new instances of the commands.
        /// </summary>
        /// <remarks>The command provider should create new instance of commands only when the BuildCommands method is called.</remarks>
        void BuildCommands();

        /// <summary>
        /// Returns all commands instances.
        /// </summary>
        IEnumerable<ICommand> GetAllCommands();
        /// <summary>
        /// Retrieve the instance of the <see cref="IHelpCommand"/>.
        /// </summary>
        /// <returns>An <see cref="IHelpCommand"/> instance.</returns>
        [SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate")]
        IHelpCommand GetHelpCommand();
        /// <summary>
        /// Retrive the <see cref="ICommand"/> with the specified <paramref name="commandName"/>.
        /// </summary>
        /// <param name="commandName">The command name.</param>
        /// <returns>The <see cref="ICommand"/> with the specified <paramref name="commandName"/> or null if the command is not found.</returns>
        ICommand GetCommand(string commandName);
    }
}