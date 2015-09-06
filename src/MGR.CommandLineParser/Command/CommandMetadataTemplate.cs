using System.Collections.Generic;
using System.Diagnostics;

namespace MGR.CommandLineParser.Command
{
    [DebuggerDisplay("{CommandTemplate:{Name}, Nb options : {Options.Count}")]
    internal sealed class CommandMetadataTemplate
    {
        internal CommandMetadataTemplate()
        {
            Description = string.Empty;
            Usage = string.Empty;
        }

        internal string Name { get; set; }
        internal string Description { get; set; }
        internal string Usage { get; set; }

        internal List<OptionMetadataTemplate> Options { get; } = new List<OptionMetadataTemplate>();

        internal CommandMetadata ToCommandMetadata(ICommand command) => new CommandMetadata(this, command);
    }
}