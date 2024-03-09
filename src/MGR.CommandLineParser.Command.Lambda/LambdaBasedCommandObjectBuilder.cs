using System.ComponentModel.DataAnnotations;
using MGR.CommandLineParser.Extensibility.Command;

namespace MGR.CommandLineParser.Command.Lambda;

internal class LambdaBasedCommandObjectBuilder : CommandObjectBuilderBase<LambdaBasedCommandOption>
{
    private readonly List<string> _arguments = [];
    private readonly Func<CommandExecutionContext, CancellationToken, Task<int>> _executeCommand;
    private readonly IServiceProvider _serviceProvider;

    internal LambdaBasedCommandObjectBuilder(ICommandMetadata commandMetadata,
        IEnumerable<LambdaBasedCommandOption> commandOptions, IServiceProvider serviceProvider,
        Func<CommandExecutionContext, CancellationToken, Task<int>> executeCommand)
        : base(commandMetadata, commandOptions)
    {
        _serviceProvider = serviceProvider;
        _executeCommand = executeCommand;
    }

    public override void AddArguments(string argument) => _arguments.Add(argument);

    public override ICommandObject GenerateCommandObject()
    {
        var commandObject = new LambdaBasedCommandObject(_executeCommand, CommandOptions, _arguments, _serviceProvider);
        return commandObject;
    }

    protected override bool DoValidate(List<ValidationResult> validationResults, IServiceProvider serviceProvider)
    {
        var isValueValid = true;
        var instance = new object();
        foreach (var commandOption in CommandOptions)
        {
            var value = commandOption.ValueAssigner.GetValue();
            var validationContext = new ValidationContext(instance, serviceProvider, null) {
                MemberName = commandOption.Metadata.DisplayInfo.Name
            };
            isValueValid &= Validator.TryValidateValue(value, validationContext, validationResults, commandOption.ValidationAttributes);
        }
        return isValueValid;
    }
}