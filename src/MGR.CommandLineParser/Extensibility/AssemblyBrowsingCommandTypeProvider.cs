using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using MGR.CommandLineParser.Command;
using MGR.CommandLineParser.Extensibility.Command;
using MGR.CommandLineParser.Extensibility.Converters;

namespace MGR.CommandLineParser.Extensibility
{
    /// <summary>
    /// Browse types in assemblies to get all commands.
    /// </summary>
    public sealed class AssemblyBrowsingCommandTypeProvider : ICommandTypeProvider
    {
        private readonly IAssemblyProvider _assemblyProvider;
        private readonly Lazy<Dictionary<string, ICommandType>> _commands;

        private readonly IEnumerable<IConverter> _converters;

        internal AssemblyBrowsingCommandTypeProvider(IAssemblyProvider assemblyProvider, IEnumerable<IConverter> converters)
        {
            _assemblyProvider = assemblyProvider;
            _converters = converters;
            _commands = new Lazy<Dictionary<string, ICommandType>>(SearchAllCommandTypes);
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

        /// <inheritdoc />
        public ICommandType GetCommandType<TCommand>() where TCommand : ICommand
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

        private Dictionary<string, ICommandType> SearchAllCommandTypes()
        {
            var assemblies = _assemblyProvider.GetAssembliesToBrowse().ToList();
            var types = assemblies.GetTypes(type => TypeExtensions.IsType<ICommand>(type)).ToList();

            var commandTypes = types.Select(commandType => new CommandType(commandType, _converters)).ToList<ICommandType>();
            var commandTypesByName = commandTypes.ToDictionary(commandType => commandType.Metadata.Name, _ => _, StringComparer.OrdinalIgnoreCase);
            return commandTypesByName;
        }
    }
}