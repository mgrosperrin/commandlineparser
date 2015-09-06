using System;
using MGR.CommandLineParser.Command;

namespace MGR.CommandLineParser
{
    public interface ICommandActivator
    {
        ICommand ActivateCommand(Type commandType);
    }
}