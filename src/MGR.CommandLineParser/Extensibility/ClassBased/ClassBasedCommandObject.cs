using System.Threading.Tasks;
using MGR.CommandLineParser.Command;

namespace MGR.CommandLineParser.Extensibility.ClassBased
{
    internal class ClassBasedCommandObject: ICommandObject, IClassBasedCommandObject
    {
        public ClassBasedCommandObject(ICommand command)
        {
            Command = command;
        }
        public Task<int> ExecuteAsync() => Command.ExecuteAsync();

        public ICommand Command { get; }
    }
}
