using System.Collections.Generic;
using JetBrains.Annotations;
using MGR.CommandLineParser.Command;
using MGR.CommandLineParser.Converters;

namespace MGR.CommandLineParser
{
    public interface IParser
    {
        [NotNull]
        string CommandLineName { get; }
        [NotNull]
        ICommandProvider CommandProvider { get; }
        [NotNull]
        IConsole Console { get; }
        [NotNull]
        IEnumerable<IConverter> Converters { get; }
        [NotNull]
        string Logo { get; }
        CommandResult<TCommand> Parse<TCommand>([ItemNotNull] IEnumerable<string> arguments) where TCommand : class, ICommand;
        CommandResult<ICommand> Parse([ItemNotNull] IEnumerable<string> arguments);
    }
}