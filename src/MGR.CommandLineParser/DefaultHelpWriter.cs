using System;
using System.Globalization;
using System.Linq;
using MGR.CommandLineParser.Command;
using MGR.CommandLineParser.Properties;

namespace MGR.CommandLineParser
{
    /// <summary>
    /// The default help writer. Writes to the <see cref="IConsole"/>.
    /// </summary>
    public sealed class DefaultHelpWriter : IHelpWriter
    {
        internal const string CollectionIndicator = "+";
        internal const string DictionaryIndicator = "#";

        private readonly IConsole _console;
        private readonly ICommandTypeProvider _commandTypeProvider;

        internal DefaultHelpWriter(IConsole console, ICommandTypeProvider commandTypeProvider)
        {
            _console = console;
            _commandTypeProvider = commandTypeProvider;
        }

        /// <inheritdoc />
        public void WriteCommandListing(IParserOptions parserOptions)
        {
            Guard.NotNull(parserOptions, nameof(parserOptions));

            WriteGeneralInformation(parserOptions);

            _console.WriteLine(Strings.DefaultHelpWriter_GlobalHelp_AvailableCommands);
            _console.WriteLine();
            var commandTypes = _commandTypeProvider.GetAllVisibleCommandsTypes().ToList();
            var maxNameLength = commandTypes.Max(m => m.Metadata.Name.Length);
            foreach (var commandType in commandTypes)
            {
                _console.Write(" {0, -" + maxNameLength + "}   ", commandType.Metadata.Name);
                // Starting index of the description
                var descriptionPadding = maxNameLength + 4;
                _console.PrintJustified(descriptionPadding, commandType.Metadata.Description);
                _console.WriteLine();
            }
        }

        /// <inheritdoc />
        public void WriteHelpForCommand(IParserOptions parserOptions, params CommandType[] commandTypes)
        {
            Guard.NotNull(parserOptions, nameof(parserOptions));
            Guard.NotNull(commandTypes, nameof(commandTypes));

            WriteGeneralInformation(parserOptions);

            foreach (var commandType in commandTypes)
            {
                var metadata = commandType.Metadata;
                _console.WriteLine(parserOptions.Logo);
                _console.WriteLine(Strings.DefaultHelpWriter_CommandUsageFormat, parserOptions.CommandLineName, metadata.Name, metadata.Usage);
                _console.WriteLine(metadata.Description);
                _console.WriteLine();

                if (commandType.Options.Any())
                {
                    _console.WriteLine(Strings.DefaultHelpWriter_OptionsListTitle);
                    var maxOptionWidth = commandType.Options.Max(o => o.DisplayInfo.Name.Length) + 2;
                    var maxAltOptionWidth = commandType.Options.Max(o => (o.DisplayInfo.ShortName ?? string.Empty).Length);
                    foreach (var commandOption in commandType.Options)
                    {
                        _console.Write(" -{0, -" + (maxOptionWidth + 2) + "}", commandOption.DisplayInfo.Name + GetMultiValueIndicator(commandOption));
                        _console.Write(" {0, -" + (maxAltOptionWidth + 4) + "}", FormatShortName(commandOption.DisplayInfo.ShortName));

                        _console.PrintJustified((10 + maxAltOptionWidth + maxOptionWidth), commandOption.DisplayInfo.Description);
                        _console.WriteLine();
                    }
                }

                var samples = commandType.Metadata.Samples;
                foreach (var usage in samples)
                {
                    _console.WriteLine(usage);
                }
            }
        }

        private void WriteGeneralInformation(IParserOptions parserOptions)
        {
            Guard.NotNull(parserOptions, nameof(parserOptions));

            _console.WriteLine(parserOptions.Logo);
            _console.WriteLine(Strings.DefaultHelpWriter_GlobalUsageFormat, string.Format(CultureInfo.CurrentUICulture, Strings.DefaultHelpWriter_GlobalCommandLineCommandFormat, parserOptions.CommandLineName).Trim());
            _console.WriteLine(Strings.DefaultHelpWriter_GlobalHelpCommandUsageFormat, string.Format(CultureInfo.CurrentUICulture, "{0} {1}", parserOptions.CommandLineName, HelpCommand.Name).Trim());
            _console.WriteLine();
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
