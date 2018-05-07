﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using MGR.CommandLineParser.Command;
using MGR.CommandLineParser.Extensibility;
using MGR.CommandLineParser.Extensibility.Command;
using MGR.CommandLineParser.Extensibility.Converters;
using MGR.CommandLineParser.Extensibility.DependencyInjection;

namespace MGR.CommandLineParser
{
    /// <summary>
    ///     Represents a type of command.
    /// </summary>
    internal sealed class CommandType : ICommandType
    {
        private readonly Lazy<CommandMetadata> _commandMetadata;
        private readonly Lazy<List<Option>> _commandOptions;
        /// <summary>
        /// Creates a new <see cref="CommandType"/>.
        /// </summary>
        /// <param name="commandType">The type of the command.</param>
        /// <param name="converters">The converters.</param>
        public CommandType(Type commandType, IEnumerable<IConverter> converters)
        {
            Type = commandType;
            _commandMetadata = new Lazy<CommandMetadata>(() => new CommandMetadata(Type));
            _commandOptions = new Lazy<List<Option>>(() => new List<Option>(ExtractCommandOptions(Type, Metadata, converters.ToList())));

        }
        /// <summary>
        ///     Gets the type of the command.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1721:PropertyNamesShouldNotMatchGetMethods")]
        public Type Type { get; }
        /// <summary>
        /// Gets the name of the command.
        /// </summary>
        public ICommandMetadata Metadata => _commandMetadata.Value;
        /// <summary>
        /// Gets the option of the command type.
        /// </summary>
        public IEnumerable<ICommandOptionMetadata> Options => _commandOptions.Value.Select(option => option.CommandOptionMetadata);

        internal IEnumerable<ICommandOption> CommandOptions => _commandOptions.Value.Select(option => option.CommandOption);

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
            var om = _commandOptions.Value.FirstOrDefault(option => option.CommandOptionMetadata.DisplayInfo.Name.Equals(optionName, StringComparison.OrdinalIgnoreCase));
            if (om != null)
            {
                return om.CommandOption;
            }
            var alternateOption = _commandOptions.Value.FirstOrDefault(option => option.CommandOptionMetadata.DisplayInfo.AlternateNames.Any(alternateName => alternateName.Equals(optionName, StringComparison.OrdinalIgnoreCase)));
            return alternateOption?.CommandOption;
        }

        public ICommandOption FindOptionByShortName(string optionShortName)
        {
            var unwrappedOption = FindUnwrappedOptionByShortName(optionShortName);
            if (unwrappedOption != null)
            {
                return new WrapCommandOption(optionShortName, Metadata.Name, unwrappedOption);
            }

            return null;
        }

        private ICommandOption FindUnwrappedOptionByShortName(string optionShortName)
        {
            var shortOption = _commandOptions.Value.FirstOrDefault(option => (option.CommandOptionMetadata.DisplayInfo.ShortName ?? string.Empty).Equals(optionShortName, StringComparison.OrdinalIgnoreCase));
            if (shortOption != null)
            {
                return shortOption.CommandOption;
            }
            return null;
        }

        /// <summary>
        /// Create the command from its type.
        /// </summary>
        /// <param name="dependencyResolver">The scoped dependendy resolver.</param>
        /// <param name="parserOptions">The options of the current parser.</param>
        /// <returns></returns>
        public ICommand CreateCommand(IDependencyResolverScope dependencyResolver, IParserOptions parserOptions)
        {
            Guard.NotNull(dependencyResolver, nameof(dependencyResolver));
            Guard.NotNull(parserOptions, nameof(parserOptions));

            var commandActivator = dependencyResolver.ResolveDependency<ICommandActivator>();
            var command = commandActivator.ActivateCommand(Type);
            foreach (var commandOption in _commandOptions.Value)
            {
                if (!string.IsNullOrEmpty(commandOption.CommandOptionMetadata.DefaultValue))
                {
                    commandOption.CommandOption.AssignValue(commandOption.CommandOptionMetadata.DefaultValue, command);
                }
            }
            var commandBase = command as CommandBase;
            commandBase?.Configure(parserOptions, dependencyResolver, this);
            return command;
        }

        private static IEnumerable<Option> ExtractCommandOptions(Type commandType, ICommandMetadata commandMetadata, List<IConverter> converters)
        {
            foreach (var propertyInfo in commandType.GetProperties(BindingFlags.Public | BindingFlags.Instance).Where(pi => pi.Name != nameof(ICommand.Arguments)))
            {
                var commandOption = CommandOption.Create(propertyInfo, commandMetadata, converters);
                if (commandOption != null)
                {
                    var commandOptionMetadata = CommandOptionMetadata.Create(propertyInfo, commandMetadata);
                    if (commandOptionMetadata != null)
                    {
                        yield return new Option(commandOption, commandOptionMetadata);
                    }
                }
            }
        }
    }
}
