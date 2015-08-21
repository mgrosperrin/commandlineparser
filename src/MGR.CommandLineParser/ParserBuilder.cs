namespace MGR.CommandLineParser
{
    /// <summary>
    ///     Represents the constructor of a parser.
    /// </summary>
    public sealed class ParserBuilder
    {
        private readonly ParserBuilderOptions _parserBuilderOptions = ParserBuilderOptions.Default;

        /// <summary>
        ///     Creates a new instance of <see cref="Parser" /> with the default options.
        /// </summary>
        /// <returns>A new instance of <see cref="Parser" />.</returns>
        public IParser BuildParser()
        {
            var parserOptions = _parserBuilderOptions.ToParserOptions();
            var parser = new Parser(parserOptions);
            return parser;
        }

        /// <summary>
        ///     Changes the logo to use when creating the <see cref="Parser" />.
        /// </summary>
        /// <param name="logo">The custom logo</param>
        /// <returns>This instance of <see cref="ParserBuilder" />.</returns>
        public ParserBuilder Logo(string logo)
        {
            _parserBuilderOptions.Logo = logo;
            return this;
        }

        /// <summary>
        ///     Changes the command line name to use when creating the <see cref="Parser" />.
        /// </summary>
        /// <param name="commandLineName">The custom command line name</param>
        /// <returns>This instance of <see cref="ParserBuilder" />.</returns>
        public ParserBuilder CommandLineName(string commandLineName)
        {
            _parserBuilderOptions.CommandLineName = commandLineName;
            return this;
        }
    }
}