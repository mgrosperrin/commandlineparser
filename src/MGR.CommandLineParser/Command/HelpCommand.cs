using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;
using MGR.CommandLineParser.Extensibility;
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
        ///     Show detailled help for all commands.
        /// </summary>
        public bool All { get; set; }

        /// <summary>
        ///     Executes the command.
        /// </summary>
        /// <returns>Return 0 is everything was right, an negative error code otherwise.</returns>
        protected override Task<int> ExecuteCommandAsync()
        {
            var commandTypeProvider = CurrentDependencyResolverScope.GetRequiredService<ICommandTypeProvider>();
            var helpWriter = CurrentDependencyResolverScope.GetRequiredService<IHelpWriter>();
            var commandType = commandTypeProvider.GetCommandType(Arguments.FirstOrDefault() ?? string.Empty);
            if (commandType == null)
            {
                WriteHelpWhenNoCommandAreSpecified(commandTypeProvider, helpWriter);
            }
            else
            {
                helpWriter.WriteHelpForCommand(ParserOptions, commandType);
            }
            return Task.FromResult(0);
        }

        private void WriteHelpWhenNoCommandAreSpecified(ICommandTypeProvider commandTypeProvider, IHelpWriter helpWriter)
        {
            if (All)
            {
                var commands = commandTypeProvider.GetAllVisibleCommandsTypes();
                helpWriter.WriteHelpForCommand(ParserOptions, commands.ToArray());
            }
            else
            {
                helpWriter.WriteCommandListing(ParserOptions);
            }
        }
    }
}