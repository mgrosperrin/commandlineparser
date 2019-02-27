using System;
using MGR.CommandLineParser.Command;

namespace MGR.CommandLineParser.Extensibility.ClassBased
{
    /// <summary>
    /// Basic command activator that uses <code>Activator.CreateInstance</code> to instantiate commands.
    /// </summary>
    public sealed class ClassBasedBasicCommandActivator : IClassBasedCommandActivator
    {
        internal static readonly IClassBasedCommandActivator Instance = new ClassBasedBasicCommandActivator();

        private ClassBasedBasicCommandActivator()
        {
        }

        /// <inheritdoc />
        public ICommand ActivateCommand(Type commandType) => Activator.CreateInstance(commandType) as ICommand;
    }
}