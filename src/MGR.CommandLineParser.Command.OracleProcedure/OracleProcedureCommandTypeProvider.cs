
using System.Collections.Generic;
using MGR.CommandLineParser.Extensibility.Command;

namespace MGR.CommandLineParser.Command.OracleProcedure
{
    internal class OracleProcedureCommandTypeProvider : ICommandTypeProvider
    {
        public IEnumerable<ICommandType> GetAllCommandTypes() => throw new System.NotImplementedException();
        public ICommandType GetCommandType(string commandName) => throw new System.NotImplementedException();
    }
}
