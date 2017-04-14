using System.Globalization;
using System.Linq;
using JetBrains.Annotations;
using MGR.CommandLineParser.Properties;

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
        public const string Name = nameof(Help);

        /// <summary>
        ///     Show detailled help for all commands.
        /// </summary>
        public bool All { get; set; }

        /// <summary>
        ///     Writes help for the specified command. If the command is null, lists all available commands.
        /// </summary>
        /// <param name="commandType">The <see cref="ICommand" />.</param>
        public void WriteHelp(CommandType commandType)
        {
            if (commandType == null)
            {
                if (All)
                {
                    var commandProvider = CurrentDependencyResolverScope.ResolveDependency<ICommandTypeProvider>();
                    WriteHelpForAllCommand(commandProvider);
                }
                else
                {
                    WriteGeneralHelp();
                }
            }
            else
            {
                WriteHelpForCommand(commandType);
            }
        }

        /// <summary>
        ///     Executes the command.
        /// </summary>
        /// <returns>Return 0 is everything was right, an negative error code otherwise.</returns>
        protected override int ExecuteCommand()
        {
            var commandTypeProvider = CurrentDependencyResolverScope.ResolveDependency<ICommandTypeProvider>();
            var commandType = commandTypeProvider.GetCommandType(Arguments.FirstOrDefault() ?? string.Empty);
            WriteHelp(commandType);
            return 0;
        }

        private void WriteHelpForAllCommand(ICommandTypeProvider commandProvider)
        {
            WriteGeneralInformation();
            var commands = commandProvider.GetAllVisibleCommandsTypes();
            foreach (var command in commands)
            {
                Console.WriteLine(string.Format(CultureInfo.CurrentUICulture, Strings.HelpCommand_CommandTitleFormat, command.Metadata.Name));
                WriteHelpForCommand(command);
            }
        }

        private void WriteHelpForCommand(CommandType commandType)
        {
            Guard.NotNull(commandType, nameof(commandType));

            var helpWriter = CurrentDependencyResolverScope.ResolveDependency<IHelpWriter>();
            helpWriter.WriteHelpForCommand(ParserOptions, commandType);
        }

        private void WriteGeneralHelp()
        {
            var helpWriter = CurrentDependencyResolverScope.ResolveDependency<IHelpWriter>();
            helpWriter.WriteCommandListing(ParserOptions);
        }

        private void WriteGeneralInformation()
        {
            Console.WriteLine(ParserOptions.Logo);
            Console.WriteLine(Strings.HelpCommand_GlobalUsageFormat, string.Format(CultureInfo.CurrentUICulture, Strings.HelpCommand_GlobalCommandLineCommandFormat, ParserOptions.CommandLineName).Trim());
            Console.WriteLine(Strings.HelpCommand_GlobalHelpCommandUsageFormat, string.Format(CultureInfo.CurrentUICulture, "{0} {1}", ParserOptions.CommandLineName, Name).Trim());
            Console.WriteLine();
            Console.WriteLine(Strings.HelpCommand_GlobalHelp_AvailableCommands);
            Console.WriteLine();
        }
    }
}