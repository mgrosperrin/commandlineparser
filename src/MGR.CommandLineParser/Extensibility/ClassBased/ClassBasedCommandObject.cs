using MGR.CommandLineParser.Command;

namespace MGR.CommandLineParser.Extensibility.ClassBased;

internal class ClassBasedCommandObject<TCommandHandler, TCommandData> : ICommandObject, IClassBasedCommandObject<TCommandHandler, TCommandData>
    where TCommandHandler : class, ICommandHandler<TCommandData>
    where TCommandData : CommandData, new()
{
    internal ClassBasedCommandObject(TCommandHandler commandHandler, TCommandData commandData)
    {
        Command = commandHandler;
        CommandData = commandData;
    }
    public Task<int> ExecuteAsync(CancellationToken cancellationToken) => Command.ExecuteAsync(CommandData, cancellationToken);

    public TCommandHandler Command { get; }
    public TCommandData CommandData { get; }
}
