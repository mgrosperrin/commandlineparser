using System.Collections.Generic;
using System.Linq;
using MGR.CommandLineParser;

// ReSharper disable once CheckNamespace
namespace System
{
    internal static class TypeExtensions
    {
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

        internal static Type GetUnderlyingGenericType(this Type source, int index = 0)
        {
            Guard.NotNull(source, nameof(source));

            if (!source.IsGenericType)
            {
                return null;
            }
            return source.GetGenericArguments()[index];
        }

        internal static Type GetUnderlyingCollectionType(this Type source, int index = 0)
        {
            Guard.NotNull(source, nameof(source));

            var collectionType = source.GetCollectionType();
            if (collectionType == null)
            {
                return null;
            }
            return collectionType.GetUnderlyingGenericType(index);
        }

        internal static Type GetUnderlyingDictionaryType(this Type source, bool key)
        {
            Guard.NotNull(source, nameof(source));

            var collectionType = source.GetDictionaryType();
            if (collectionType == null)
            {
                return null;
            }
            return collectionType.GetUnderlyingGenericType(key ? 0 : 1);
        }

        internal static bool IsMultiValuedType(this Type source)
        {
            Guard.NotNull(source, nameof(source));

            return source.IsCollectionType() || source.IsDictionaryType();
        }

        internal static bool IsDictionaryType(this Type source)
        {
            Guard.NotNull(source, nameof(source));

            return source.GetDictionaryType() != null;
        }

        internal static Type GetDictionaryType(this Type source)
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

        internal static bool IsType<T>(this Type source)
        {
            Guard.NotNull(source, nameof(source));
            return source.IsType(typeof (T)) && source.IsClass;
        }
        internal static bool IsType(this Type source, Type baseType)
        {
            Guard.NotNull(source, nameof(source));

            if (source == baseType)
            {
                return true;
            }

            return source.IsVisible && !source.IsAbstract && baseType.IsAssignableFrom(source);
        }

        internal static string GetFullCommandName(this Type commandType)
        {
            Guard.NotNull(commandType, nameof(commandType));

            var fullCommandName = commandType.Name;
            if (fullCommandName.EndsWith(Constants.CommandSuffix, StringComparison.Ordinal))
            {
                fullCommandName = fullCommandName.Substring(0, fullCommandName.Length - Constants.CommandSuffix.Length);
            }
            return fullCommandName;
        }

        internal static TAttribute GetAttribute<TAttribute>(this Type source)
            where TAttribute : Attribute
        {
            var attribute = source.GetCustomAttributes(typeof(TAttribute), true).FirstOrDefault() as TAttribute;
            return attribute;
        }
    }
}