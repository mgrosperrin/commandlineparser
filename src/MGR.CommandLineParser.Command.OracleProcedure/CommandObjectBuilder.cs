using System;
using System.Collections.Generic;
using System.Data.Common;
using MGR.CommandLineParser.Extensibility.Command;

namespace MGR.CommandLineParser.Command.OracleProcedure
{
    internal class CommandObjectBuilder : ICommandObjectBuilder
    {
        private IEnumerable<ICommandOptionMetadata> _options;
        private IEnumerable<Parameter> _outParameters;
        private DbConnection _dbConnection;

        public CommandObjectBuilder(IEnumerable<ICommandOptionMetadata> options, IEnumerable<Parameter> outParameters, DbConnection dbConnection)
        {
            _options = options;
            _outParameters = outParameters;
            _dbConnection = dbConnection;
        }

        public void AddArguments(string argument) => throw new NotImplementedException("Oracle procedures does not define unamed arguments");
        public ICommandOption FindOption(string optionName)
        {
            throw new NotImplementedException();
        }

        public ICommandOption FindOptionByShortName(string optionShortName)
        {
            throw new NotImplementedException($"Oracle procedures does not define 'short name' for the parameters ('{ optionShortName }')");
        }

        public ICommandObject GenerateCommandObject()
        {
            throw new NotImplementedException();
        }

        public CommandValidationResult Validate(IServiceProvider serviceProvider)
        {
            throw new NotImplementedException();
        }
    }
}
