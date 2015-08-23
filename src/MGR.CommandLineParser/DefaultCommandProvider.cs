using System;
using System.Collections.Generic;
using System.ComponentModel.Composition.Hosting;
using System.IO;
using System.Linq;
using System.Reflection;
using MGR.CommandLineParser.Command;

namespace MGR.CommandLineParser
{
    internal sealed class DefaultCommandProvider : ICommandProvider
    {
        private readonly Lazy<List<ICommand>> _commands;

        public DefaultCommandProvider()
        {
            _commands = new Lazy<List<ICommand>>(BuildCommands);
        }

        private List<ICommand> BuildCommands()
        {
            var commands = new List<ICommand>();
            using (var catalog = new AggregateCatalog())
            {
                var directory = Path.GetDirectoryName(typeof (DefaultCommandProvider).Assembly.CodeBase);
                if (!string.IsNullOrEmpty(directory))
                {
                    string thisDirectory = new Uri(directory).AbsolutePath;
                    Assembly entryAssembly = Assembly.GetEntryAssembly();
                    if (entryAssembly != null) // could be null in test context or if the main executable is not .Net
                    {
                        string entryDirectory = Path.GetDirectoryName(entryAssembly.CodeBase);
                        if (thisDirectory.Equals(entryDirectory, StringComparison.OrdinalIgnoreCase))
                        {
                            catalog.Catalogs.Add(new AssemblyCatalog(entryAssembly));
                        }
                    }
                    foreach (var item in Directory.EnumerateFiles(thisDirectory, "*.dll", SearchOption.AllDirectories))
                    {
                        try
                        {
                            catalog.Catalogs.Add(new AssemblyCatalog(item));
                        }
                        catch (BadImageFormatException)
                        {
                            // Ignore if the dll wasn't a valid assembly
                        }
                    }
                    foreach (var item in Directory.EnumerateFiles(thisDirectory, "*.exe", SearchOption.AllDirectories))
                    {
                        try
                        {
                            catalog.Catalogs.Add(new AssemblyCatalog(item));
                        }
                        catch (BadImageFormatException)
                        {
                            // Ignore if the dll wasn't a valid assembly
                        }
                    }
                    using (var container = new CompositionContainer(catalog))
                    {
                        commands.AddRange(container.GetExportedValues<ICommand>());
                    } 
                }
            }
            return commands;
        }

        public IEnumerable<ICommand> GetAllCommands() => _commands.Value.OrderBy(command => command.ExtractCommandName()).AsEnumerable();

        public HelpCommand GetHelpCommand() => GetCommand(HelpCommand.Name) as HelpCommand;

        public ICommand GetCommand(string commandName) => _commands.Value.FirstOrDefault(command => command.ExtractCommandName().Equals(commandName, StringComparison.OrdinalIgnoreCase));
    }
}