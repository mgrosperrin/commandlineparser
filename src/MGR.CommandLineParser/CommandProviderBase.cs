using System;
using System.Collections.Generic;
using System.Linq;
using MGR.CommandLineParser.Command;

namespace MGR.CommandLineParser
{
    /// <summary>
    /// Base implementation for the logic of <see cref="ICommandProvider"/>. The logic is based on operations on the list of <see cref="ICommand"/>.
    /// </summary>
    public abstract class CommandProviderBase : ICommandProvider
    {

        private readonly Lazy<List<ICommand>> _commands;
        /// <summary>
        /// Default constructor.
        /// </summary>
        protected CommandProviderBase()
        {
            _commands = new Lazy<List<ICommand>>(BuildCommands);
        }

        /// <inheritdoc />
        public IEnumerable<ICommand> GetAllCommands() => _commands.Value.OrderBy(command => command.ExtractCommandName()).AsEnumerable();

        /// <inheritdoc />
        public HelpCommand GetHelpCommand(IParserOptions parserOptions, IConsole console) => GetCommand(HelpCommand.Name, parserOptions, console) as HelpCommand;

        /// <inheritdoc />
        public ICommand GetCommand(string commandName, IParserOptions parserOptions, IConsole console)
        {
            var command = _commands.Value.FirstOrDefault(c => c.ExtractCommandName().Equals(commandName, StringComparison.OrdinalIgnoreCase));
            var commandBase = command as CommandBase;
            if (commandBase != null)
            {
                commandBase.Configure(parserOptions, console);
            }
            return command;
        }

        /// <summary>
        /// Build the list of the <see cref="ICommand"/>. This method is called lazily and only once when needed.
        /// </summary>
        /// <returns></returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists")]
        protected abstract List<ICommand> BuildCommands();
    }
}