using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using MGR.CommandLineParser.Command;
using MGR.CommandLineParser.Converters;

namespace MGR.CommandLineParser
{
    /// <summary>
    ///     Represents a type of command.
    /// </summary>
    public sealed class CommandType
    {
        private readonly Lazy<CommandMetadata> _commandMetadata;
        private readonly Lazy<List<CommandOption>> _commandOptions;
        /// <summary>
        /// Creates a new <see cref="CommandType"/>.
        /// </summary>
        /// <param name="commandType">The type of the command.</param>
        /// <param name="converters">The converters.</param>
        public CommandType(Type commandType, IEnumerable<IConverter> converters)
        {
            Type = commandType;
            _commandMetadata = new Lazy<CommandMetadata>(() => new CommandMetadata(Type));
            _commandOptions = new Lazy<List<CommandOption>>(() => new List<CommandOption>(ExtractCommandOptions(Type, Metadata, converters)));

        }
        /// <summary>
        ///     Gets the type of the command.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1721:PropertyNamesShouldNotMatchGetMethods")]
        public Type Type { get; }
        /// <summary>
        /// Gets the name of the command.
        /// </summary>
        public CommandMetadata Metadata => _commandMetadata.Value;
        /// <summary>
        /// Gets the option of the command type.
        /// </summary>
        public IEnumerable<CommandOption> Options => _commandOptions.Value;

        internal CommandOption FindOption(string optionName)
        {
            var om = Options.FirstOrDefault(option => option.DisplayInfo.Name.Equals(optionName, StringComparison.OrdinalIgnoreCase));
            if (om != null)
            {
                return om;
            }
            return Options.FirstOrDefault(option => (option.DisplayInfo.ShortName ?? string.Empty).Equals(optionName, StringComparison.OrdinalIgnoreCase));
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
            var commandBase = command as CommandBase;
            foreach (var commandOption in Options)
            {
                commandOption.AssignDefaultValue(command);
            }
            commandBase?.Configure(parserOptions, dependencyResolver, this);
            return command;
        }

        private static IEnumerable<CommandOption> ExtractCommandOptions(Type commandType, CommandMetadata commandMetadata, IEnumerable<IConverter> converters)
        {
            foreach (var propertyInfo in commandType.GetProperties(BindingFlags.Public | BindingFlags.Instance).Where(pi => pi.Name != nameof(ICommand.Arguments)))
            {
                var commandOption = CommandOption.Create(propertyInfo, commandMetadata, converters);
                if (commandOption != null)
                {
                    yield return commandOption;
                }
            }
        }
    }
}
