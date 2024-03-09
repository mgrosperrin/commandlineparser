using MGR.CommandLineParser.Extensibility.ClassBased;

namespace MGR.CommandLineParser.Extensibility.Command;

internal static class EnumerableCommandTypeProviderExtensions
{
    internal static async Task<IEnumerable<ICommandType>> GetAllVisibleCommandsTypes(this IEnumerable<ICommandTypeProvider> commandTypeProviders)
    {
        var visibleCommandTypes = new List<ICommandType>();
        foreach (var commandTypeProvider in commandTypeProviders)
        {
            var commandTypes = await commandTypeProvider.GetAllCommandTypes();
            foreach (var commandType in commandTypes)
            {
                if (!commandType.Metadata.HideFromHelpListing)
                {
                    visibleCommandTypes.Add(commandType);
                }
            }
        }

        return visibleCommandTypes;
    }

    internal static async Task<ICommandType> GetCommandType(this IEnumerable<ICommandTypeProvider> commandTypeProviders, string commandName)
    {
        var commandTypes = new List<ICommandType>();
        foreach (var commandTypeProvider in commandTypeProviders)
        {
            var commandType = await commandTypeProvider.GetCommandType(commandName);
            if (commandType != null)
            {
                commandTypes.Add(commandType);
            }
        }

        return commandTypes.SingleOrDefault();
    }

    internal static async Task<ICommandType> GetCommandType<TCommand>(this IEnumerable<ICommandTypeProvider> commandTypeProviders)
    {
        var commandType = default(ICommandType);
        foreach (var commandTypeProvider in commandTypeProviders.OfType<AssemblyBrowsingClassBasedCommandTypeProvider>())
        {
            var commands = await commandTypeProvider.GetAllCommandTypes();
            foreach (var classBasedCommandType in commands.OfType<ClassBasedCommandType>())
            {
                if (classBasedCommandType.Type == typeof(TCommand))
                {
                    if (commandType != default(ICommandType))
                    {
                        throw new InvalidOperationException();
                    }
                    commandType = classBasedCommandType;
                }
            }
        }

        return commandType ?? throw new ArgumentException();
    }
}
