using System.Collections.Generic;
using System.Linq;
using MGR.CommandLineParser;

// ReSharper disable once CheckNamespace

namespace System.Reflection
{
    internal static class AssemblyExtensions
    {
        [Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        public static IEnumerable<Type> GetTypes(this IEnumerable<Assembly> source, Func<Type, bool> predicate)
        {
            Guard.NotNull(predicate, nameof(predicate));

            var result = new List<Type>();
            if (source == null)
            {
                return result;
            }

            foreach (var assembly in source.Where(assembly => assembly != null && !assembly.IsDynamic))
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

#pragma warning disable CC0003 // Your catch maybe include some Exception
                catch
#pragma warning restore CC0003 // Your catch maybe include some Exception
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