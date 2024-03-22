using System.ComponentModel.DataAnnotations;
using JetBrains.Annotations;
using MGR.CommandLineParser.Extensibility.Command;
using MGR.CommandLineParser.Properties;

namespace MGR.CommandLineParser.Command;
/// <summary>
/// 
/// </summary>
public abstract class HelpedCommandData : CommandData
{
    /// <summary>
    ///     Gets or sets the indicator for showing the help of the command.
    /// </summary>
    [Display(ShortName = "Command_HelpOption_ShortNameMessage",
        Description = "Command_HelpOption_DescriptionMessage", ResourceType = typeof(Strings))]
    [PublicAPI]
    public bool Help { get; set; }
    /// <summary>
    ///     Gets the <see cref="CommandType" /> of the command.
    /// </summary>
    protected internal ICommandType CommandType { get; private set; }

    /// <summary>
    ///     Configure the command with the <see cref="ICommandType" /> representing the command.
    /// </summary>
    /// <param name="commandType">The <see cref="CommandType" /> of the command.</param>
    public virtual void Configure(ICommandType commandType)
    {
        CommandType = commandType;
    }
}
