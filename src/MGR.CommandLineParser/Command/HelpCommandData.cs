namespace MGR.CommandLineParser.Command;
/// <summary>
/// Represents the data for the help command.
/// </summary>
public class HelpCommandData : HelpedCommandData
{
    /// <summary>
    /// Show detailed help for all commands.
    /// </summary>
    public bool All { get; set; }
}