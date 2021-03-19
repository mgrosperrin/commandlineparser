using System.Collections.Generic;
using System.Threading.Tasks;
using MGR.CommandLineParser.Extensibility.Command;

namespace MGR.CommandLineParser.Command.Lambda
{
    internal class LambdaBasedCommandTypeProvider : ICommandTypeProvider
    {
        private readonly ICommandType _commandType;
        private readonly IEnumerable<ICommandType> _commandTypeEnumerable;

        internal LambdaBasedCommandTypeProvider(LambdaBasedCommandType commandType)
        {
            _commandType = commandType;
            _commandTypeEnumerable = new[] {commandType};
        }

        public Task<IEnumerable<ICommandType>> GetAllCommandTypes()
        {
            return Task.FromResult(_commandTypeEnumerable);
        }

        public Task<ICommandType> GetCommandType(string commandName)
        {
            return Task.FromResult(commandName == _commandType.Metadata.Name ? _commandType : null);
        }
    }
}
