using System;
using System.Collections.Generic;
using System.Text;

namespace MGR.CommandLineParser.Command;
/// <summary>
/// 
/// </summary>
public class CommandData
{
    /// <summary>
    /// The list of arguments of the command.
    /// </summary>
    public IList<string> Arguments { get; } = new List<string>();
}
