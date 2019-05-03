using System.Globalization;
using System.Reflection;
using MGR.CommandLineParser.Properties;

namespace MGR.CommandLineParser
{
    /// <summary>
    /// Represents a builder for the <see cref="ParserOptions"/>.
    /// </summary>
    public class ParserOptionsBuilder
    {
        internal static ParserOptionsBuilder Default => new ParserOptionsBuilder();

        internal ParserOptionsBuilder()
        {
            var entryAssembly = Assembly.GetEntryAssembly();
            if (entryAssembly != null)
            {
                ForAssembly(entryAssembly);
            }
        }
        /// <summary>
        /// Initializes the <seealso cref="Logo"/> and the <seealso cref="CommandLineName"/> based on an assembly.
        /// </summary>
        /// <param name="entryAssembly">The <see cref="Assembly"/> from which to extract the <seealso cref="Logo"/> and the <seealso cref="CommandLineName"/>.</param>
        public void ForAssembly(Assembly entryAssembly)
        {
            Guard.NotNull(entryAssembly, nameof(entryAssembly));

            var entryAssemblyName = entryAssembly.GetName();
            CommandLineName = entryAssemblyName.Name;
            Logo = string.Format(CultureInfo.CurrentUICulture, Strings.ParserOptions_LogoFormat, entryAssemblyName.Name, entryAssemblyName.Version);
        }

        /// <summary>
        /// Gets ir sets the logo of the parser.
        /// </summary>
        public string Logo { get; set; }
        /// <summary>
        /// Gets or sets the executable name.
        /// </summary>
        public string CommandLineName { get; set; }

        internal ParserOptions ToParserOptions()
        {
            var parserOptions = new ParserOptions
            {
                CommandLineName = CommandLineName,
                Logo = Logo ?? string.Empty
            };

            return parserOptions;
        }
    }
}