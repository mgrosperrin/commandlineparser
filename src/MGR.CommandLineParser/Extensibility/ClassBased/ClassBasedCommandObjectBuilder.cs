using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using MGR.CommandLineParser.Command;
using MGR.CommandLineParser.Extensibility.Command;
using MGR.CommandLineParser.Properties;
using Microsoft.Extensions.DependencyInjection;

namespace MGR.CommandLineParser.Extensibility.ClassBased
{
    internal class ClassBasedCommandObjectBuilder : ICommandObjectBuilder
    {
        private readonly IEnumerable<ICommandOption> _commandOptions;
        private readonly ICommandMetadata _commandMetadata;
        private readonly ICommand _command;

        internal ClassBasedCommandObjectBuilder(ICommandMetadata commandMetadata, IEnumerable<ClassBasedCommandOptionMetadata> commandOptionMetadatas, ICommand command)
        {
            _commandMetadata = commandMetadata;
            _command = command;

            _commandOptions = commandOptionMetadatas.Select(metadata =>
                new CommandOption(metadata, command)).ToList();
        }

        public void AddArguments(string argument)
        {
            _command.Arguments.Add(argument);
        }

        public ICommandOption FindOption(string optionName)
        {
            var commandOption = _commandOptions.FirstOrDefault(option => option.Metadata.DisplayInfo.Name.Equals(optionName, StringComparison.OrdinalIgnoreCase));
            if (commandOption != null)
            {
                return commandOption;
            }
            var alternateOption = _commandOptions.FirstOrDefault(option => option.Metadata.DisplayInfo.AlternateNames.Any(alternateName => alternateName.Equals(optionName, StringComparison.OrdinalIgnoreCase)));
            return alternateOption;
        }

        public ICommandOption FindOptionByShortName(string optionShortName)
        {
            var shortOption = _commandOptions.FirstOrDefault(option => (option.Metadata.DisplayInfo.ShortName ?? string.Empty).Equals(optionShortName, StringComparison.OrdinalIgnoreCase));
            if (shortOption != null)
            {
                return shortOption;
            }
            return FindCombinedBooleanOptionsByShortName(optionShortName);
        }

        public ICommandObject Generate() => new ClassBasedCommandObject(_command);

        public CommandValidationResult Validate(IServiceProvider serviceProvider)
        {
            var validationContext = new ValidationContext(_command, null, null);
            var results = new List<ValidationResult>();

            var isValid = Validator.TryValidateObject(_command, validationContext, results, true);
            if (!isValid)
            {
                var console = serviceProvider.GetRequiredService<IConsole>();
                console.WriteError(Strings.Parser_CommandInvalidArgumentsFormat, _commandMetadata.Name);
                foreach (var validation in results)
                {
                    console.WriteError(string.Format(CultureInfo.CurrentUICulture, "-{0} :", validation.ErrorMessage));
                    foreach (var memberName in validation.MemberNames)
                    {
                        console.WriteError(string.Format(CultureInfo.CurrentUICulture, "  -{0}", memberName));
                    }
                }
            }
            return new CommandValidationResult(isValid, results);
        }

        private ICommandOption FindCombinedBooleanOptionsByShortName(string optionShortName)
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
            return new WrapCommandOption(optionShortName, _commandMetadata.Name, options);
        }
    }
}