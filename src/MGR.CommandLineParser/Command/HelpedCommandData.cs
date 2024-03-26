using System.ComponentModel.DataAnnotations;
using MGR.CommandLineParser.Properties;

namespace MGR.CommandLineParser.Command;
/// <summary>
/// Base class for command data with help support.
/// </summary>
public abstract class HelpedCommandData : CommandData
{
    /// <summary>
    /// Gets or sets the indicator for showing the help of the command.
    /// </summary>
    [Display(ShortName = "Command_HelpOption_ShortNameMessage",
        Description = "Command_HelpOption_DescriptionMessage", ResourceType = typeof(Strings))]
    public bool Help { get; set; }
}
