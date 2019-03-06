using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using MGR.CommandLineParser.Command;
using MGR.CommandLineParser.Extensibility.Command;
using MGR.CommandLineParser.Extensibility.Converters;
using Microsoft.Extensions.DependencyInjection;

namespace MGR.CommandLineParser.Extensibility.ClassBased
{
    internal sealed class ClassBasedCommandType : ICommandType
    {
        private readonly Lazy<ClassBasedCommandMetadata> _commandMetadata;
        private readonly Lazy<List<ClassBasedCommandOptionMetadata>> _commandOptions;

        internal ClassBasedCommandType(Type commandType, IEnumerable<IConverter> converters, IEnumerable<IPropertyOptionAlternateNameGenerator> optionAlternateNameGenerators)
        {
            Type = commandType;
            _commandMetadata = new Lazy<ClassBasedCommandMetadata>(() => new ClassBasedCommandMetadata(Type));
            _commandOptions = new Lazy<List<ClassBasedCommandOptionMetadata>>(() => new List<ClassBasedCommandOptionMetadata>(ExtractCommandOptions(Type, Metadata, converters.ToList(), optionAlternateNameGenerators.ToList())));

        }

        [SuppressMessage("Microsoft.Naming", "CA1721:PropertyNamesShouldNotMatchGetMethods")]
        internal Type Type { get; }

        public ICommandMetadata Metadata => _commandMetadata.Value;

        public IEnumerable<ICommandOptionMetadata> Options => _commandOptions.Value;

        public ICommandObjectBuilder CreateCommandObjectBuilder(IServiceProvider serviceProvider, IParserOptions parserOptions)
        {
            Guard.NotNull(serviceProvider, nameof(serviceProvider));
            Guard.NotNull(parserOptions, nameof(parserOptions));

            var commandActivator = serviceProvider.GetRequiredService<IClassBasedCommandActivator>();
            var command = commandActivator.ActivateCommand(Type);
            var commandObject = new ClassBasedCommandObjectBuilder(Metadata, _commandOptions.Value, command);

            var commandBase = command as CommandBase;
            commandBase?.Configure(parserOptions, this);
            return commandObject;
        }

        private static IEnumerable<ClassBasedCommandOptionMetadata> ExtractCommandOptions(Type commandType, ICommandMetadata commandMetadata, List<IConverter> converters, List<IPropertyOptionAlternateNameGenerator> optionAlternateNameGenerators)
        {
            foreach (var propertyInfo in commandType.GetProperties(BindingFlags.Public | BindingFlags.Instance).Where(pi => pi.Name != nameof(ICommand.Arguments)))
            {
                var commandOption = ClassBasedCommandOptionMetadata.Create(propertyInfo, commandMetadata, converters, optionAlternateNameGenerators);
                if (commandOption != null)
                {
                    yield return commandOption;
                }
            }
        }
    }
}
