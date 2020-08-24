using System;
using System.Collections.Generic;
using MGR.CommandLineParser.Extensibility.Command;

namespace MGR.CommandLineParser.Command.OracleProcedure
{
    internal class CommandType : ICommandType
    {
        private readonly IEnumerable<Parameter> _outParameters;

        public CommandType(CommandMetadata commandMetadata, IEnumerable<CommandOptionMetadata> options, IEnumerable<Parameter> outParameters)
        {
            Metadata = commandMetadata;
            Options = options;
            _outParameters = outParameters;
        }
        public ICommandMetadata Metadata { get; }

        public IEnumerable<ICommandOptionMetadata> Options { get; }

        public ICommandObjectBuilder CreateCommandObjectBuilder(IServiceProvider serviceProvider) => throw new NotImplementedException();
    }
}
