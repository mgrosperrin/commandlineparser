using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MGR.CommandLineParser.Extensibility.Command;

namespace MGR.CommandLineParser.Command
{
    internal class ClassBasedCommandObject : ICommandObject
    {
        private readonly IEnumerable<CommandOptionMetadata> _commandOptionMetadatas;
        private readonly IEnumerable<ICommandOption> _commandOptions;
        private readonly ICommandMetadata _commandMetadata;

        public ClassBasedCommandObject(ICommandMetadata commandMetadata, IEnumerable<CommandOptionMetadata> commandOptionMetadatas, ICommand command)
        {
            _commandOptionMetadatas = commandOptionMetadatas;
            _commandMetadata = commandMetadata;
            Command = command;

            _commandOptions = _commandOptionMetadatas.Select(metadata =>
                new CommandOption(metadata, command)).ToList();
        }

        public ICommand Command { get; }

        public void AddArguments(string argument)
        {
            Command.Arguments.Add(argument);
        }


        /// <inheritdoc />
        public ICommandOption FindOption(string optionName)
        {
            var unwrappedOption = FindUnwrappedOption(optionName);
            if (unwrappedOption != null)
            {
                return new WrapCommandOption(optionName, _commandMetadata.Name, unwrappedOption);
            }

            return null;
        }

        private ICommandOption FindUnwrappedOption(string optionName)
        {
            var om = _commandOptions.FirstOrDefault(option => option.Metadata.DisplayInfo.Name.Equals(optionName, StringComparison.OrdinalIgnoreCase));
            if (om != null)
            {
                return om;
            }
            var alternateOption = _commandOptions.FirstOrDefault(option => option.Metadata.DisplayInfo.AlternateNames.Any(alternateName => alternateName.Equals(optionName, StringComparison.OrdinalIgnoreCase)));
            return alternateOption;
        }

        public ICommandOption FindOptionByShortName(string optionShortName)
        {
            var unwrappedOptions = FindUnwrappedOptionByShortName(optionShortName);
            if (unwrappedOptions != null)
            {
                return new WrapCommandOption(optionShortName, _commandMetadata.Name, unwrappedOptions);
            }

            return null;
        }

        public Task<int> ExecuteAsync() => Command.ExecuteAsync();

        private ICommandOption[] FindUnwrappedOptionByShortName(string optionShortName)
        {
            var shortOption = _commandOptions.FirstOrDefault(option => (option.Metadata.DisplayInfo.ShortName ?? string.Empty).Equals(optionShortName, StringComparison.OrdinalIgnoreCase));
            if (shortOption != null)
            {
                return new[] { shortOption };
            }
            return FindUnwrappedCombinedBooleanOptionsByShortName(optionShortName);
        }

        private ICommandOption[] FindUnwrappedCombinedBooleanOptionsByShortName(string optionShortName)
        {
            var shortName = optionShortName;
            var options = new List<ICommandOption>();
            while (!string.IsNullOrEmpty(shortName))
            {
                var shortOption = _commandOptions.FirstOrDefault(option => !string.IsNullOrEmpty(option.Metadata.DisplayInfo.ShortName) && shortName.StartsWith(option.Metadata.DisplayInfo.ShortName, StringComparison.OrdinalIgnoreCase));
                if (shortOption != null)
                {
                    options.Add(shortOption);
                    shortName = shortName.Substring(shortOption.Metadata.DisplayInfo.ShortName.Length);
                }
                else
                {
                    return null;
                }
            }
            return options.ToArray();

        }
    }
}