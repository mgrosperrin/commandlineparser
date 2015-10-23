using System.Diagnostics.CodeAnalysis;
using System.IO;
using JetBrains.Annotations;

namespace MGR.CommandLineParser
{
    /// <summary>
    /// Implementation of <see cref="IAssemblyProvider"/> for providing all files (*.dll and *.exe) in the current folder recursivly.
    /// </summary>
    public sealed class RecursiveAssemblyProvider : AssemblyProviderBase
    {
        /// <summary>
        /// Gets the singleton instance of <see cref="RecursiveAssemblyProvider"/>.
        /// </summary>
        [SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes")]
        [PublicAPI, NotNull]
        public static readonly IAssemblyProvider Instance = new RecursiveAssemblyProvider();
        private RecursiveAssemblyProvider() { }
        /// <inheritdoc />
        protected override SearchOption SearchOption => SearchOption.AllDirectories;
    }
}