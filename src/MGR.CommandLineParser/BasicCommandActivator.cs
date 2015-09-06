using System;
using MGR.CommandLineParser.Command;

namespace MGR.CommandLineParser
{
    internal sealed class BasicCommandActivator : ICommandActivator
    {
        public ICommand ActivateCommand(Type commandType) => Activator.CreateInstance(commandType) as ICommand;
    }
}