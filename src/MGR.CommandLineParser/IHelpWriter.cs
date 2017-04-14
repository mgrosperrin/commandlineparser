namespace MGR.CommandLineParser
{
    /// <summary>
    ///     Display the help.
    /// </summary>
    public interface IHelpWriter
    {
        /// <summary>
        ///     Write command listing.
        /// </summary>
        /// <param name="parserOptions">The options of the parser.</param>
        void WriteCommandListing(IParserOptions parserOptions);

        /// <summary>
        ///     Write the help for a commant.
        /// </summary>
        /// <param name="parserOptions">The options of the parser.</param>
        /// <param name="commandType">The <see cref="CommandType" /> of the command.</param>
        void WriteHelpForCommand(IParserOptions parserOptions, CommandType commandType);
    }
}