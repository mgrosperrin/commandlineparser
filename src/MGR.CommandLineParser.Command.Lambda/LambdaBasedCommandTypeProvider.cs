using System.Collections.Generic;
using MGR.CommandLineParser.Extensibility.Command;

namespace MGR.CommandLineParser.Command.Lambda
{
    internal class LambdaBasedCommandTypeProvider : ICommandTypeProvider
    {
        private readonly LambdaBasedCommandType _commandType;

        public LambdaBasedCommandTypeProvider(LambdaBasedCommandType commandType)
        {
            _commandType = commandType;
        }

        public IEnumerable<ICommandType> GetAllCommandTypes()
        {
            yield return _commandType;
        }

        public ICommandType GetCommandType(string commandName)
        {
            if (commandName == _commandType.Metadata.Name)
            {
                return _commandType;
            }

            return null;
        }
    }
}
