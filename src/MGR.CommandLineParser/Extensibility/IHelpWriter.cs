using MGR.CommandLineParser.Extensibility.ClassBased;
using MGR.CommandLineParser.Extensibility.Command;

namespace MGR.CommandLineParser.Extensibility
{
    /// <summary>
    ///     Display the help.
    /// </summary>
    public interface IHelpWriter
    {
        /// <summary>
        ///     Write command listing.
        /// </summary>
        void WriteCommandListing();

        /// <summary>
        ///     Write the help for a command.
        /// </summary>
        /// <param name="parserOptions">The options of the parser.</param>
        /// <param name="commandTypes">The <see cref="ClassBasedCommandType" /> of the commands to display help.</param>
        void WriteHelpForCommand(IParserOptions parserOptions, params ICommandType[] commandTypes);
    }
}