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

        internal static string ExtractCommandName(this ICommand commandSource)
        {
            if (commandSource == null)
            {
                throw new ArgumentNullException("commandSource");
            }
            return commandSource.ExtractCommandMetadataTemplate().Name;
        }

        internal static CommandMetadataTemplate ExtractMetadataTemplate(this ICommand command, IParserOptions options)
        {
            if (command == null)
            {
                throw new ArgumentNullException("command");
            }
            Type commandType = command.GetType();
            lock (CommandMetadataCacheLockObject)
            {
                if (!CommandMetadataCache.ContainsKey(commandType))
                {
                    var metadata = ExtractCommandMetadataTemplate(command);
                    foreach (PropertyInfo propInfo in commandType.GetProperties(BindingFlags.Public | BindingFlags.Instance).Where(pi => pi.Name != "Arguments"))
                    {
                        OptionMetadataTemplate optionMetadata = propInfo.ExtractMetadata(metadata, options);
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

        internal static CommandMetadata ExtractMetadata(this ICommand command, IParserOptions options)
        {
            return ExtractMetadataTemplate(command, options).ToCommandMetadata(command);
        }

        internal static CommandMetadataTemplate ExtractCommandMetadataTemplate(this ICommand command)
        {
            if (command == null)
            {
                throw new ArgumentNullException("command");
            }
            lock (SimpleCommandMetadataCacheLockObject)
            {
                if (!SimpleCommandMetadataCache.ContainsKey(command.GetType()))
                {
                    var metadata = new CommandMetadataTemplate();
                    Type commandType = command.GetType();
                    string fullCommandName = commandType.Name;
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
                    SimpleCommandMetadataCache.Add(command.GetType(), metadata);
                }
                return SimpleCommandMetadataCache[command.GetType()];
            }
        }
    }
}