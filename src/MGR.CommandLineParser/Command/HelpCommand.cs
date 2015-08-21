using System;
using System.Globalization;
using System.Linq;
using MGR.CommandLineParser.Properties;

namespace MGR.CommandLineParser.Command
{
    /// <summary>
    ///     Defines the default implementation of the <see cref="IHelpCommand" />.
    /// </summary>
    public sealed class HelpCommand : CommandBase, IHelpCommand
    {
        /// <summary>
        ///     Name of the help command.
        /// </summary>
        public const string Name = "Help";

        private IParserOptions _options;

        /// <summary>
        ///     Gets or sets the <see cref="IHelpCommand" /> used by the parser.
        /// </summary>
        public static IHelpCommand Current { get; set; }

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
            var console = ServiceResolver.Current.ResolveService<IConsole>();
            if (command == null)
            {
                if (All)
                {
                    WriteHelpForAllCommand(console);
                }
                else
                {
                    WriteGeneralHelp(console);
                }
            }
            else
            {
                WriteHelpForCommand(console, command);
            }
        }

        /// <summary>
        ///     Defines the parser options.
        /// </summary>
        /// <param name="options"><see cref="IParserOptions" />The options.</param>
        public void DefineOptions(IParserOptions options)
        {
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }
            _options = options;
        }

        /// <summary>
        ///     Executes the command.
        /// </summary>
        /// <returns>Return 0 is everything was right, an negative error code otherwise.</returns>
        protected override int ExecuteCommand()
        {
            var command = _options.CommandProvider.GetCommand(Arguments.FirstOrDefault());
            WriteHelp(command);
            return 0;
        }

        private void WriteHelpForAllCommand(IConsole console)
        {
            WriteGeneralInformation(console);
            var commands = _options.CommandProvider.AllCommands;
            foreach (var command in commands)
            {
                console.WriteLine(string.Format(CultureInfo.CurrentUICulture, Strings.HelpCommand_CommandTitleFormat, command.ExtractCommandName()));
                WriteHelpForCommand(console, command);
            }
        }

        private void WriteHelpForCommand(IConsole console, ICommand command)
        {
            if (command == null)
            {
                throw new ArgumentNullException(nameof(command));
            }
            var metadata = command.ExtractMetadataTemplate(_options);
            console.WriteLine(_options.Logo);
            console.WriteLine(Strings.HelpCommand_CommandUsageFormat, _options.CommandLineName, metadata.Name, metadata.Usage);
            console.WriteLine(metadata.Description);
            console.WriteLine();

            if (metadata.Options.Any())
            {
                console.WriteLine(Strings.HelpCommand_OptionsListTitle);
                var maxOptionWidth = metadata.Options.Max(o => o.Name.Length) + 2;
                var maxAltOptionWidth = metadata.Options.Max(o => (o.ShortName ?? string.Empty).Length);
                foreach (var optionMetadata in metadata.Options)
                {
                    console.Write(" -{0, -" + (maxOptionWidth + 2) + "}", optionMetadata.Name + GetMultiValueIndicator(optionMetadata));
                    console.Write(" {0, -" + (maxAltOptionWidth + 4) + "}", FormatShortName(optionMetadata.ShortName));

                    console.PrintJustified((10 + maxAltOptionWidth + maxOptionWidth), optionMetadata.Description);
                    console.WriteLine();
                }
            }

            var usageCommand = command as ISampleCommand;
            if (usageCommand != null)
            {
                foreach (var usage in usageCommand.Samples)
                {
                    console.WriteLine(usage);
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

        private void WriteGeneralHelp(IConsole console)
        {
            WriteGeneralInformation(console);

            var metadatas = _options.CommandProvider.AllCommands.Select(command => command.ExtractCommandMetadataTemplate()).ToList();
            var maxNameLength = metadatas.Max(m => m.Name.Length);
            foreach (var metadata in metadatas)
            {
                console.Write(" {0, -" + maxNameLength + "}   ", metadata.Name);
                // Starting index of the description
                var descriptionPadding = maxNameLength + 4;
                console.PrintJustified(descriptionPadding, metadata.Description);
                console.WriteLine();
            }
        }

        private void WriteGeneralInformation(IConsole console)
        {
            console.WriteLine(_options.Logo);
            console.WriteLine(Strings.HelpCommand_GlobalUsageFormat, string.Format(CultureInfo.CurrentUICulture, Strings.HelpCommand_GlobalCommandLineCommandFormat, _options.CommandLineName).Trim());
            console.WriteLine(Strings.HelpCommand_GlobalHelpCommandUsageFormat, string.Format(CultureInfo.CurrentUICulture, "{0} {1}", _options.CommandLineName, Name).Trim());
            console.WriteLine();
            console.WriteLine(Strings.HelpCommand_GlobalHelp_AvailableCommands);
            console.WriteLine();
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