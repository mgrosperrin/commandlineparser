using System.Globalization;
using System.Reflection;
using MGR.CommandLineParser.Properties;

namespace MGR.CommandLineParser
{
    internal class ParserBuilderOptions
    {
        internal static ParserBuilderOptions Default => new ParserBuilderOptions();

        private ParserBuilderOptions()
        {
            var entryAssembly = Assembly.GetEntryAssembly();
            if (entryAssembly != null)
            {
                var entryAssemblyName = entryAssembly.GetName();
                CommandLineName = entryAssemblyName.Name;
                Logo = string.Format(CultureInfo.CurrentUICulture, Strings.ParserOptions_LogoFormat, entryAssemblyName.Name, entryAssemblyName.Version);
            }
        }

        public string Logo { get; set; }
        public string CommandLineName { get; set; }

        public IParserOptions ToParserOptions()
        {
            var parserOptions = new ParserOptions
            {
                CommandLineName = CommandLineName,
                Logo = Logo
            };

            return parserOptions;
        }
    }
}