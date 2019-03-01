using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using MGR.CommandLineParser.Extensibility.Converters;
using Microsoft.Extensions.DependencyInjection;

namespace MGR.CommandLineParser.Command.Lambda
{
    public class OptionBuilder
    {
        private readonly string _optionName;
        private readonly string _shortOptionName;
        private readonly List<string> _alternateNames = new List<string>();
        private string _description;
        private readonly Type _optionType;
        private string _defaultValue;
        private readonly List<ValidationAttribute> _validationAttributes = new List<ValidationAttribute>();

        public OptionBuilder(string optionName, string shortOptionName, Type optionType)
        {
            _optionName = optionName;
            _shortOptionName = shortOptionName;
            _optionType = optionType;
        }

        public OptionBuilder AddAlternateName(string alternateName)
        {
            _alternateNames.Add(alternateName);
            return this;
        }

        public OptionBuilder WithDescription(string description)
        {
            _description = description;
            return this;
        }

        public OptionBuilder WithDefaultValue(string defaultValue)
        {
            _defaultValue = defaultValue;
            return this;
        }

        public OptionBuilder AddValidation(ValidationAttribute validationAttribute)
        {
            _validationAttributes.Add(validationAttribute);
            return this;
        }

        internal LambdaBasedCommandOption ToCommandOption(IServiceProvider serviceProvider)
        {
            var converters = serviceProvider.GetServices<IConverter>();
            var converter = converters.FirstOrDefault(c => c.CanConvertTo(_optionType));

            var commandOption = new LambdaBasedCommandOption(
                new LambdaBasedComandOptionMetadata(
                    new LambdaBasedOptionDisplayInfo(_optionName, _alternateNames, _shortOptionName, _description),
                    _defaultValue,
                    _validationAttributes.OfType<RequiredAttribute>().Any(),
                    _optionType
                    ),
                _optionType,
                converter,
                _validationAttributes
                );
            return commandOption;
        }
    }
}