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
        private readonly Lazy<List<CommandOptionMetadata>> _commandOptions;
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
            _commandOptions = new Lazy<List<CommandOptionMetadata>>(() => new List<CommandOptionMetadata>(ExtractCommandOptions(Type, Metadata, converters.ToList(), optionAlternateNameGenerators)));

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
        public IEnumerable<ICommandOptionMetadata> Options => _commandOptions.Value;


        /// <summary>
        /// Create the command from its type.
        /// </summary>
        /// <param name="serviceProvider">The scoped dependendy resolver.</param>
        /// <param name="parserOptions">The options of the current parser.</param>
        /// <returns></returns>
        public ICommandObject CreateCommand(IServiceProvider serviceProvider, IParserOptions parserOptions)
        {
            Guard.NotNull(serviceProvider, nameof(serviceProvider));
            Guard.NotNull(parserOptions, nameof(parserOptions));

            var commandActivator = serviceProvider.GetRequiredService<ICommandActivator>();
            var command = commandActivator.ActivateCommand(Type);
            var commandObject = new ClassBasedCommandObject(Metadata, _commandOptions.Value, command);

            var commandBase = command as CommandBase;
            commandBase?.Configure(parserOptions, serviceProvider, this);
            return commandObject;
        }

        private static IEnumerable<CommandOptionMetadata> ExtractCommandOptions(Type commandType, ICommandMetadata commandMetadata, List<IConverter> converters, IEnumerable<IOptionAlternateNameGenerator> optionAlternateNameGenerators)
        {
            foreach (var propertyInfo in commandType.GetProperties(BindingFlags.Public | BindingFlags.Instance).Where(pi => pi.Name != nameof(ICommand.Arguments)))
            {
                var commandOption = CommandOptionMetadata.Create(propertyInfo, commandMetadata, converters, optionAlternateNameGenerators);
                if (commandOption != null)
                {
                    yield return commandOption;
                }
            }
        }
    }
}
