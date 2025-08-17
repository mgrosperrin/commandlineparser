using MGR.CommandLineParser.Command;
using MGR.CommandLineParser.Extensibility.Command;

namespace MGR.CommandLineParser.Extensibility.ClassBased;

internal sealed class ClassBasedCommandMetadata<TCommandHandler, TCommandData> : ICommandMetadata
    where TCommandHandler : class, ICommandHandler<TCommandData>
    where TCommandData : CommandData, new()
{
    internal ClassBasedCommandMetadata()
    {
        var commandHandlerType = typeof(TCommandHandler);
        Name = commandHandlerType.GetFullCommandName();
        var commandAttribute = commandHandlerType.GetAttribute<CommandAttribute>();
        if (commandAttribute != null)
        {
            Description = commandAttribute.GetLocalizedDescription();
            Usage = commandAttribute.GetLocalizedUsage();
            Samples = commandAttribute.Samples ?? [];
            HideFromHelpListing = commandAttribute.HideFromHelpListing;
        }
    }

    public string Name { get; }

    public string Description { get; } = string.Empty;

    public string Usage { get; } = string.Empty;

    public string[] Samples { get; } = [];

    public bool HideFromHelpListing { get; }
}