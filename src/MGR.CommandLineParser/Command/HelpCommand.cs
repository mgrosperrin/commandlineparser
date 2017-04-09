using System;
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
        internal const string CollectionIndicator = "+";
        internal const string DictionaryIndicator = "#";
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
                var commandProvider = CurrentDependencyResolverScope.ResolveDependency<ICommandTypeProvider>();
                if (All)
                {
                    WriteHelpForAllCommand(commandProvider);
                }
                else
                {
                    WriteGeneralHelp(commandProvider);
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
            var commands = commandProvider.GetAllCommandTypes();
            foreach (var command in commands)
            {
                Console.WriteLine(string.Format(CultureInfo.CurrentUICulture, Strings.HelpCommand_CommandTitleFormat, command.Metadata.Name));
                WriteHelpForCommand(command);
            }
        }

        private void WriteHelpForCommand(CommandType commandType)
        {
            Guard.NotNull(commandType, nameof(commandType));

            var metadata = commandType.Metadata;
            Console.WriteLine(ParserOptions.Logo);
            Console.WriteLine(Strings.HelpCommand_CommandUsageFormat, ParserOptions.CommandLineName, metadata.Name, metadata.Usage);
            Console.WriteLine(metadata.Description);
            Console.WriteLine();

            if (commandType.Options.Any())
            {
                Console.WriteLine(Strings.HelpCommand_OptionsListTitle);
                var maxOptionWidth = commandType.Options.Max(o => o.DisplayInfo.Name.Length) + 2;
                var maxAltOptionWidth = commandType.Options.Max(o => (o.DisplayInfo.ShortName ?? string.Empty).Length);
                foreach (var commandOption in commandType.Options)
                {
                    Console.Write(" -{0, -" + (maxOptionWidth + 2) + "}", commandOption.DisplayInfo.Name + GetMultiValueIndicator(commandOption));
                    Console.Write(" {0, -" + (maxAltOptionWidth + 4) + "}", FormatShortName(commandOption.DisplayInfo.ShortName));

                    Console.PrintJustified((10 + maxAltOptionWidth + maxOptionWidth), commandOption.DisplayInfo.Description);
                    Console.WriteLine();
                }
            }

            var sampleCommandAttribute = commandType.Type.GetAttribute<CommandAttribute>();
            if (sampleCommandAttribute != null)
            {
                foreach (var usage in sampleCommandAttribute.Samples)
                {
                    Console.WriteLine(usage);
                }
            }
        }

        internal static string GetMultiValueIndicator(CommandOption commandOption)
        {
            if (commandOption.PropertyOption.PropertyType.IsCollectionType())
            {
                return CollectionIndicator;
            }
            if (commandOption.PropertyOption.PropertyType.IsDictionaryType())
            {
                return DictionaryIndicator;
            }
            return string.Empty;
        }

        private void WriteGeneralHelp(ICommandTypeProvider commandProvider)
        {
            WriteGeneralInformation();

            var commandTypes = commandProvider.GetAllCommandTypes().ToList();
            var maxNameLength = commandTypes.Max(m => m.Metadata.Name.Length);
            foreach (var commandType in commandTypes)
            {
                Console.Write(" {0, -" + maxNameLength + "}   ", commandType.Metadata.Name);
                // Starting index of the description
                var descriptionPadding = maxNameLength + 4;
                Console.PrintJustified(descriptionPadding, commandType.Metadata.Description);
                Console.WriteLine();
            }
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

        private static string FormatShortName(string shortName)
        {
            if (string.IsNullOrEmpty(shortName))
            {
                return string.Empty;
            }
            return string.Format(CultureInfo.CurrentUICulture, "({0})", shortName);
        }
    }
}