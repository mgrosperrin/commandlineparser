using System.Collections.Generic;
using MGR.CommandLineParser.Converters;

namespace MGR.CommandLineParser
{
    /// <summary>
    /// Defines the options for the parser.
    /// </summary>
    public interface IParserOptions
    {
        /// <summary>
        /// The implementation of <see cref="IConsole"/> used by the parser.
        /// </summary>
        IConsole Console { get; }
        /// <summary>
        /// The implementation of <see cref="ICommandProvider"/> used by the parser.
        /// </summary>
        ICommandProvider CommandProvider { get; }
        /// <summary>
        /// The logo used in the help by the parser.
        /// </summary>
        string Logo { get; }
        /// <summary>
        /// The name of the executable to run used in the help by the parser.
        /// </summary>
        string CommandLineName { get; }
        /// <summary>
        /// The collection of <see cref="IConverter"/> used by the parser.
        /// </summary>
        IEnumerable<IConverter> Converters { get; }
    }
}