using System.Collections.Generic;
using JetBrains.Annotations;
using MGR.CommandLineParser.Command;

namespace MGR.CommandLineParser
{
    [PublicAPI]
    public interface IParser
    {
        [NotNull]
        string CommandLineName { get; }

        [NotNull]
        string Logo { get; }

        CommandResult<TCommand> Parse<TCommand>([ItemNotNull] IEnumerable<string> arguments) where TCommand : class, ICommand;
        CommandResult<ICommand> Parse([ItemNotNull] IEnumerable<string> arguments);
    }
}