using System;
using System.Collections.Generic;
using System.Linq;

namespace MGR.CommandLineParser.Extensibility.Command
{
    internal class WrapCommandOption : ICommandOption
    {
        private readonly string _commandName;
        private readonly IEnumerable<ICommandOption> _commandOptions;
        private readonly string _optionText;

        public WrapCommandOption(string optionText, string commandName, IEnumerable<ICommandOption> commandOptions)
        {
            _commandOptions = commandOptions;
            _optionText = optionText;
            _commandName = commandName;
        }

        public bool OptionalValue => _commandOptions.Aggregate(true, (optionalValue, commandOption) => optionalValue && commandOption.OptionalValue);

        public void AssignValue(string optionValue)
        {
            if (!OptionalValue && optionValue == null)
            {
                throw new CommandLineParserException(Constants.ExceptionMessages.FormatParserOptionValueRequired(_commandName, _optionText));
            }

            foreach (var commandOption in _commandOptions)
            {
                commandOption.AssignValue(optionValue);
            }
        }

        public ICommandOptionMetadata Metadata
        {
            get { throw new InvalidOperationException(); }
        }
    }
}