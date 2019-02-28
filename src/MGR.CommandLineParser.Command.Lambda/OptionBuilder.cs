using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using MGR.CommandLineParser.Extensibility.Command;

namespace MGR.CommandLineParser.Command.Lambda
{
    public class OptionBuilder
    {
        private readonly string _optionName;
        private readonly string _shortOptionName;
        private readonly Type _optionType;
        private readonly List<ValidationAttribute> _validationAttributes = new List<ValidationAttribute>();

        public OptionBuilder(string optionName, string shortOptionName, Type optionType)
        {
            _optionName = optionName;
            _shortOptionName = shortOptionName;
            _optionType = optionType;
        }
        public OptionBuilder AddValidation(ValidationAttribute validationAttribute)
        {
            _validationAttributes.Add(validationAttribute);
            return this;
        }

        internal ICommandOption ToCommandOption()
        {
            throw new NotImplementedException();
        }
    }
}