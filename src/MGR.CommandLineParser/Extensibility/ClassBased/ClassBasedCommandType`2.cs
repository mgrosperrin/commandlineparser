using System.Reflection;
using MGR.CommandLineParser.Command;
using MGR.CommandLineParser.Extensibility.Command;
using MGR.CommandLineParser.Extensibility.Converters;
using Microsoft.Extensions.DependencyInjection;

namespace MGR.CommandLineParser.Extensibility.ClassBased;

internal sealed class ClassBasedCommandType<TCommandHandler, TCommandData> : ICommandType
    where TCommandHandler : class, ICommandHandler<TCommandData>
    where TCommandData : CommandData, new()
{
    private readonly Lazy<ICommandMetadata> _commandMetadata;
    private readonly Lazy<List<ClassBasedCommandOptionMetadata>> _commandOptions;

    internal ClassBasedCommandType(IEnumerable<IConverter> converters, IEnumerable<IPropertyOptionAlternateNameGenerator> optionAlternateNameGenerators)
    {
        _commandMetadata = new Lazy<ICommandMetadata>(() => new ClassBasedCommandMetadata<TCommandHandler, TCommandData>());
        _commandOptions = new Lazy<List<ClassBasedCommandOptionMetadata>>(() => new List<ClassBasedCommandOptionMetadata>(ExtractCommandOptions(Metadata, converters.ToList(), optionAlternateNameGenerators.ToList())));

    }

    public ICommandMetadata Metadata => _commandMetadata.Value;

    public IEnumerable<ICommandOptionMetadata> Options => _commandOptions.Value;

    public ICommandObjectBuilder? CreateCommandObjectBuilder(IServiceProvider serviceProvider)
    {
        Guard.NotNull(serviceProvider, nameof(serviceProvider));

        var commandActivator = serviceProvider.GetRequiredService<IClassBasedCommandActivator>();
        var commandHandler = commandActivator.ActivateCommand<TCommandHandler, TCommandData>();
        if (commandHandler == null)
        {
            return null;
        }
        var commandData = new TCommandData();
        var commandObject = new ClassBasedCommandObjectBuilder<TCommandHandler, TCommandData>(Metadata, _commandOptions.Value, commandHandler, commandData);
        commandData.Configure(this);
        return commandObject;
    }

    private static IEnumerable<ClassBasedCommandOptionMetadata> ExtractCommandOptions(ICommandMetadata commandMetadata, List<IConverter> converters, List<IPropertyOptionAlternateNameGenerator> optionAlternateNameGenerators)
    {
        var commandDataType = typeof(TCommandData);
        foreach (var propertyInfo in commandDataType.GetProperties(BindingFlags.Public | BindingFlags.Instance).Where(pi => pi.Name != nameof(CommandData.Arguments)))
        {
            var commandOption = ClassBasedCommandOptionMetadata.Create(propertyInfo, commandMetadata, converters, optionAlternateNameGenerators);
            if (commandOption != null)
            {
                yield return commandOption;
            }
        }
    }
}
