using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

// ReSharper disable CheckNamespace

namespace MGR.CommandLineParser.Command
// ReSharper restore CheckNamespace
{
    internal static class CommandExtensions
    {
        private static readonly Dictionary<Type, CommandMetadataTemplate> CommandMetadataCache = new Dictionary<Type, CommandMetadataTemplate>();
        private static readonly Dictionary<Type, CommandMetadataTemplate> SimpleCommandMetadataCache = new Dictionary<Type, CommandMetadataTemplate>();
        private static readonly object CommandMetadataCacheLockObject = new object();
        private static readonly object SimpleCommandMetadataCacheLockObject = new object();

        private const string COMMAND_SUFFIX = "Command";

        internal static string ExtractCommandName(this ICommand source)
        {
            Guard.NotNull(source, nameof(source));

            return source.ExtractCommandMetadataTemplate().Name;
        }

        internal static CommandMetadataTemplate ExtractMetadataTemplate(this ICommand source)
        {
            Guard.NotNull(source, nameof(source));

            var commandType = source.GetType();
            lock (CommandMetadataCacheLockObject)
            {
                if (!CommandMetadataCache.ContainsKey(commandType))
                {
                    var metadata = ExtractCommandMetadataTemplate(source);
                    foreach (var propInfo in commandType.GetProperties(BindingFlags.Public | BindingFlags.Instance).Where(pi => pi.Name != nameof(ICommand.Arguments)))
                    {
                        var optionMetadata = propInfo.ExtractMetadata(metadata);
                        if (optionMetadata != null)
                        {
                            metadata.Options.Add(optionMetadata);
                        }
                    }
                    CommandMetadataCache.Add(commandType, metadata);
                }
                return CommandMetadataCache[commandType];
            }
        }

        internal static CommandMetadata ExtractMetadata(this ICommand source) => ExtractMetadataTemplate(source).ToCommandMetadata(source);

        internal static CommandMetadataTemplate ExtractCommandMetadataTemplate(this ICommand source)
        {
            Guard.NotNull(source, nameof(source));

            lock (SimpleCommandMetadataCacheLockObject)
            {
                if (!SimpleCommandMetadataCache.ContainsKey(source.GetType()))
                {
                    var metadata = new CommandMetadataTemplate();
                    var commandType = source.GetType();
                    var fullCommandName = commandType.Name;
                    if (fullCommandName.EndsWith(COMMAND_SUFFIX, StringComparison.Ordinal))
                    {
                        fullCommandName = fullCommandName.Substring(0, fullCommandName.Length - COMMAND_SUFFIX.Length);
                    }
                    metadata.Name = fullCommandName;
                    var displayAttribute = commandType.GetCustomAttributes(typeof(CommandDisplayAttribute), true).FirstOrDefault() as CommandDisplayAttribute;
                    if (displayAttribute != null)
                    {
                        metadata.Description = displayAttribute.GetLocalizedDescription();
                        metadata.Usage = displayAttribute.GetLocalizedUsage();
                    }
                    SimpleCommandMetadataCache.Add(source.GetType(), metadata);
                }
                return SimpleCommandMetadataCache[source.GetType()];
            }
        }
    }
}