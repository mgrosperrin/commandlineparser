using System.Collections.Generic;
using MGR.CommandLineParser.Extensibility.Command;

namespace MGR.CommandLineParser.Command.Lambda
{
    internal class LambdaBasedCommandTypeProvider : ICommandTypeProvider
    {
        private readonly LambdaBasedCommandType _commandType;

        internal LambdaBasedCommandTypeProvider(LambdaBasedCommandType commandType)
        {
            _commandType = commandType;
        }

        public IEnumerable<ICommandType> GetAllCommandTypes()
        {
            yield return _commandType;
        }

        public ICommandType GetCommandType(string commandName)
        {
            return commandName == _commandType.Metadata.Name ? _commandType : null;
        }
    }
}
