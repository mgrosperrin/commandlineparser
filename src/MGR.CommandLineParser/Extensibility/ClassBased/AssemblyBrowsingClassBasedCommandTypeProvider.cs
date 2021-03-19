using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using MGR.CommandLineParser.Command;
using MGR.CommandLineParser.Extensibility.Command;
using MGR.CommandLineParser.Extensibility.Converters;

namespace MGR.CommandLineParser.Extensibility.ClassBased
{
    internal sealed class AssemblyBrowsingClassBasedCommandTypeProvider : ICommandTypeProvider
    {
        private readonly IEnumerable<IAssemblyProvider> _assemblyProviders;
        private readonly Lazy<Dictionary<string, ICommandType>> _commands;

        private readonly IEnumerable<IConverter> _converters;
        private readonly IEnumerable<IPropertyOptionAlternateNameGenerator> _optionAlternateNameGenerators;

        public AssemblyBrowsingClassBasedCommandTypeProvider(IEnumerable<IAssemblyProvider> assemblyProviders, IEnumerable<IConverter> converters, IEnumerable<IPropertyOptionAlternateNameGenerator> optionAlternateNameGenerators)
        {
            _assemblyProviders = assemblyProviders;
            _converters = converters;
            _optionAlternateNameGenerators = optionAlternateNameGenerators;
            _commands = new Lazy<Dictionary<string, ICommandType>>(SearchAllCommandTypes);
        }

        public Task<IEnumerable<ICommandType>> GetAllCommandTypes()
        {
            var commandTypes = _commands.Value;
            return Task.FromResult<IEnumerable<ICommandType>>(commandTypes.Values);
        }

        public Task<ICommandType> GetCommandType(string commandName)
        {
            var commandTypes = _commands.Value;
            if (commandTypes.ContainsKey(commandName))
            {
                return Task.FromResult(commandTypes[commandName]);
            }
            return Task.FromResult<ICommandType>(null);
        }

        private Dictionary<string, ICommandType> SearchAllCommandTypes()
        {
            var assemblies = _assemblyProviders.SelectMany(assemblyProvider => assemblyProvider.GetAssembliesToBrowse()).ToList();
            var types = assemblies.GetTypes(type => type.IsType<ICommand>()).ToList();

            var commandTypes = types.Select<Type, ICommandType>(commandType => new ClassBasedCommandType(commandType, _converters, _optionAlternateNameGenerators)).ToList();
            var commandTypesByName = commandTypes.ToDictionary(commandType => commandType.Metadata.Name, _ => _, StringComparer.OrdinalIgnoreCase);
            return commandTypesByName;
        }
    }
}