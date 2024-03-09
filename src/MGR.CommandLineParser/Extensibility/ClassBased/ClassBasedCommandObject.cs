using System.Threading.Tasks;
using MGR.CommandLineParser.Command;

namespace MGR.CommandLineParser.Extensibility.ClassBased;

internal class ClassBasedCommandObject: ICommandObject, IClassBasedCommandObject
{
    internal ClassBasedCommandObject(ICommandHandler command)
    {
        Command = command;
    }
    public Task<int> ExecuteAsync() => Command.ExecuteAsync();

    public ICommandHandler Command { get; }
}
