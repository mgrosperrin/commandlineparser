using System.ComponentModel.DataAnnotations;
using MGR.CommandLineParser.Command;
using MGR.CommandLineParser.Extensibility.Command;

namespace MGR.CommandLineParser.Extensibility.ClassBased;

internal class ClassBasedCommandObjectBuilder<TCommandHandler, TCommandData> : CommandObjectBuilderBase<ClassBasedCommandOption>
    where TCommandHandler : class, ICommandHandler<TCommandData>
    where TCommandData : CommandData, new()
{
    private readonly TCommandData _commandData;
    private readonly TCommandHandler _commandHandler;

    internal ClassBasedCommandObjectBuilder(ICommandMetadata commandMetadata, IEnumerable<ClassBasedCommandOptionMetadata> commandOptionMetadatas, TCommandHandler commandHandler, TCommandData commandData)
    : base(commandMetadata, commandOptionMetadatas.Select(metadata =>
        new ClassBasedCommandOption(metadata, commandData)).ToList())
    {
        _commandHandler = commandHandler;
        _commandData = commandData;
    }

    public override void AddArguments(string argument)
    {
        _commandData.Arguments.Add(argument);
    }

    public override ICommandObject GenerateCommandObject() => new ClassBasedCommandObject<TCommandHandler, TCommandData>(_commandHandler, _commandData);

    protected override bool DoValidate(List<ValidationResult> validationResults, IServiceProvider serviceProvider)
    {
        if (_commandData is HelpedCommandData commandBase && commandBase.Help)
        {
            return true;
        }
        var validationContext = new ValidationContext(_commandData, null, null);
        var isValid = Validator.TryValidateObject(_commandData, validationContext, validationResults, true);
        return isValid;
    }

}