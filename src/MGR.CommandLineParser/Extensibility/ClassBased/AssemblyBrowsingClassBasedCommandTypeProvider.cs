using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using MGR.CommandLineParser.Command;
using MGR.CommandLineParser.Extensibility.Command;
using MGR.CommandLineParser.Extensibility.Converters;

namespace MGR.CommandLineParser.Extensibility.ClassBased
{
    internal sealed class AssemblyBrowsingClassBasedCommandTypeProvider : ICommandTypeProvider
    {
        private readonly IEnumerable<IAssemblyProvider> _assemblyProviders;
        private readonly Lazy<Dictionary<string, ClassBasedCommandType>> _commands;

        private readonly IEnumerable<IConverter> _converters;
        private readonly IEnumerable<IPropertyOptionAlternateNameGenerator> _optionAlternateNameGenerators;

        public AssemblyBrowsingClassBasedCommandTypeProvider(IEnumerable<IAssemblyProvider> assemblyProviders, IEnumerable<IConverter> converters, IEnumerable<IPropertyOptionAlternateNameGenerator> optionAlternateNameGenerators)
        {
            _assemblyProviders = assemblyProviders;
            _converters = converters;
            _optionAlternateNameGenerators = optionAlternateNameGenerators;
            _commands = new Lazy<Dictionary<string, ClassBasedCommandType>>(SearchAllCommandTypes);
        }

        public IEnumerable<ICommandType> GetAllCommandTypes()
        {
            var commandTypes = _commands.Value;
            return commandTypes.Values;
        }

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
            var assemblies = _assemblyProviders.SelectMany(assemblyProvider => assemblyProvider.GetAssembliesToBrowse()).ToList();
            var types = assemblies.GetTypes(type => TypeExtensions.IsType<ICommand>(type)).ToList();

            var commandTypes = types.Select(commandType => new ClassBasedCommandType(commandType, _converters, _optionAlternateNameGenerators)).ToList();
            var commandTypesByName = commandTypes.ToDictionary(commandType => commandType.Metadata.Name, _ => _, StringComparer.OrdinalIgnoreCase);
            return commandTypesByName;
        }
    }
}