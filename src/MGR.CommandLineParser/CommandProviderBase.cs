using System;
using System.Collections.Generic;
using System.Linq;
using MGR.CommandLineParser.Command;

namespace MGR.CommandLineParser
{
    public abstract class CommandProviderBase : ICommandProvider
    {
        protected CommandProviderBase()
        {
            _commands = new Lazy<List<ICommand>>(BuildCommands);
        }

        private readonly Lazy<List<ICommand>> _commands;

        public IEnumerable<ICommand> GetAllCommands() => _commands.Value.OrderBy(command => command.ExtractCommandName()).AsEnumerable();

        public HelpCommand GetHelpCommand(IParserOptions parserOptions, IConsole console) => GetCommand(HelpCommand.Name, parserOptions, console) as HelpCommand;

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

        protected abstract List<ICommand> BuildCommands();
    }
}