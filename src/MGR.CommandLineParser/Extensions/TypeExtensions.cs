using System.Collections.Generic;
using System.Linq;

// ReSharper disable CheckNamespace

namespace System
// ReSharper restore CheckNamespace
{
    internal static class TypeExtensions
    {
        internal static bool IsCollectionType(this Type type)
        {
            if (type == null)
            {
                throw new ArgumentNullException("type");
            }
            return type.GetCollectionType() != null && !type.IsDictionaryType();
        }

        internal static Type GetCollectionType(this Type type)
        {
            if (type == null)
            {
                throw new ArgumentNullException("type");
            }
            return GetInterfaceType(type, typeof (ICollection<>));
        }

        public static Type GetUnderlyingGenericType(this Type type, int index = 0)
        {
            if (type == null)
            {
                throw new ArgumentNullException("type");
            }
            if (!type.IsGenericType)
            {
                return null;
            }
            return type.GetGenericArguments()[index];
        }

        public static Type GetUnderlyingCollectionType(this Type type, int index = 0)
        {
            if (type == null)
            {
                throw new ArgumentNullException("type");
            }
            Type collectionType = type.GetCollectionType();
            if (collectionType == null)
            {
                return null;
            }
            return collectionType.GetUnderlyingGenericType(index);
        }

        public static Type GetUnderlyingDictionaryType(this Type type, bool key)
        {
            if (type == null)
            {
                throw new ArgumentNullException("type");
            }
            Type collectionType = type.GetDictionaryType();
            if (collectionType == null)
            {
                return null;
            }
            return collectionType.GetUnderlyingGenericType(key ? 0 : 1);
        }

        public static bool IsMultiValuedType(this Type type)
        {
            if (type == null)
            {
                throw new ArgumentNullException("type");
            }
            return type.IsCollectionType() || type.IsDictionaryType();
        }

        public static bool IsDictionaryType(this Type type)
        {
            if (type == null)
            {
                throw new ArgumentNullException("type");
            }
            return type.GetDictionaryType() != null;
        }

        public static Type GetDictionaryType(this Type type)
        {
            if (type == null)
            {
                throw new ArgumentNullException("type");
            }
            return type.GetInterfaceType(typeof (IDictionary<,>));
        }

        private static Type GetInterfaceType(this Type type, Type interfaceType)
        {
            if (type.IsGenericType && type.GetGenericTypeDefinition() == interfaceType)
            {
                return type;
            }
            return (from t in type.GetInterfaces()
                    where t.IsGenericType && t.GetGenericTypeDefinition() == interfaceType
                    select t).SingleOrDefault();
        }
    }
}
