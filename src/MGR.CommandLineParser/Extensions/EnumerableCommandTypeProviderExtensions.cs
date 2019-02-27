using System.Linq;
using JetBrains.Annotations;
using MGR.CommandLineParser.Extensibility.ClassBased;
using System.Collections.Generic;

// ReSharper disable once CheckNamespace
namespace MGR.CommandLineParser.Extensibility.Command
{
    internal static class EnumerableCommandTypeProviderExtensions
    {
        internal static IEnumerable<ICommandType> GetAllVisibleCommandsTypes([NotNull, ItemNotNull]this IEnumerable<ICommandTypeProvider> commandTypeProviders)
        {
            var commands = commandTypeProviders.SelectMany(provider => provider.GetAllCommandTypes());
            return commands.Where(commandType => !commandType.Metadata.HideFromHelpListing);
        }

        internal static ICommandType GetCommandType([NotNull, ItemNotNull]this IEnumerable<ICommandTypeProvider> commandTypeProviders, string commandName)
        {
            var commands = commandTypeProviders.Select(provider => provider.GetCommandType(commandName)).Where(commandType => commandType != null);
            return commands.SingleOrDefault();
        }

        internal static ICommandType GetCommandType<TCommand>([NotNull, ItemNotNull]this IEnumerable<ICommandTypeProvider> commandTypeProviders) => commandTypeProviders.OfType<AssemblyBrowsingClassBasedCommandTypeProvider>().SelectMany(provider => provider.GetAllCommandTypes()).OfType<ClassBasedCommandType>().SingleOrDefault(commandType => commandType.Type == typeof(TCommand));
    }
}
