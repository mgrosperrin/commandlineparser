using System;
using System.Collections.Generic;
using MGR.CommandLineParser.Command;
using MGR.CommandLineParser.Extensibility.DependencyInjection;

namespace MGR.CommandLineParser.Extensibility.Command
{
    /// <summary>
    /// Represents a type of command.
    /// </summary>
    public interface ICommandType
    {
        /// <summary>
        ///     Gets the type of the command.
        /// </summary>
        Type Type { get; }

        /// <summary>
        /// Gets the name of the command.
        /// </summary>
        ICommandMetadata Metadata { get; }

        /// <summary>
        /// Gets the option of the command type.
        /// </summary>
        IEnumerable<CommandOption> Options { get; }

        /// <summary>
        /// Create the command from its type.
        /// </summary>
        /// <param name="dependencyResolver">The scoped dependendy resolver.</param>
        /// <param name="parserOptions">The options of the current parser.</param>
        /// <returns></returns>
        ICommand CreateCommand(IDependencyResolverScope dependencyResolver, IParserOptions parserOptions);
        /// <summary>
        /// Find an option based on its name.
        /// </summary>
        /// <param name="optionName">The name (short or long form) of the option.</param>
        /// <returns>The <see cref="CommandOption"/> representing the option of the command.</returns>
        CommandOption FindOption(string optionName);
        /// <summary>
        /// Find an option based on its short name.
        /// </summary>
        /// <param name="optionShortName">The short name of the option.</param>
        /// <returns>The <see cref="CommandOption"/> representing the option of the command.</returns>
        CommandOption FindOptionByShortName(string optionShortName);
    }
}