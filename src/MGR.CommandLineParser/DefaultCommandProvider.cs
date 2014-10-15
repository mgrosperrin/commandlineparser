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
        private readonly List<ICommand> _commands = new List<ICommand>();

        public void BuildCommands()
        {
            _commands.Clear();
            using (var catalog = new AggregateCatalog())
            {
                string thisDirectory = new Uri(Path.GetDirectoryName(typeof(DefaultCommandProvider).Assembly.CodeBase)).AbsolutePath ?? string.Empty;
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
                    _commands.AddRange(container.GetExportedValues<ICommand>());
                }
            }
        }

        public IEnumerable<ICommand> AllCommands
        {
            get { return _commands.OrderBy(command => command.ExtractCommandName()).AsEnumerable(); }
        }

        public IHelpCommand GetHelpCommand()
        {
            return GetCommand(HelpCommand.Name) as IHelpCommand;
        }

        public ICommand GetCommand(string commandName)
        {
            return _commands.SingleOrDefault(command => command.ExtractCommandName().Equals(commandName, StringComparison.OrdinalIgnoreCase));
        }
    }
}