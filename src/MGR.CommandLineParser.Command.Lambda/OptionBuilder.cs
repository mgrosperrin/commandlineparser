using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using MGR.CommandLineParser.Extensibility.Converters;
using Microsoft.Extensions.DependencyInjection;

namespace MGR.CommandLineParser.Command.Lambda
{
    /// <summary>
    /// Represents the object used to build an option of a lambda-based command.
    /// </summary>
    public class OptionBuilder
    {
        private readonly string _optionName;
        private readonly string _shortOptionName;
        private readonly List<string> _alternateNames = new List<string>();
        private string _description;
        private readonly Type _optionType;
        private string _defaultValue;
        private readonly List<ValidationAttribute> _validationAttributes = new List<ValidationAttribute>();

        internal OptionBuilder(string optionName, string shortOptionName, Type optionType)
        {
            _optionName = optionName;
            _shortOptionName = shortOptionName;
            _optionType = optionType;
        }

        /// <summary>
        /// Add an alternate name for the option.
        /// </summary>
        /// <param name="alternateName">The alternate name.</param>
        /// <returns>The <see cref="OptionBuilder"/> to chain calls.</returns>
        public OptionBuilder AddAlternateName(string alternateName)
        {
            _alternateNames.Add(alternateName);
            return this;
        }

        /// <summary>
        /// Defines the description of the option.
        /// </summary>
        /// <param name="description">The description of the option.</param>
        /// <returns>The <see cref="OptionBuilder"/> to chain calls.</returns>
        public OptionBuilder WithDescription(string description)
        {
            _description = description;
            return this;
        }

        /// <summary>
        /// Defines the default value of the option.
        /// </summary>
        /// <param name="defaultValue">The default value of the option.</param>
        /// <returns>The <see cref="OptionBuilder"/> to chain calls.</returns>
        public OptionBuilder WithDefaultValue(string defaultValue)
        {
            _defaultValue = defaultValue;
            return this;
        }

        /// <summary>
        /// Add a validation to the option.
        /// </summary>
        /// <param name="validationAttribute">A validation.</param>
        /// <returns>The <see cref="OptionBuilder"/> to chain calls.</returns>
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
                new LambdaBasedCommandOptionMetadata(
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