using System;
using MGR.CommandLineParser.Command;

namespace MGR.CommandLineParser.Extensibility
{
    /// <summary>
    /// Basic command activator that uses <code>Activator.CreateInstance</code> to instantiate commands.
    /// </summary>
    public sealed class BasicCommandActivator : ICommandActivator
    {
        internal static readonly ICommandActivator Instance = new BasicCommandActivator();

        private BasicCommandActivator()
        {
        }

        /// <inheritdoc />
        public ICommand ActivateCommand(Type commandType) => Activator.CreateInstance(commandType) as ICommand;
    }
}