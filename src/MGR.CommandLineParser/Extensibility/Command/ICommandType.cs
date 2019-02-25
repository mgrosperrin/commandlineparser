using System;
using System.Collections.Generic;
using MGR.CommandLineParser.Command;

namespace MGR.CommandLineParser.Extensibility.Command
{
    /// <summary>
    /// Represents a type of command.
    /// </summary>
    public interface ICommandType
    {
        /// <summary>
        /// Gets the name of the command.
        /// </summary>
        ICommandMetadata Metadata { get; }

        /// <summary>
        /// Gets the option of the command type.
        /// </summary>
        IEnumerable<ICommandOption> Options { get; }

        /// <summary>
        /// Create the command from its type.
        /// </summary>
        /// <param name="serviceProvider">The scoped dependency resolver.</param>
        /// <param name="parserOptions">The options of the current parser.</param>
        /// <returns></returns>
        ICommand CreateCommand(IServiceProvider serviceProvider, IParserOptions parserOptions);
        /// <summary>
        /// Find an option based on its name.
        /// </summary>
        /// <param name="optionName">The name (short or long form) of the option.</param>
        /// <returns>The <see cref="CommandOption"/> representing the option of the command.</returns>
        ICommandOption FindOption(string optionName);
        /// <summary>
        /// Find an option based on its short name.
        /// </summary>
        /// <param name="optionShortName">The short name of the option.</param>
        /// <returns>The <see cref="CommandOption"/> representing the option of the command.</returns>
        ICommandOption FindOptionByShortName(string optionShortName);
    }
}