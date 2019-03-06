using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using MGR.CommandLineParser.Command;
using MGR.CommandLineParser.Extensibility.Command;
using MGR.CommandLineParser.Extensibility.Converters;

namespace MGR.CommandLineParser.Extensibility.ClassBased
{
    /// <summary>
    /// Browse types in assemblies to get all commands.
    /// </summary>
    public sealed class AssemblyBrowsingClassBasedCommandTypeProvider : ICommandTypeProvider
    {
        private readonly IAssemblyProvider _assemblyProvider;
        private readonly Lazy<Dictionary<string, ClassBasedCommandType>> _commands;

        private readonly IEnumerable<IConverter> _converters;
        private readonly IEnumerable<IPropertyOptionAlternateNameGenerator> _optionAlternateNameGenerators;

        /// <summary>
        /// Create a new <see cref="AssemblyBrowsingClassBasedCommandTypeProvider"/>.
        /// </summary>
        /// <param name="assemblyProvider"></param>
        /// <param name="converters"></param>
        /// <param name="optionAlternateNameGenerators"></param>
        public AssemblyBrowsingClassBasedCommandTypeProvider(IAssemblyProvider assemblyProvider, IEnumerable<IConverter> converters, IEnumerable<IPropertyOptionAlternateNameGenerator> optionAlternateNameGenerators)
        {
            _assemblyProvider = assemblyProvider;
            _converters = converters;
            _optionAlternateNameGenerators = optionAlternateNameGenerators;
            _commands = new Lazy<Dictionary<string, ClassBasedCommandType>>(SearchAllCommandTypes);
        }

        /// <inheritdoc />
        public IEnumerable<ICommandType> GetAllCommandTypes()
        {
            var commandTypes = _commands.Value;
            return commandTypes.Values;
        }

        /// <inheritdoc />
        public ICommandType GetCommandType(string commandName)
        {
            var commandTypes = _commands.Value;
            if (commandTypes.ContainsKey(commandName))
            {
                return commandTypes[commandName];
            }
            return null;
        }

        private Dictionary<string, ClassBasedCommandType> SearchAllCommandTypes()
        {
            var assemblies = _assemblyProvider.GetAssembliesToBrowse().ToList();
            var types = assemblies.GetTypes(type => TypeExtensions.IsType<ICommand>(type)).ToList();

            var commandTypes = types.Select(commandType => new ClassBasedCommandType(commandType, _converters, _optionAlternateNameGenerators)).ToList();
            var commandTypesByName = commandTypes.ToDictionary(commandType => commandType.Metadata.Name, _ => _, StringComparer.OrdinalIgnoreCase);
            return commandTypesByName;
        }
    }
}