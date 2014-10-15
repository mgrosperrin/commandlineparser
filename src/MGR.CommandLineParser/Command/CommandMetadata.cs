using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace MGR.CommandLineParser.Command
{
    [DebuggerDisplay("{CommandMetadata:{Name}, Nb options : {Options.Count()}")]
    internal sealed class CommandMetadata
    {
        internal CommandMetadata(CommandMetadataTemplate commandMetadataTemplate, ICommand command)
        {
            if (commandMetadataTemplate == null)
            {
                throw new ArgumentNullException("commandMetadataTemplate");
            }
            if (command == null)
            {
                throw new ArgumentNullException("command");
            }
            Name = commandMetadataTemplate.Name;
            Description = commandMetadataTemplate.Description;
            Usage = commandMetadataTemplate.Usage;
            Command = command;
            _options = new List<OptionMetadata>(commandMetadataTemplate.Options.Select(option => option.ToOptionMetadata(this)));
        }

        internal readonly List<OptionMetadata> _options;

        internal string Name { get; set; }
        internal string Description { get; set; }
        internal string Usage { get; set; }

        internal IEnumerable<OptionMetadata> Options
        {
            get { return _options.AsEnumerable(); }
        }

        internal ICommand Command { get; private set; }

        internal OptionMetadata GetOption(string optionName)
        {
            OptionMetadata om = Options.FirstOrDefault(option => option.Name.Equals(optionName, StringComparison.OrdinalIgnoreCase));
            if (om != null)
            {
                return om;
            }
            return Options.FirstOrDefault(option => (option.ShortName ?? string.Empty).Equals(optionName, StringComparison.OrdinalIgnoreCase));
        }

    }
}