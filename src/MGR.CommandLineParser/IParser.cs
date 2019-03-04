using System;
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
        /// <param name="serviceProvider">The scoped service provider for the parsing operation.</param>
        /// <remarks>This method can only be used with class-based command.</remarks>
        /// <returns>The result of the parsing of the arguments.</returns>
        ParsingResult Parse<TCommand>([ItemNotNull] IEnumerable<string> arguments, IServiceProvider serviceProvider) where TCommand : class, ICommand;

        /// <summary>
        ///     Parse the supplied arguments.
        /// </summary>
        /// <param name="arguments">The arguments.</param>
        /// <param name="serviceProvider">The scoped service provider for the parsing operation.</param>
        /// <returns>The result of the parsing of the arguments.</returns>
        ParsingResult Parse([ItemNotNull] IEnumerable<string> arguments, IServiceProvider serviceProvider);

        /// <summary>
        ///     Parse the supplied arguments. If the name of the command is not the first argument, fallback to the specified command. The default command can only be class-based.
        /// </summary>
        /// <typeparam name="TCommand">The type of the default command.</typeparam>
        /// <param name="arguments">The arguments.</param>
        /// <param name="serviceProvider">The scoped service provider for the parsing operation.</param>
        /// <returns>The result of the parsing of the arguments.</returns>
        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        ParsingResult ParseWithDefaultCommand<TCommand>([ItemNotNull] IEnumerable<string> arguments, IServiceProvider serviceProvider) where TCommand : class, ICommand;
    }
}