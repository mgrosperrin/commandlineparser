using System;
using System.Collections.Generic;
using System.IO;

namespace MGR.CommandLineParser
{
    /// <summary>
    /// Base class for providing all files (*.dll and *.exe) in the current folder (recursive or not).
    /// </summary>
    public abstract class AssemblyFileProviderBase : IAssemblyFileProvider
    {
        /// <summary>
        /// Gets the recursivity options for browsing the current folder.
        /// </summary>
        protected abstract SearchOption SearchOption { get; }

        /// <inheritdoc />
        public IEnumerable<string> GetFilesToLoad()
        {
            var directory = Path.GetDirectoryName(typeof (AssemblyFileProviderBase).Assembly.CodeBase);
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
    }
}