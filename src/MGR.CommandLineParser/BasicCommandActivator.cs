using System;
using MGR.CommandLineParser.Command;

namespace MGR.CommandLineParser
{
    internal sealed class BasicCommandActivator : ICommandActivator
    {
        internal static readonly ICommandActivator Instance = new BasicCommandActivator();

        private BasicCommandActivator()
        {
        }

        public ICommand ActivateCommand(Type commandType) => Activator.CreateInstance(commandType) as ICommand;
    }
}