using MGR.CommandLineParser.Extensibility.Command;

namespace MGR.CommandLineParser.Extensibility;

/// <summary>
/// Display the help.
/// </summary>
public interface IHelpWriter
{
    /// <summary>
    /// Write command listing.
    /// </summary>
    Task WriteCommandListing();

    /// <summary>
    /// Write the help for some commands.
    /// </summary>
    /// <param name="commandTypes">The <see cref="ICommandType"/> of the commands to display help.</param>
    void WriteHelpForCommand(params ICommandType[] commandTypes);
}