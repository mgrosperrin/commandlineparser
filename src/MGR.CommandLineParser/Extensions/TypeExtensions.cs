using System.Collections.Generic;
using System.Linq;
using MGR.CommandLineParser;

// ReSharper disable CheckNamespace

namespace System
// ReSharper restore CheckNamespace
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
    }
}
