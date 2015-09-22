using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using MGR.CommandLineParser.Command;

namespace MGR.CommandLineParser
{
    internal sealed class DirectotyBrowsingCommandProvider : CommandProviderBase
    {
        private readonly IAssemblyFileProvider _assemblyProvider;
        private readonly ICommandActivator _commandActivator;

        public DirectotyBrowsingCommandProvider(IAssemblyFileProvider assemblyProvider, ICommandActivator commandActivator)
        {
            _assemblyProvider = assemblyProvider;
            _commandActivator = commandActivator;
        }


        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2001:AvoidCallingProblematicMethods", MessageId = "System.Reflection.Assembly.LoadFrom")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        protected override IEnumerable<ICommand> BuildCommands()
        {
            foreach (var assemblyFile in _assemblyProvider.GetFilesToLoad())
            {
                try
                {
                    Assembly.LoadFrom(assemblyFile);
                }

#pragma warning disable CC0004 // Catch block cannot be empty
                // ReSharper disable once EmptyGeneralCatchClause
                catch
#pragma warning restore CC0004 // Catch block cannot be empty
                { }
            }

            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            var commandTypes = assemblies.GetTypes(type => type.IsType<ICommand>()).ToList();
            return commandTypes.Select(commandType => _commandActivator.ActivateCommand(commandType)).ToList();
        }
    }
}