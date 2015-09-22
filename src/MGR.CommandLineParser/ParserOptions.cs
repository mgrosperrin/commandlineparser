namespace MGR.CommandLineParser
{
    /// <summary>
    ///     Default implementation of <see cref="IParserOptions" />.
    /// </summary>
    internal sealed class ParserOptions : IParserOptions
    {
        /// <summary>
        ///     The logo used in the help by the parser.
        /// </summary>
        public string Logo { get; set; }

        /// <summary>
        ///     The name of the executable to run used in the help by the parser.
        /// </summary>
        public string CommandLineName { get; set; }
    }
}