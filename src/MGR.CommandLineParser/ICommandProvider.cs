using System.Collections.Generic;
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
        /// Returns all commands instances.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate")]
        IEnumerable<ICommand> GetAllCommands();
        /// <summary>
        /// Retrieve the instance of the <see cref="HelpCommand"/>.
        /// </summary>
        /// <param name="parserOptions">The options of the parser.</param>
        /// <param name="console">The console to print message.</param>
        /// <returns>An <see cref="HelpCommand"/> instance.</returns>
        //[SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate")]
        HelpCommand GetHelpCommand(IParserOptions parserOptions, IConsole console);
        /// <summary>
        /// Retrive the <see cref="ICommand"/> with the specified <paramref name="commandName"/>.
        /// </summary>
        /// <param name="commandName">The command name.</param>
        /// <param name="parserOptions">The options of the parser.</param>
        /// <param name="console">The console to print message.</param>
        /// <returns>The <see cref="ICommand"/> with the specified <paramref name="commandName"/> or null if the command is not found.</returns>
        ICommand GetCommand(string commandName, IParserOptions parserOptions, IConsole console);
    }
}