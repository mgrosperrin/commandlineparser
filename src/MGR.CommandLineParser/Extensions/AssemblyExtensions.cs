using System.Collections.Generic;
using System.Linq;
using MGR.CommandLineParser;

// ReSharper disable once CheckNamespace

namespace System.Reflection
{
    internal static class AssemblyExtensions
    {
        public static IEnumerable<Type> GetTypes(this IEnumerable<Assembly> assemblies, Func<Type, bool> predicate)
        {
            Guard.NotNull(predicate, nameof(predicate));

            var result = new List<Type>();
            if (assemblies == null)
            {
                return result;
            }

            foreach (var assembly in assemblies.Where(assembly => assembly != null && !assembly.IsDynamic))
            {
                Type[] exportedTypes = null;

                try
                {
                    exportedTypes = assembly.GetExportedTypes();
                }
                catch (ReflectionTypeLoadException ex)
                {
                    exportedTypes = ex.Types;
                }
                catch
                {
                    // We deliberately ignore all other exceptions. 
                    continue;
                }

                if (exportedTypes != null)
                {
                    result.AddRange(exportedTypes.Where(predicate));
                }
            }
            return result;
        }
    }
}