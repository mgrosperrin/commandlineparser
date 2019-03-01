using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MGR.CommandLineParser.Extensibility.Command;

namespace MGR.CommandLineParser.Command.Lambda
{
    internal class LambdaBasedCommandType : ICommandType
    {
        private readonly IEnumerable<LambdaBasedCommandOption> _options;
        private readonly Func<CommandContext, Task<int>> _executeCommand;

        public LambdaBasedCommandType(LambdaBasedCommandMetadata commandMetadata, IEnumerable<LambdaBasedCommandOption> options, Func<CommandContext, Task<int>> executeCommand)
        {
            _options = options;
            _executeCommand = executeCommand;
            Metadata = commandMetadata;
        }
        public ICommandMetadata Metadata { get; }
        public IEnumerable<ICommandOptionMetadata> Options => _options.Select(option => option.Metadata);
        public ICommandObjectBuilder CreateCommandObjectBuilder(IServiceProvider serviceProvider, IParserOptions parserOptions) => new LambdaBasedCommandObjectBuilder(Metadata, _options, serviceProvider, _executeCommand);
    }
}
