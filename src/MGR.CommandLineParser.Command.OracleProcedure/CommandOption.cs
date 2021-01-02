using System;
using MGR.CommandLineParser.Extensibility.Command;

namespace MGR.CommandLineParser.Command.OracleProcedure
{
    internal class CommandOption : ICommandOption
    {
        public bool ShouldProvideValue => throw new NotImplementedException();

        public ICommandOptionMetadata Metadata => throw new NotImplementedException();

        public void AssignValue(string optionValue) => throw new NotImplementedException();
    }
}
