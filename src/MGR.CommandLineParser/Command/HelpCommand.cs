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
        /// <summary>
        ///     Name of the help command.
        /// </summary>
        public const string Name = "Help";

        /// <summary>
        ///     Show detailled help for all commands.
        /// </summary>
        public bool All { get; set; }

        /// <summary>
        ///     Writes help for the specified command. If the command is null, lists all available commands.
        /// </summary>
        /// <param name="command">The <see cref="ICommand" />.</param>
        public void WriteHelp(ICommand command)
        {
            if (command == null)
            {
            var commandProvider = ServiceResolver.Current.ResolveService<ICommandProvider>();
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
                WriteHelpForCommand(command);
            }
        }

        /// <summary>
        ///     Executes the command.
        /// </summary>
        /// <returns>Return 0 is everything was right, an negative error code otherwise.</returns>
        protected override int ExecuteCommand()
        {
            var commandProvider = ServiceResolver.Current.ResolveService<ICommandProvider>();
            var command = commandProvider.GetCommand(Arguments.FirstOrDefault(), ParserOptions, Console);
            WriteHelp(command);
            return 0;
        }

        private void WriteHelpForAllCommand(ICommandProvider commandProvider)
        {
            WriteGeneralInformation();
            var commands = commandProvider.GetAllCommands();
            foreach (var command in commands)
            {
                Console.WriteLine(string.Format(CultureInfo.CurrentUICulture, Strings.HelpCommand_CommandTitleFormat, command.ExtractCommandName()));
                WriteHelpForCommand(command);
            }
        }

        private void WriteHelpForCommand(ICommand command)
        {
            if (command == null)
            {
                throw new ArgumentNullException(nameof(command));
            }
            var metadata = command.ExtractMetadataTemplate();
            Console.WriteLine(ParserOptions.Logo);
            Console.WriteLine(Strings.HelpCommand_CommandUsageFormat, ParserOptions.CommandLineName, metadata.Name, metadata.Usage);
            Console.WriteLine(metadata.Description);
            Console.WriteLine();

            if (metadata.Options.Any())
            {
                Console.WriteLine(Strings.HelpCommand_OptionsListTitle);
                var maxOptionWidth = metadata.Options.Max(o => o.Name.Length) + 2;
                var maxAltOptionWidth = metadata.Options.Max(o => (o.ShortName ?? string.Empty).Length);
                foreach (var optionMetadata in metadata.Options)
                {
                    Console.Write(" -{0, -" + (maxOptionWidth + 2) + "}", optionMetadata.Name + GetMultiValueIndicator(optionMetadata));
                    Console.Write(" {0, -" + (maxAltOptionWidth + 4) + "}", FormatShortName(optionMetadata.ShortName));

                    Console.PrintJustified((10 + maxAltOptionWidth + maxOptionWidth), optionMetadata.Description);
                    Console.WriteLine();
                }
            }

            var usageCommand = command as ISampleCommand;
            if (usageCommand != null)
            {
                foreach (var usage in usageCommand.Samples)
                {
                    Console.WriteLine(usage);
                }
            }
        }

        internal static string GetMultiValueIndicator(OptionMetadataTemplate optionMetadata)
        {
            if (optionMetadata.PropertyOption.PropertyType.IsCollectionType())
            {
                return "+";
            }
            if (optionMetadata.PropertyOption.PropertyType.IsDictionaryType())
            {
                return "#";
            }
            return string.Empty;
        }

        private void WriteGeneralHelp(ICommandProvider commandProvider)
        {
            WriteGeneralInformation();

            var metadatas = commandProvider.GetAllCommands().Select(command => command.ExtractCommandMetadataTemplate()).ToList();
            var maxNameLength = metadatas.Max(m => m.Name.Length);
            foreach (var metadata in metadatas)
            {
                Console.Write(" {0, -" + maxNameLength + "}   ", metadata.Name);
                // Starting index of the description
                var descriptionPadding = maxNameLength + 4;
                Console.PrintJustified(descriptionPadding, metadata.Description);
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