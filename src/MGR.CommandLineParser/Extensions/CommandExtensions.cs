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

        private const string COMMAND_SUFFIX = nameof(Command);

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
                    ExtractOptionMetadataTemplates(commandType, metadata);
                    CommandMetadataCache.Add(commandType, metadata);
                }
                return CommandMetadataCache[commandType];
            }
        }

        private static void ExtractOptionMetadataTemplates(Type commandType, CommandMetadataTemplate metadata)
        {
            foreach (var propInfo in commandType.GetProperties(BindingFlags.Public | BindingFlags.Instance).Where(pi => pi.Name != nameof(ICommand.Arguments)))
            {
                var optionMetadataTemplate = propInfo.ExtractOptionMetadataTemplate(metadata);
                if (optionMetadataTemplate != null)
                {
                    metadata.Options.Add(optionMetadataTemplate);
                }
            }
        }

        internal static CommandMetadata ExtractMetadata(this ICommand source) => ExtractMetadataTemplate(source).ToCommandMetadata(source);

        internal static CommandMetadataTemplate ExtractCommandMetadataTemplate(this ICommand source)
        {
            Guard.NotNull(source, nameof(source));

            lock (SimpleCommandMetadataCacheLockObject)
            {
                var commandType = source.GetType();
                if (!SimpleCommandMetadataCache.ContainsKey(commandType))
                {
                    var metadata = new CommandMetadataTemplate();
                    var fullCommandName = GetFullCommandName(commandType);
                    metadata.Name = fullCommandName;
                    ExtractCommandDisplayInformation(commandType, metadata);
                    SimpleCommandMetadataCache.Add(commandType, metadata);
                }
                return SimpleCommandMetadataCache[commandType];
            }
        }

        private static void ExtractCommandDisplayInformation(Type commandType, CommandMetadataTemplate metadata)
        {
            var displayAttribute = commandType.GetCustomAttributes(typeof (CommandDisplayAttribute), true).FirstOrDefault() as CommandDisplayAttribute;
            if (displayAttribute != null)
            {
                metadata.Description = displayAttribute.GetLocalizedDescription();
                metadata.Usage = displayAttribute.GetLocalizedUsage();
            }
        }

        private static string GetFullCommandName(Type commandType)
        {
            var fullCommandName = commandType.Name;
            if (fullCommandName.EndsWith(COMMAND_SUFFIX, StringComparison.Ordinal))
            {
                fullCommandName = fullCommandName.Substring(0, fullCommandName.Length - COMMAND_SUFFIX.Length);
            }
            return fullCommandName;
        }
    }
}