using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MGR.CommandLineParser.Command.Lambda
{
    internal class LambdaBasedCommandObject: ICommandObject
    {
        private readonly Func<CommandExecutionContext, Task<int>> _executeCommand;
        private readonly IEnumerable<LambdaBasedCommandOption> _commandOptions;
        private readonly List<string> _arguments;
        private readonly IServiceProvider _serviceProvider;

        internal LambdaBasedCommandObject(Func<CommandExecutionContext, Task<int>> executeCommand, IEnumerable<LambdaBasedCommandOption> commandOptions, List<string> arguments, IServiceProvider serviceProvider)
        {
            _executeCommand = executeCommand;
            _commandOptions = commandOptions;
            _arguments = arguments;
            _serviceProvider = serviceProvider;
        }

        public Task<int> ExecuteAsync()
        {
            var commandContext = new CommandExecutionContext(_commandOptions, _arguments, _serviceProvider);
            return _executeCommand(commandContext);
        }
    }
}
