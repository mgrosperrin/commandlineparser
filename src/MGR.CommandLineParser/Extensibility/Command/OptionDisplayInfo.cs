using System.Diagnostics.CodeAnalysis;

namespace MGR.CommandLineParser.Extensibility.Command
{
    /// <summary>
    ///     Represents the display information of an option.
    /// </summary>
    public sealed class OptionDisplayInfo
    {
        /// <summary>
        ///     Creates a new <see cref="OptionDisplayInfo" />.
        /// </summary>
        public OptionDisplayInfo()
        {
            AlternateNames = new string[] { };
        }

        /// <summary>
        ///     Gets the name of the option.
        /// </summary>
        public string Name { get; internal set; }

        /// <summary>
        ///     Gets the alternates names of the option.
        /// </summary>
        [SuppressMessage("Microsoft.Performance", "CA1819:PropertiesShouldNotReturnArrays")]
        public string[] AlternateNames { get; internal set; }

        /// <summary>
        ///     Gets the shortname of the option.
        /// </summary>
        public string ShortName { get; internal set; }

        /// <summary>
        ///     Gets the description of the option.
        /// </summary>
        public string Description { get; internal set; }
    }
}