using System;
using System.Collections.Generic;

namespace MGR.CommandLineParser.Extensibility.Command
{
    /// <summary>
    /// Represents a type of command.
    /// </summary>
    public interface ICommandType
    {
        /// <summary>
        /// Gets the metadata of the command.
        /// </summary>
        ICommandMetadata Metadata { get; }

        /// <summary>
        /// Gets the option's metadata of the command type.
        /// </summary>
        IEnumerable<ICommandOptionMetadata> Options { get; }

        /// <summary>
        /// Create the command from its type.
        /// </summary>
        /// <param name="serviceProvider">The scoped dependency resolver.</param>
        /// <param name="parserOptions">The options of the current parser.</param>
        /// <returns></returns>
        ICommandObject CreateCommand(IServiceProvider serviceProvider, IParserOptions parserOptions);
    }
}