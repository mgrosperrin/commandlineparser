using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
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
        var theType = typeof(ClassBasedCommandType<,>).MakeGenericType(commandHandlerType, commandHandlerType.GetInterfaces()
            .FirstOrDefault(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(ICommandHandler<>))
            .GetGenericArguments()
            .Single());
        _typedClassBasedCommandType = (ICommandType)Activator.CreateInstance(theType, BindingFlags.Instance | BindingFlags.NonPublic, null, [converters, optionAlternateNameGenerators], null);

    }

    [SuppressMessage("Microsoft.Naming", "CA1721:PropertyNamesShouldNotMatchGetMethods")]
    internal Type Type { get; }

    public ICommandMetadata Metadata => _typedClassBasedCommandType.Metadata;

    public IEnumerable<ICommandOptionMetadata> Options => _typedClassBasedCommandType.Options;

    public ICommandObjectBuilder CreateCommandObjectBuilder(IServiceProvider serviceProvider)
        => _typedClassBasedCommandType.CreateCommandObjectBuilder(serviceProvider);
}
