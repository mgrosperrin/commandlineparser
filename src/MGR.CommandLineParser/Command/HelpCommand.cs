using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;
using MGR.CommandLineParser.Extensibility;
using MGR.CommandLineParser.Extensibility.Command;
using Microsoft.Extensions.DependencyInjection;

namespace MGR.CommandLineParser.Command;

/// <summary>
///     Defines the default implementation of the <see cref="HelpCommand" />.
/// </summary>
[PublicAPI]
public sealed class HelpCommand : ICommandHandler<HelpCommandData>
{
    /// <summary>
    ///     Name of the help command.
    /// </summary>
    public const string Name = "help";
    private readonly IServiceProvider _serviceProvider;

    /// <summary>
    /// Creates a new instance of <see cref="HelpCommand"/>.
    /// </summary>
    /// <param name="serviceProvider">A <see cref="IServiceProvider"/> used to resolve services.</param>
    public HelpCommand(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }


    /// <summary>
    ///     Executes the command.
    /// </summary>
    /// <returns>Return 0 is everything was right, an negative error code otherwise.</returns>
    public async Task<int> ExecuteAsync(HelpCommandData commandData)
    {
        var commandTypeProviders = _serviceProvider.GetServices<ICommandTypeProvider>().ToList();
        var helpWriter = _serviceProvider.GetRequiredService<IHelpWriter>();
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