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
        [NotNull]
        IParser DefineCommandLineName([NotNull]string commandLineName);
        [NotNull]
        IParser DefineCommandProvider([NotNull]ICommandProvider commandProvider);
        [NotNull]
        IParser DefineConsole([NotNull]IConsole console);
        [NotNull]
        IParser DefineConverter([NotNull]IConverter converter);
        [NotNull]
        IParser DefineConverter([NotNull]IConverter converter, bool overwrite);
        [NotNull]
        IParser DefineLogo(string logo);
        CommandResult<TCommand> Parse<TCommand>([ItemNotNull] IEnumerable<string> arguments) where TCommand : class, ICommand;
        CommandResult<ICommand> Parse([ItemNotNull] IEnumerable<string> arguments);
    }
}