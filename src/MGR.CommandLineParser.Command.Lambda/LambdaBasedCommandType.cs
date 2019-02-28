using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MGR.CommandLineParser.Extensibility.Command;

namespace MGR.CommandLineParser.Command.Lambda
{
    public class LambdaBasedCommandType : ICommandType
    {
        private readonly IEnumerable<ICommandOption> _options;
        private readonly Func<CommandContext, Task<int>> _executeCommand;

        public LambdaBasedCommandType(LambdaBasedCommandMetadata commandMetadata, IEnumerable<ICommandOption> options, Func<CommandContext, Task<int>> executeCommand)
        {
            _options = options;
            _executeCommand = executeCommand;
            Metadata = commandMetadata;
        }
        public ICommandMetadata Metadata { get; }
        public IEnumerable<ICommandOptionMetadata> Options => _options.Select(option => option.Metadata);
        public ICommandObjectBuilder CreateCommandObjectBuilder(IServiceProvider serviceProvider, IParserOptions parserOptions) => throw new NotImplementedException();
    }
}
