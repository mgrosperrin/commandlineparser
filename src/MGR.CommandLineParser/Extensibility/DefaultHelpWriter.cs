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
        internal const string MultiOptionNameSeparator = "|";

        private readonly IConsole _console;
        private readonly IEnumerable<ICommandTypeProvider> _commandTypeProviders;
        private readonly IParserOptionsAccessor _parserOptionsAccessor;

        /// <summary>
        /// Create a new <see cref="DefaultHelpWriter"/>.
        /// </summary>
        /// <param name="console"></param>
        /// <param name="commandTypeProviders"></param>
        /// <param name="parserOptionsAccessor"></param>
        public DefaultHelpWriter(IConsole console, IEnumerable<ICommandTypeProvider> commandTypeProviders, IParserOptionsAccessor parserOptionsAccessor)
        {
            _console = console;
            _commandTypeProviders = commandTypeProviders;
            _parserOptionsAccessor = parserOptionsAccessor;
        }

        /// <inheritdoc />
        public void WriteCommandListing()
        {
            WriteGeneralInformation(_parserOptionsAccessor.Current);

            _console.WriteLine(Strings.DefaultHelpWriter_GlobalHelp_AvailableCommands);
            var commandTypes = _commandTypeProviders.GetAllVisibleCommandsTypes().ToList();
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

                var options = commandType.Options.ToList();
                if (options.Count > 0)
                {
                    _console.WriteLine(Strings.DefaultHelpWriter_OptionsListTitle);
                    var maxOptionWidth = commandType.Options.Max(o => o.DisplayInfo.Name.Length + o.DisplayInfo.AlternateNames.Sum(
                        alternateName => alternateName.Length + 1)) + 2;
                    var maxAltOptionWidth = commandType.Options.Max(o => (o.DisplayInfo.ShortName ?? string.Empty).Length);
                    foreach (var commandOptionMetadata in options)
                    {
                        var alternateNames = string.Join(MultiOptionNameSeparator, commandOptionMetadata.DisplayInfo.AlternateNames);
                        var prefixAlternateNames = MultiOptionNameSeparator;
                        if (string.IsNullOrEmpty(alternateNames))
                        {
                            prefixAlternateNames = String.Empty;
                        }
                        var optionName = string.Concat(commandOptionMetadata.DisplayInfo.Name, prefixAlternateNames, alternateNames, GetMultiValueIndicator(commandOptionMetadata));
                        var optionShortName = FormatShortName(commandOptionMetadata.DisplayInfo.ShortName);
                        _console.Write(" -{0, -" + maxOptionWidth + "}", optionName);
                        _console.Write("{0, -" + (maxAltOptionWidth + 4) + "}", optionShortName);

                        _console.Write(commandOptionMetadata.DisplayInfo.Description);
                        _console.WriteLine();
                    }
                }

                var samples = commandType.Metadata.Samples;
                if (samples.Length > 0)
                {
                    _console.WriteLine();
                    _console.WriteLine("Samples:");
                    foreach (var usage in samples)
                    {
                        _console.WriteLine(usage);
                    }
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

        internal static string GetMultiValueIndicator(ICommandOptionMetadata commandOptionMetadata)
        {
#pragma warning disable CC0073 // Add braces to switch sections.
            switch (commandOptionMetadata.CollectionType)
            {
                case CommandOptionCollectionType.Collection:
                    return CollectionIndicator;
                case CommandOptionCollectionType.Dictionary:
                    return DictionaryIndicator;
                default:
                    return string.Empty;
            }
#pragma warning restore CC0073 // Add braces to switch sections.
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
