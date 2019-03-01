using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using MGR.CommandLineParser.Command;
using MGR.CommandLineParser.Extensibility.Command;
using Microsoft.Extensions.DependencyInjection;

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

        public override ICommandObject Generate() => new ClassBasedCommandObject(_command);

        public override CommandValidationResult Validate(IServiceProvider serviceProvider)
        {
            var validationContext = new ValidationContext(_command, null, null);
            var results = new List<ValidationResult>();

            var isValid = Validator.TryValidateObject(_command, validationContext, results, true);
            if (!isValid)
            {
                var console = serviceProvider.GetRequiredService<IConsole>();
                WriteErrorsToConsole(console, results);
            }
            return new CommandValidationResult(isValid, results);
        }

    }
}