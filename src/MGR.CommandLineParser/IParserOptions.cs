namespace MGR.CommandLineParser
{
    /// <summary>
    ///     Defines the options for the parser.
    /// </summary>
    public interface IParserOptions
    {
        /// <summary>
        ///     The logo used in the help by the parser.
        /// </summary>
        string Logo { get; }

        /// <summary>
        ///     The name of the executable to run used in the help by the parser.
        /// </summary>
        string CommandLineName { get; }
    }
}