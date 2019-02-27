using System.Diagnostics.CodeAnalysis;
using System.IO;
using JetBrains.Annotations;

namespace MGR.CommandLineParser.Extensibility.ClassBased
{
    /// <summary>
    /// Implementation of <see cref="IAssemblyProvider"/> for providing all files (*.dll and *.exe) in the current folder not recursivly (this is the current default implementation).
    /// </summary>
    public sealed class CurrentDirectoryAssemblyProvider : AssemblyProviderBase
    {
        /// <summary>
        /// Gets the singleton instance of <see cref="CurrentDirectoryAssemblyProvider"/>.
        /// </summary>
        [SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes")]
        [PublicAPI, NotNull]
        public static readonly IAssemblyProvider Instance = new CurrentDirectoryAssemblyProvider();
        /// <summary>
        /// Create a new <see cref="CurrentDirectoryAssemblyProvider"/>.
        /// </summary>

        /// <inheritdoc />
        protected override SearchOption SearchOption => SearchOption.TopDirectoryOnly;
    }
}