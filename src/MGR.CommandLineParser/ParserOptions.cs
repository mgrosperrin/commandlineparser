using System.Collections.Generic;
using MGR.CommandLineParser.Converters;

namespace MGR.CommandLineParser
{
    /// <summary>
    ///     Default implementation of <see cref="IParserOptions" />.
    /// </summary>
    public sealed class ParserOptions : IParserOptions
    {
        /// <summary>
        ///     The implementation of <see cref="IConsole" /> used by the parser.
        /// </summary>
        public IConsole Console => ServiceResolver.Current.ResolveService<IConsole>();

        /// <summary>
        ///     The implementation of <see cref="ICommandProvider" /> used by the parser.
        /// </summary>
        public ICommandProvider CommandProvider => ServiceResolver.Current.ResolveService<ICommandProvider>();

        /// <summary>
        ///     The logo used in the help by the parser.
        /// </summary>
        public string Logo { get; set; }

        /// <summary>
        ///     The name of the executable to run used in the help by the parser.
        /// </summary>
        public string CommandLineName { get; set; }

        /// <summary>
        ///     The collection of <see cref="IConverter" /> used by the parser.
        /// </summary>
        public IEnumerable<IConverter> Converters => ServiceResolver.Current.ResolveServices<IConverter>();
    }
}