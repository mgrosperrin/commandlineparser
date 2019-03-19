using System.Diagnostics.CodeAnalysis;
using JetBrains.Annotations;

namespace MGR.CommandLineParser
{
    /// <summary>
    ///     Represents the constructor of a parser.
    /// </summary>
    [PublicAPI]
    public sealed class ParserBuilder
    {
        private readonly ParserOptionsBuilder _parserOptionsBuilder = ParserOptionsBuilder.Default;

        /// <summary>
        ///     Creates a new instance of <see cref="Parser" /> with the default options.
        /// </summary>
        /// <returns>A new instance of <see cref="Parser" />.</returns>
        public IParser BuildParser()
        {
            var parserOptions = _parserOptionsBuilder.ToParserOptions();
            var parser = new Parser(parserOptions);
            return parser;
        }

        /// <summary>
        ///     Changes the logo to use when creating the <see cref="Parser" />.
        /// </summary>
        /// <param name="logo">The custom logo</param>
        /// <returns>This instance of <see cref="ParserBuilder" />.</returns>
        [SuppressMessage("Microsoft.Naming", "CA1719:ParameterNamesShouldNotMatchMemberNames", MessageId = "0#")]
        public ParserBuilder Logo(string logo)
        {
            _parserOptionsBuilder.Logo = logo;
            return this;
        }

        /// <summary>
        ///     Changes the command line name to use when creating the <see cref="Parser" />.
        /// </summary>
        /// <param name="commandLineName">The custom command line name</param>
        /// <returns>This instance of <see cref="ParserBuilder" />.</returns>
        [SuppressMessage("Microsoft.Naming", "CA1719:ParameterNamesShouldNotMatchMemberNames", MessageId = "0#")]
        public ParserBuilder CommandLineName(string commandLineName)
        {
            _parserOptionsBuilder.CommandLineName = commandLineName;
            return this;
        }
    }
}