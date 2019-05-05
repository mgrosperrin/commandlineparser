using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.DependencyModel;

namespace MGR.CommandLineParser.Extensibility.ClassBased
{
    /// <summary>
    /// Base class for providing all files (*.dll and *.exe) in the current folder (recursive or not).
    /// </summary>
    public abstract class AssemblyProviderBase : IAssemblyProvider
    {
        /// <summary>
        /// Gets the recursively options for browsing the current folder.
        /// </summary>
        protected abstract SearchOption SearchOption { get; }

        private IEnumerable<string> GetFilesToLoad()
        {
            var thisDirectory = Environment.CurrentDirectory;
            foreach (var item in Directory.EnumerateFiles(thisDirectory, "*.exe", SearchOption))
            {
                yield return new FileInfo(item).FullName;
            }
            foreach (var item in Directory.EnumerateFiles(thisDirectory, "*.dll", SearchOption))
            {
                yield return new FileInfo(item).FullName;
            }
        }

        /// <inheritdoc />
        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        public IEnumerable<Assembly> GetAssembliesToBrowse()
        {
            var alreadyLoadablesLivraries = DependencyContext.Default.RuntimeLibraries;
            foreach (var assemblyFile in GetFilesToLoad())
            {
                var assemblyNameFromFile = Path.GetFileNameWithoutExtension(assemblyFile);
                if (alreadyLoadablesLivraries.Any(runtimeLibrary => runtimeLibrary.Name.Equals(assemblyNameFromFile, StringComparison.OrdinalIgnoreCase)))
                {
                    continue;
                }

                try
                {
                    Assembly.LoadFile(assemblyFile);
                }

                // ReSharper disable once EmptyGeneralCatchClause
#pragma warning disable CC0004 // Catch block cannot be empty
#pragma warning disable S2486 // Generic exceptions should not be ignored
                catch
#pragma warning disable S108 // Nested blocks of code should not be left empty
                {
                }
#pragma warning restore S108 // Nested blocks of code should not be left empty
#pragma warning restore CC0004 // Catch block cannot be empty
#pragma warning restore S2486 // Generic exceptions should not be ignored
            }

            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            return assemblies;
        }
    }
}