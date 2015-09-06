using System;
using System.Collections.Generic;
using System.IO;

namespace MGR.CommandLineParser
{
    public abstract class AssemblyFileProviderBase : IAssemblyFileProvider
    {
        protected abstract SearchOption SearchOption { get; }

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