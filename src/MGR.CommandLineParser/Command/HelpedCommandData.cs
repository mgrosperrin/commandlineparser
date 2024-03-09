using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using JetBrains.Annotations;
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
}
