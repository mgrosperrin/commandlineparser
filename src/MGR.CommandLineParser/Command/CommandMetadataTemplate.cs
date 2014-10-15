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

        private readonly List<OptionMetadataTemplate> _options = new List<OptionMetadataTemplate>();

        internal string Name { get; set; }
        internal string Description { get; set; }
        internal string Usage { get; set; }

        internal List<OptionMetadataTemplate> Options
        {
            get { return _options; }
        }

        internal CommandMetadata ToCommandMetadata(ICommand command)
        {
            return new CommandMetadata(this, command);
        }
    }
}