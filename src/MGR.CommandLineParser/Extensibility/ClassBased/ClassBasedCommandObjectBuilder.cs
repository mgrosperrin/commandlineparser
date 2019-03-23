using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using MGR.CommandLineParser.Command;
using MGR.CommandLineParser.Extensibility.Command;

namespace MGR.CommandLineParser.Extensibility.ClassBased
{
    internal class ClassBasedCommandObjectBuilder : CommandObjectBuilderBase<ClassBasedCommandOption>
    {
        private readonly ICommand _command;

        internal ClassBasedCommandObjectBuilder(ICommandMetadata commandMetadata, IEnumerable<ClassBasedCommandOptionMetadata> commandOptionMetadatas, ICommand command)
        :base(commandMetadata, commandOptionMetadatas.Select(metadata =>
            new ClassBasedCommandOption(metadata, command)).ToList())
        {
            _command = command;
        }

        public override void AddArguments(string argument)
        {
            _command.Arguments.Add(argument);
        }

        public override ICommandObject GenerateCommandObject() => new ClassBasedCommandObject(_command);

        protected override bool DoValidate(List<ValidationResult> validationResults, IServiceProvider serviceProvider)
        {
            var validationContext = new ValidationContext(_command, null, null);
            var isValid = Validator.TryValidateObject(_command, validationContext, validationResults, true);
            return isValid;
        }

    }
}