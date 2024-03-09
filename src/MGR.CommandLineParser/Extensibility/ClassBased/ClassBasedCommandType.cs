using System.Reflection;
using MGR.CommandLineParser.Command;
using MGR.CommandLineParser.Extensibility.Command;
using MGR.CommandLineParser.Extensibility.Converters;

namespace MGR.CommandLineParser.Extensibility.ClassBased;

internal sealed class ClassBasedCommandType : ICommandType
{
    private readonly ICommandType _typedClassBasedCommandType;

    internal ClassBasedCommandType(Type commandHandlerType, IEnumerable<IConverter> converters, IEnumerable<IPropertyOptionAlternateNameGenerator> optionAlternateNameGenerators)
    {
        Type = commandHandlerType;
        var concreteClassBasedCommandType = typeof(ClassBasedCommandType<,>).MakeGenericType(commandHandlerType, commandHandlerType.GetInterfaces()
            .FirstOrDefault(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(ICommandHandler<>))
            .GetGenericArguments()
            .Single());
        _typedClassBasedCommandType = (ICommandType)Activator.CreateInstance(concreteClassBasedCommandType, BindingFlags.Instance | BindingFlags.NonPublic, null, [converters, optionAlternateNameGenerators], null);

    }

    internal Type Type { get; }

    public ICommandMetadata Metadata => _typedClassBasedCommandType.Metadata;

    public IEnumerable<ICommandOptionMetadata> Options => _typedClassBasedCommandType.Options;

    public ICommandObjectBuilder? CreateCommandObjectBuilder(IServiceProvider serviceProvider)
        => _typedClassBasedCommandType.CreateCommandObjectBuilder(serviceProvider);
}
