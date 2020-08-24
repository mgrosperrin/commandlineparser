using System;
using System.Collections.Generic;
using System.Text;
using MGR.CommandLineParser.Extensibility.Command;

namespace MGR.CommandLineParser.Command.OracleProcedure
{
    class CommandObjectBuilder : ICommandObjectBuilder
    {
        public CommandObjectBuilder(IEnumerable<CommandOption> options, )
        {

        }
        public void AddArguments(string argument) => throw new NotImplementedException("Oracle procedures does not define unamed arguments");
        public ICommandOption FindOption(string optionName) => throw new NotImplementedException();
        public ICommandOption FindOptionByShortName(string optionShortName) => throw new NotImplementedException("Oracle procedures does not define 'short name' for the parameters");
        public ICommandObject GenerateCommandObject() => throw new NotImplementedException();
        public CommandValidationResult Validate(IServiceProvider serviceProvider) => throw new NotImplementedException();
    }
}
