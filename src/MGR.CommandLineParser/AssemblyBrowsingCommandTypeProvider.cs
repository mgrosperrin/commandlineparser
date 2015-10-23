using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using MGR.CommandLineParser.Command;
using MGR.CommandLineParser.Converters;

namespace MGR.CommandLineParser
{
    internal sealed class AssemblyBrowsingCommandTypeProvider : ICommandTypeProvider
    {
        private readonly IAssemblyProvider _assemblyProvider;
        private readonly Lazy<Dictionary<string, CommandType>> _commands;

        private readonly IEnumerable<IConverter> _converters;

        public AssemblyBrowsingCommandTypeProvider(IAssemblyProvider assemblyProvider, IEnumerable<IConverter> converters)
        {
            _assemblyProvider = assemblyProvider;
            _converters = converters;
            _commands = new Lazy<Dictionary<string, CommandType>>(SearchAllCommandTypes);
        }

        public IEnumerable<CommandType> GetAllCommandTypes()
        {
            var commandTypes = _commands.Value;
            return commandTypes.Values;
        }

        public CommandType GetCommandType(string commandName)
        {
            var commandTypes = _commands.Value;
            if (commandTypes.ContainsKey(commandName))
            {
                return commandTypes[commandName];
            }
            return null;
        }

        public CommandType GetCommandType<TCommand>() where TCommand : ICommand
        {
            var commandTypes = _commands.Value;
            var typeOfCommand = typeof (TCommand);
            var commandType = commandTypes.Values.FirstOrDefault(ct => ct.Type == typeOfCommand);
            if (commandType == null)
            {
                lock (this)
                {
                    commandType = commandTypes.Values.FirstOrDefault(ct => ct.Type == typeOfCommand);
                    if (commandType == null)
                    {
                        commandType = new CommandType(typeOfCommand, _converters);
                        commandTypes.Add(commandType.Metadata.Name, commandType);
                    }
                }
            }
            return commandType;
        }

        private Dictionary<string, CommandType> SearchAllCommandTypes()
        {
            var assemblies = _assemblyProvider.GetAssembliesToBrowse().ToList();
            var types = assemblies.GetTypes(type => type.IsType<ICommand>()).ToList();

            var commandTypes = types.Select(commandType => new CommandType(commandType, _converters)).ToList();
            var commandTypesByName = commandTypes.ToDictionary(commandType => commandType.Metadata.Name, StringComparer.OrdinalIgnoreCase);
            return commandTypesByName;
        }
    }
}