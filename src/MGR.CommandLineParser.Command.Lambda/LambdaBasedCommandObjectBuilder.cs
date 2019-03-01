using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using MGR.CommandLineParser.Extensibility;
using MGR.CommandLineParser.Extensibility.Command;
using Microsoft.Extensions.DependencyInjection;

namespace MGR.CommandLineParser.Command.Lambda
{
    internal class LambdaBasedCommandObjectBuilder : CommandObjectBuilderBase<LambdaBasedCommandOption>
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly Func<CommandContext, Task<int>> _executeCommand;
        private readonly List<string> _arguments = new List<string>();
        public LambdaBasedCommandObjectBuilder(ICommandMetadata commandMetadata,
            IEnumerable<LambdaBasedCommandOption> commandOptions, IServiceProvider serviceProvider,
            Func<CommandContext, Task<int>> executeCommand)
        : base(commandMetadata, commandOptions)
        {
            _serviceProvider = serviceProvider;
            _executeCommand = executeCommand;
        }

        public override void AddArguments(string argument) => _arguments.Add(argument);

        public override ICommandObject Generate()
        {
            var commandObject = new LambdaBasedCommandObject(_executeCommand, CommandOptions, _arguments, _serviceProvider);
            return commandObject;
        }

        public override CommandValidationResult Validate(IServiceProvider serviceProvider)
        {
            var results = new List<ValidationResult>();
            var isValueValid = true;
            var instance = new object();
            foreach (var commandOption in CommandOptions)
            {
                var value = commandOption.ValueAssigner.GetValue();
                var validationContext = new ValidationContext(instance, serviceProvider, null) {
                    MemberName = commandOption.Metadata.DisplayInfo.Name
                };
                isValueValid &= Validator.TryValidateValue(value, validationContext, results, commandOption.ValidationAttributes);
            }
            if (!isValueValid)
            {
                var console = serviceProvider.GetRequiredService<IConsole>();
                WriteErrorsToConsole(console, results);
            }
            return new CommandValidationResult(isValueValid, results);
        }
    }
}