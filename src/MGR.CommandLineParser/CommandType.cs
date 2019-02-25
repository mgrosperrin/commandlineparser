using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using MGR.CommandLineParser.Command;
using MGR.CommandLineParser.Extensibility;
using MGR.CommandLineParser.Extensibility.Command;
using MGR.CommandLineParser.Extensibility.Converters;
using Microsoft.Extensions.DependencyInjection;

namespace MGR.CommandLineParser
{
    /// <summary>
    ///     Represents a type of command.
    /// </summary>
    internal sealed class CommandType : ICommandType
    {
        private readonly Lazy<CommandMetadata> _commandMetadata;
        private readonly Lazy<List<ICommandOption>> _commandOptions;
        /// <summary>
        /// Creates a new <see cref="CommandType"/>.
        /// </summary>
        /// <param name="commandType">The type of the command.</param>
        /// <param name="converters">The converters.</param>
        /// <param name="optionAlternateNameGenerators">The generators of alternate name..</param>
        public CommandType(Type commandType, IEnumerable<IConverter> converters, IEnumerable<IOptionAlternateNameGenerator> optionAlternateNameGenerators)
        {
            Type = commandType;
            _commandMetadata = new Lazy<CommandMetadata>(() => new CommandMetadata(Type));
            _commandOptions = new Lazy<List<ICommandOption>>(() => new List<ICommandOption>(ExtractCommandOptions(Type, Metadata, converters.ToList(), optionAlternateNameGenerators)));

        }
        /// <summary>
        ///     Gets the type of the command.
        /// </summary>
        [SuppressMessage("Microsoft.Naming", "CA1721:PropertyNamesShouldNotMatchGetMethods")]
        public Type Type { get; }
        /// <summary>
        /// Gets the name of the command.
        /// </summary>
        public ICommandMetadata Metadata => _commandMetadata.Value;
        /// <summary>
        /// Gets the option of the command type.
        /// </summary>
        public IEnumerable<ICommandOption> Options => _commandOptions.Value;

        /// <inheritdoc />
        public ICommandOption FindOption(string optionName)
        {
            var unwrappedOption = FindUnwrappedOption(optionName);
            if (unwrappedOption != null)
            {
                return new WrapCommandOption(optionName, Metadata.Name, unwrappedOption);
            }

            return null;
        }

        private ICommandOption FindUnwrappedOption(string optionName)
        {
            var om = _commandOptions.Value.FirstOrDefault(option => option.Metadata.DisplayInfo.Name.Equals(optionName, StringComparison.OrdinalIgnoreCase));
            if (om != null)
            {
                return om;
            }
            var alternateOption = _commandOptions.Value.FirstOrDefault(option => option.Metadata.DisplayInfo.AlternateNames.Any(alternateName => alternateName.Equals(optionName, StringComparison.OrdinalIgnoreCase)));
            return alternateOption;
        }

        public ICommandOption FindOptionByShortName(string optionShortName)
        {
            var unwrappedOptions = FindUnwrappedOptionByShortName(optionShortName);
            if (unwrappedOptions != null)
            {
                return new WrapCommandOption(optionShortName, Metadata.Name, unwrappedOptions);
            }

            return null;
        }

        private ICommandOption[] FindUnwrappedOptionByShortName(string optionShortName)
        {
            var shortOption = _commandOptions.Value.FirstOrDefault(option => (option.Metadata.DisplayInfo.ShortName ?? string.Empty).Equals(optionShortName, StringComparison.OrdinalIgnoreCase));
            if (shortOption != null)
            {
                return new[]{ shortOption};
            }
            return FindUnwrappedCombinedBooleanOptionsByShortName(optionShortName);
        }

        private ICommandOption[] FindUnwrappedCombinedBooleanOptionsByShortName(string optionShortName)
        {
            var shortName = optionShortName;
            var options = new List<ICommandOption>();
            while (!string.IsNullOrEmpty(shortName))
            {
                var shortOption = _commandOptions.Value.FirstOrDefault(option => !string.IsNullOrEmpty(option.Metadata.DisplayInfo.ShortName) && shortName.StartsWith(option.Metadata.DisplayInfo.ShortName, StringComparison.OrdinalIgnoreCase));
                if (shortOption != null)
                {
                    options.Add(shortOption);
                    shortName = shortName.Substring(shortOption.Metadata.DisplayInfo.ShortName.Length);
                }
                else
                {
                    return null;
                }
            }
            return options.ToArray();

        }

        /// <summary>
        /// Create the command from its type.
        /// </summary>
        /// <param name="serviceProvider">The scoped dependendy resolver.</param>
        /// <param name="parserOptions">The options of the current parser.</param>
        /// <returns></returns>
        public ICommand CreateCommand(IServiceProvider serviceProvider, IParserOptions parserOptions)
        {
            Guard.NotNull(serviceProvider, nameof(serviceProvider));
            Guard.NotNull(parserOptions, nameof(parserOptions));

            var commandActivator = serviceProvider.GetRequiredService<ICommandActivator>();
            var command = commandActivator.ActivateCommand(Type);
            foreach (var commandOption in _commandOptions.Value)
            {
                if (!string.IsNullOrEmpty(commandOption.Metadata.DefaultValue))
                {
                    commandOption.AssignValue(commandOption.Metadata.DefaultValue, command);
                }
            }
            var commandBase = command as CommandBase;
            commandBase?.Configure(parserOptions, serviceProvider, this);
            return command;
        }

        private static IEnumerable<ICommandOption> ExtractCommandOptions(Type commandType, ICommandMetadata commandMetadata, List<IConverter> converters, IEnumerable<IOptionAlternateNameGenerator> optionAlternateNameGenerators)
        {
            foreach (var propertyInfo in commandType.GetProperties(BindingFlags.Public | BindingFlags.Instance).Where(pi => pi.Name != nameof(ICommand.Arguments)))
            {
                var commandOption = CommandOption.Create(propertyInfo, commandMetadata, converters, optionAlternateNameGenerators);
                if (commandOption != null)
                {
                    yield return commandOption;
                }
            }
        }
    }
}
