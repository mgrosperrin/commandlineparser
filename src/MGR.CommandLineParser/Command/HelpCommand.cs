using MGR.CommandLineParser.Extensibility;
using MGR.CommandLineParser.Extensibility.Command;
using Microsoft.Extensions.DependencyInjection;

namespace MGR.CommandLineParser.Command;

/// <summary>
/// Defines the default implementation of the <see cref="HelpCommand" />.
/// </summary>
public sealed class HelpCommand : CommandBase<HelpCommandData>
{
    /// <summary>
    /// Name of the help command.
    /// </summary>
    public const string Name = "help";

    /// <summary>
    /// Creates a new instance of <see cref="HelpCommand"/>.
    /// </summary>
    /// <param name="serviceProvider">A <see cref="IServiceProvider"/> used to resolve services.</param>
    public HelpCommand(IServiceProvider serviceProvider)
        : base(serviceProvider)
    { }


    /// <inheritdoc />
    protected override async Task<int> ExecuteCommandAsync(HelpCommandData commandData, CancellationToken cancellationToken)
    {
        var commandTypeProviders = ServiceProvider.GetServices<ICommandTypeProvider>().ToList();
        var helpWriter = ServiceProvider.GetRequiredService<IHelpWriter>();
        var commandType = await commandTypeProviders.GetCommandType(commandData.Arguments.FirstOrDefault() ?? string.Empty);
        if (commandType == null)
        {
            await WriteHelpWhenNoCommandAreSpecified(commandTypeProviders, helpWriter, commandData);
        }
        else
        {
            helpWriter.WriteHelpForCommand(commandType);
        }

        return 0;
    }

    private async Task WriteHelpWhenNoCommandAreSpecified(IEnumerable<ICommandTypeProvider> commandTypeProviders, IHelpWriter helpWriter, HelpCommandData commandData)
    {
        if (commandData.All)
        {
            var commands = await commandTypeProviders.GetAllVisibleCommandsTypes();
            helpWriter.WriteHelpForCommand(commands.ToArray());
        }
        else
        {
            await helpWriter.WriteCommandListing();
        }
    }
}