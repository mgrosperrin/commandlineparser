using System;
using System.Collections.Generic;
using System.Linq;
using MGR.CommandLineParser.Command;

namespace MGR.CommandLineParser
{
    /// <summary>
    /// Service to resolve the commands.
    /// </summary>
    public sealed class CommandResolver
    {
        private readonly Lazy<Dictionary<string, ICommand>> _commands;

        /// <summary>
        /// Creates a new instance of <see cref="CommandResolver"/>.
        /// </summary>
        /// <param name="commandProvider"></param>
        public CommandResolver(ICommandProvider commandProvider)
        {
            _commands = new Lazy<Dictionary<string, ICommand>>(() =>
            {
                var commands = commandProvider.GetAllCommands();
                var commandsByName = commands.ToDictionary(c => c.ExtractCommandName(), StringComparer.OrdinalIgnoreCase);
                return commandsByName;
            });
        }

        /// <summary>
        /// Returns all commands instances.
        /// </summary>
        /// <param name="parserOptions">The options of the parser.</param>
        /// <param name="console">The console to print message.</param>
        /// <returns>All commands.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate")]
        public IEnumerable<ICommand> GetAllCommands(IParserOptions parserOptions, IConsole console) => _commands.Value.Keys.Select(commandName =>
        {
            var command = GetCommand(commandName, parserOptions, console);
            return command;
        });

        /// <summary>
        /// Retrieve the instance of the <see cref="HelpCommand"/>.
        /// </summary>
        /// <param name="parserOptions">The options of the parser.</param>
        /// <param name="console">The console to print message.</param>
        /// <returns>An <see cref="HelpCommand"/> instance.</returns>
        //[SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate")]
        public HelpCommand GetHelpCommand(IParserOptions parserOptions, IConsole console) => GetCommand(HelpCommand.Name, parserOptions, console) as HelpCommand;

        /// <summary>
        /// Retrive the <see cref="ICommand"/> with the specified <paramref name="commandName"/>.
        /// </summary>
        /// <param name="commandName">The command name.</param>
        /// <param name="parserOptions">The options of the parser.</param>
        /// <param name="console">The console to print message.</param>
        /// <returns>The <see cref="ICommand"/> with the specified <paramref name="commandName"/> or null if the command is not found.</returns>
        public ICommand GetCommand(string commandName, IParserOptions parserOptions, IConsole console)
        {
            if (_commands.Value.ContainsKey(commandName))
            {
                var command = _commands.Value[commandName];

                var commandBase = command as CommandBase;
                commandBase?.Configure(parserOptions, console);
                return command;
            }
            return null;
        }
    }
}
