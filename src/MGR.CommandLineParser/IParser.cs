using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using JetBrains.Annotations;
using MGR.CommandLineParser.Command;

namespace MGR.CommandLineParser
{
    /// <summary>
    ///     Represents a parser.
    /// </summary>
    [PublicAPI]
    public interface IParser
    {
        /// <summary>
        ///     Gets the name of the current tools.
        /// </summary>
        [NotNull]
        string CommandLineName { get; }

        /// <summary>
        ///     Gets the current logo (name + version) of the current tools.
        /// </summary>
        [NotNull]
        string Logo { get; }

        /// <summary>
        ///     Parse the supplied arguments for a specific command. The name of the command should not be in the arguments list.
        /// </summary>
        /// <typeparam name="TCommand">The type of the command.</typeparam>
        /// <param name="arguments">The arguments.</param>
        /// <returns>The result of the parsing of the arguments.</returns>
        CommandResult<TCommand> Parse<TCommand>([ItemNotNull] IEnumerable<string> arguments) where TCommand : class, ICommand;

        /// <summary>
        ///     Parse the supplied arguments.
        /// </summary>
        /// <param name="arguments">The arguments.</param>
        /// <returns>The result of the parsing of the arguments.</returns>
        CommandResult<ICommand> Parse([ItemNotNull] IEnumerable<string> arguments);

        /// <summary>
        ///     Parse the supplied arguments for a specific command. The name of the command should not be in the arguments list.
        /// </summary>
        /// <typeparam name="TCommand">The type of the command.</typeparam>
        /// <param name="arguments">The arguments.</param>
        /// <returns>The result of the parsing of the arguments.</returns>
        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        CommandResult<ICommand> ParseWithDefaultCommand<TCommand>([ItemNotNull] IEnumerable<string> arguments) where TCommand : class, ICommand;
    }
}