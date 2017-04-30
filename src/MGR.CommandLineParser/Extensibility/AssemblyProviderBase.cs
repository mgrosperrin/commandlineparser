using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Reflection;

namespace MGR.CommandLineParser.Extensibility
{
    /// <summary>
    /// Base class for providing all files (*.dll and *.exe) in the current folder (recursive or not).
    /// </summary>
    public abstract class AssemblyProviderBase : IAssemblyProvider
    {
        /// <summary>
        /// Gets the recursivity options for browsing the current folder.
        /// </summary>
        protected abstract SearchOption SearchOption { get; }

        /// <inheritdoc />
        private IEnumerable<string> GetFilesToLoad()
        {
            var directory = Path.GetDirectoryName(typeof (AssemblyProviderBase).Assembly.CodeBase);
            if (!string.IsNullOrEmpty(directory))
            {
                var thisDirectory = new Uri(directory).AbsolutePath;
                foreach (var item in Directory.EnumerateFiles(thisDirectory, "*.exe", SearchOption))
                {
                    yield return new FileInfo(item).FullName;
                }
                foreach (var item in Directory.EnumerateFiles(thisDirectory, "*.dll", SearchOption))
                {
                    yield return new FileInfo(item).FullName;
                }
            }
        }

        /// <inheritdoc />
        [SuppressMessage("Microsoft.Reliability", "CA2001:AvoidCallingProblematicMethods",
            MessageId = "System.Reflection.Assembly.LoadFrom")]
        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        public IEnumerable<Assembly> GetAssembliesToBrowse()
        {
            foreach (var assemblyFile in GetFilesToLoad())
            {
                try
                {
                    Assembly.LoadFrom(assemblyFile);
                }

#pragma warning disable CC0004 // Catch block cannot be empty
                // ReSharper disable once EmptyGeneralCatchClause
                catch
#pragma warning restore CC0004 // Catch block cannot be empty
                {
                }
            }

            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            return assemblies;
        }
    }
}