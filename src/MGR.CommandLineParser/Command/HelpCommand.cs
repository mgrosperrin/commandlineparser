using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using MGR.CommandLineParser.Properties;

namespace MGR.CommandLineParser.Command
{
    /// <summary>
    /// Defines the default implementation of the <see cref="IHelpCommand"/>.
    /// </summary>
    public sealed class HelpCommand : CommandBase, IHelpCommand
    {
        /// <summary>
        /// Name of the help command.
        /// </summary>
        public const string Name = "Help";
        /// <summary>
        /// Gets or sets the <see cref="IHelpCommand"/> used by the parser.
        /// </summary>
        public static IHelpCommand Current { get; set; }

        private IParserOptions _options;
        /// <summary>
        /// Show detailled help for all commands.
        /// </summary>
        public bool All { get; set; }
        /// <summary>
        /// Executes the command.
        /// </summary>
        /// <returns>Return 0 is everything was right, an negative error code otherwise.</returns>
        protected override int ExecuteCommand()
        {
            var command = _options.CommandProvider.GetCommand(Arguments.FirstOrDefault());
            WriteHelp(command);
            return 0;
        }
        /// <summary>
        /// Writes help for the specified command. If the command is null, lists all available commands.
        /// </summary>
        /// <param name="command">The <see cref="ICommand"/>.</param>
        public void WriteHelp(ICommand command)
        {
            if (command == null)
            {
                if (All)
                {
                    WriteHelpForAllCommand();
                }
                else
                {
                    WriteGeneralHelp();
                }
            }
            else
            {
                WriteHelpForCommand(command);
            }
        }
        /// <summary>
        /// Defines the parser options.
        /// </summary>
        /// <param name="options"><see cref="IParserOptions"/>The options.</param>
        public void DefineOptions(IParserOptions options)
        {
            if (options == null)
            {
                throw new ArgumentNullException("options");
            }
            _options = options;
        }

        private void WriteHelpForAllCommand()
        {
            WriteGeneralInformation();
            var commands = _options.CommandProvider.AllCommands;
            foreach (var command in commands)
            {
                _options.Console.WriteLine(string.Format(CultureInfo.CurrentUICulture, Strings.HelpCommand_CommandTitleFormat, command.ExtractCommandName()));
                WriteHelpForCommand(command);
            }
        }

        private void WriteHelpForCommand(ICommand command)
        {
            if (command == null)
            {
                throw new ArgumentNullException("command");
            }
            var metadata = command.ExtractMetadataTemplate(_options);
            _options.Console.WriteLine(_options.Logo);
            _options.Console.WriteLine(Strings.HelpCommand_CommandUsageFormat, _options.CommandLineName, metadata.Name, metadata.Usage);
            _options.Console.WriteLine(metadata.Description);
            _options.Console.WriteLine();

            if (metadata.Options.Any())
            {
                _options.Console.WriteLine(Strings.HelpCommand_OptionsListTitle);
                int maxOptionWidth = metadata.Options.Max(o => o.Name.Length) + 2;
                int maxAltOptionWidth = metadata.Options.Max(o => (o.ShortName ?? String.Empty).Length);
                foreach (var optionMetadata in metadata.Options)
                {
                    _options.Console.Write(" -{0, -" + (maxOptionWidth + 2) + "}", optionMetadata.Name + GetMultiValueIndicator(optionMetadata));
                    _options.Console.Write(" {0, -" + (maxAltOptionWidth + 4) + "}", FormatShortName(optionMetadata.ShortName));

                    _options.Console.PrintJustified((10 + maxAltOptionWidth + maxOptionWidth), optionMetadata.Description);
                    _options.Console.WriteLine();
                }
            }

            var usageCommand = command as ISampleCommand;
            if (usageCommand != null)
            {
                foreach (var usage in usageCommand.Samples)
                {
                    _options.Console.WriteLine(usage);
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

        private void WriteGeneralHelp()
        {
            WriteGeneralInformation();

            List<CommandMetadataTemplate> metadatas = _options.CommandProvider.AllCommands.Select(command => command.ExtractCommandMetadataTemplate()).ToList();
            int maxNameLength = metadatas.Max(m => m.Name.Length);
            foreach (var metadata in metadatas)
            {
                _options.Console.Write(" {0, -" + maxNameLength + "}   ", metadata.Name);
                // Starting index of the description
                int descriptionPadding = maxNameLength + 4;
                _options.Console.PrintJustified(descriptionPadding, metadata.Description);
                _options.Console.WriteLine();
            }
        }

        private void WriteGeneralInformation()
        {
            _options.Console.WriteLine(_options.Logo);
            _options.Console.WriteLine(Strings.HelpCommand_GlobalUsageFormat, string.Format(CultureInfo.CurrentUICulture, Strings.HelpCommand_GlobalCommandLineCommandFormat, _options.CommandLineName).Trim());
            _options.Console.WriteLine(Strings.HelpCommand_GlobalHelpCommandUsageFormat, string.Format(CultureInfo.CurrentUICulture, "{0} {1}", _options.CommandLineName, Name).Trim());
            _options.Console.WriteLine();
            _options.Console.WriteLine(Strings.HelpCommand_GlobalHelp_AvailableCommands);
            _options.Console.WriteLine();
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