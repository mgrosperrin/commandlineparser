using System;
using System.Collections.Generic;
using System.Data.Common;
using MGR.CommandLineParser.Extensibility.Command;

namespace MGR.CommandLineParser.Command.OracleProcedure
{
    internal class CommandType : ICommandType
    {
        private readonly IEnumerable<Parameter> _outParameters;
        private readonly DbConnection _dbConnection;

        public CommandType(CommandMetadata commandMetadata, IEnumerable<CommandOptionMetadata> options, IEnumerable<Parameter> outParameters, DbConnection dbConnection)
        {
            Metadata = commandMetadata;
            Options = options;
            _outParameters = outParameters;
            _dbConnection = dbConnection;
        }
        public ICommandMetadata Metadata { get; }

        public IEnumerable<ICommandOptionMetadata> Options { get; }

        public ICommandObjectBuilder CreateCommandObjectBuilder(IServiceProvider serviceProvider)
        {
            return new CommandObjectBuilder(Options, _outParameters, _dbConnection);
        }
    }
}
