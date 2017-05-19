using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using MGR.CommandLineParser.Command;
using MGR.CommandLineParser.Extensibility.Command;
using MGR.CommandLineParser.Properties;

namespace MGR.CommandLineParser.Extensibility
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
            var commandTypes = _commandTypeProvider.GetAllVisibleCommandsTypes().ToList();
            WriteDescriptionForSomeCommands(commandTypes);
        }

        private void WriteDescriptionForSomeCommands(List<ICommandType> commandTypes)
        {
            if (commandTypes.Any())
            {
                WriteDescriptionForAllCommands(commandTypes);
            }
            else
            {
                _console.WriteLine("No commands found.");
            }
        }

        private void WriteDescriptionForAllCommands(List<ICommandType> commandTypes)
        {
            var maxNameLength = commandTypes.Max(m => m.Metadata.Name.Length);
            foreach (var commandType in commandTypes)
            {
                WriteDescriptionForOneCommand(commandType, maxNameLength);
            }
        }

        [SuppressMessage("Microsoft.Globalization", "CA1305:SpecifyIFormatProvider", MessageId = "System.String.Format(System.String,System.Object)")]
        private void WriteDescriptionForOneCommand(ICommandType commandType, int maxNameLength)
        {
            _console.Write($" {{0, -{maxNameLength}}}   ", commandType.Metadata.Name);
            // Starting index of the description
            _console.WriteLine(commandType.Metadata.Description);
        }

        /// <inheritdoc />
        public void WriteHelpForCommand(IParserOptions parserOptions, params ICommandType[] commandTypes)
        {
            Guard.NotNull(parserOptions, nameof(parserOptions));
            Guard.NotNull(commandTypes, nameof(commandTypes));

            WriteGeneralInformation(parserOptions);

            foreach (var commandType in commandTypes)
            {
                var metadata = commandType.Metadata;
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
                        var optionName = commandOption.DisplayInfo.Name + GetMultiValueIndicator(commandOption);
                        var optionShortName = FormatShortName(commandOption.DisplayInfo.ShortName);
                        _console.Write(" -{0, -" + maxOptionWidth + "}", optionName);
                        _console.Write("{0, -" + (maxAltOptionWidth + 4) + "}", optionShortName);
                        
                        _console.Write(commandOption.DisplayInfo.Description);
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
