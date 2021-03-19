using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;
using MGR.CommandLineParser.Extensibility;
using MGR.CommandLineParser.Extensibility.Command;
using Microsoft.Extensions.DependencyInjection;

namespace MGR.CommandLineParser.Command
{
    /// <summary>
    ///     Defines the default implementation of the <see cref="HelpCommand" />.
    /// </summary>
    [PublicAPI]
    public sealed class HelpCommand : CommandBase
    {
        /// <summary>
        ///     Name of the help command.
        /// </summary>
        public const string Name = "help";

        /// <summary>
        /// Creates a new instance of <see cref="HelpCommand"/>.
        /// </summary>
        /// <param name="serviceProvider">A <see cref="IServiceProvider"/> used to resolve services.</param>
        public HelpCommand(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        /// <summary>
        ///     Show detailed help for all commands.
        /// </summary>
        public bool All { get; set; }

        /// <summary>
        ///     Executes the command.
        /// </summary>
        /// <returns>Return 0 is everything was right, an negative error code otherwise.</returns>
        protected override async Task<int> ExecuteCommandAsync()
        {
            var commandTypeProviders = ServiceProvider.GetServices<ICommandTypeProvider>().ToList();
            var helpWriter = ServiceProvider.GetRequiredService<IHelpWriter>();
            var commandType = await commandTypeProviders.GetCommandType(Arguments.FirstOrDefault() ?? string.Empty);
            if (commandType == null)
            {
                await WriteHelpWhenNoCommandAreSpecified(commandTypeProviders, helpWriter);
            }
            else
            {
                helpWriter.WriteHelpForCommand(commandType);
            }

            return 0;
        }

        private async Task WriteHelpWhenNoCommandAreSpecified(IEnumerable<ICommandTypeProvider> commandTypeProviders, IHelpWriter helpWriter)
        {
            if (All)
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
}