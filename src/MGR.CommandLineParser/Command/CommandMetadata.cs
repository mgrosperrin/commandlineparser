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
                throw new ArgumentNullException(nameof(commandMetadataTemplate));
            }
            if (command == null)
            {
                throw new ArgumentNullException(nameof(command));
            }
            Name = commandMetadataTemplate.Name;
            Description = commandMetadataTemplate.Description;
            Usage = commandMetadataTemplate.Usage;
            Command = command;
            _options = new List<OptionMetadata>(commandMetadataTemplate.Options.Select(option => option.ToOptionMetadata(this)));
        }

        private readonly List<OptionMetadata> _options;

        internal string Name { get; }
        internal string Description { get; set; }
        internal string Usage { get; set; }

        internal IEnumerable<OptionMetadata> Options => _options.AsEnumerable();

        internal ICommand Command { get; }

        internal OptionMetadata GetOption(string optionName)
        {
            var om = Options.FirstOrDefault(option => option.Name.Equals(optionName, StringComparison.OrdinalIgnoreCase));
            if (om != null)
            {
                return om;
            }
            return Options.FirstOrDefault(option => (option.ShortName ?? string.Empty).Equals(optionName, StringComparison.OrdinalIgnoreCase));
        }

    }
}