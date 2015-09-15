using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using MGR.CommandLineParser;
using MGR.CommandLineParser.Command;

// ReSharper disable CheckNamespace

namespace System
// ReSharper restore CheckNamespace
{
    internal static class TypeExtensions
    {
        private static readonly Dictionary<Type, CommandMetadataTemplate> CommandMetadataCache = new Dictionary<Type, CommandMetadataTemplate>();
        private static readonly Dictionary<Type, CommandMetadataTemplate> SimpleCommandMetadataCache = new Dictionary<Type, CommandMetadataTemplate>();
        private static readonly object CommandMetadataCacheLockObject = new object();
        private static readonly object SimpleCommandMetadataCacheLockObject = new object();

        internal static bool IsCollectionType(this Type source)
        {
            Guard.NotNull(source, nameof(source));

            return source.GetCollectionType() != null && !source.IsDictionaryType();
        }

        internal static Type GetCollectionType(this Type source)
        {
            Guard.NotNull(source, nameof(source));

            return GetInterfaceType(source, typeof (ICollection<>));
        }

        public static Type GetUnderlyingGenericType(this Type source, int index = 0)
        {
            Guard.NotNull(source, nameof(source));

            if (!source.IsGenericType)
            {
                return null;
            }
            return source.GetGenericArguments()[index];
        }

        public static Type GetUnderlyingCollectionType(this Type source, int index = 0)
        {
            Guard.NotNull(source, nameof(source));

            var collectionType = source.GetCollectionType();
            if (collectionType == null)
            {
                return null;
            }
            return collectionType.GetUnderlyingGenericType(index);
        }

        public static Type GetUnderlyingDictionaryType(this Type source, bool key)
        {
            Guard.NotNull(source, nameof(source));

            var collectionType = source.GetDictionaryType();
            if (collectionType == null)
            {
                return null;
            }
            return collectionType.GetUnderlyingGenericType(key ? 0 : 1);
        }

        public static bool IsMultiValuedType(this Type source)
        {
            Guard.NotNull(source, nameof(source));

            return source.IsCollectionType() || source.IsDictionaryType();
        }

        public static bool IsDictionaryType(this Type source)
        {
            Guard.NotNull(source, nameof(source));

            return source.GetDictionaryType() != null;
        }

        public static Type GetDictionaryType(this Type source)
        {
            Guard.NotNull(source, nameof(source));

            return source.GetInterfaceType(typeof (IDictionary<,>));
        }

        private static Type GetInterfaceType(this Type source, Type interfaceType)
        {
            if (source.IsGenericType && source.GetGenericTypeDefinition() == interfaceType)
            {
                return source;
            }
            return (from t in source.GetInterfaces()
                    where t.IsGenericType && t.GetGenericTypeDefinition() == interfaceType
                    select t).SingleOrDefault();
        }
        public static bool IsType<T>(this Type source)
        {
            Guard.NotNull(source, nameof(source));

            return source.IsClass &&
                source.IsVisible &&
                !source.IsAbstract &&
                typeof(T).IsAssignableFrom(source);
        }

        internal static CommandMetadataTemplate ExtractCommandMetadataTemplate(this Type source)
        {
            Guard.NotNull(source, nameof(source));
            Guard.OfType<ICommand>(source, nameof(source));

            lock (SimpleCommandMetadataCacheLockObject)
            {
                var commandType = source;
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
            var displayAttribute = commandType.GetCustomAttributes(typeof(CommandDisplayAttribute), true).FirstOrDefault() as CommandDisplayAttribute;
            if (displayAttribute != null)
            {
                metadata.Description = displayAttribute.GetLocalizedDescription();
                metadata.Usage = displayAttribute.GetLocalizedUsage();
            }
        }
        private static string GetFullCommandName(Type commandType)
        {
            var fullCommandName = commandType.Name;
            if (fullCommandName.EndsWith(Constants.CommandSuffix, StringComparison.Ordinal))
            {
                fullCommandName = fullCommandName.Substring(0, fullCommandName.Length - Constants.CommandSuffix.Length);
            }
            return fullCommandName;
        }
        internal static CommandMetadataTemplate ExtractMetadataTemplate(this Type source)
        {
            Guard.NotNull(source, nameof(source));
            Guard.OfType<ICommand>(source, nameof(source));

            var commandType = source;
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

    }
}
