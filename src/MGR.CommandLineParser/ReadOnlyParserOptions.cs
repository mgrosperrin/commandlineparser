using System;

namespace MGR.CommandLineParser
{
    internal sealed class ReadOnlyParserOptions : IParserOptions
    {
        internal ReadOnlyParserOptions(IParserOptions options)
        {
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }
            Logo = options.Logo;
            CommandLineName = options.CommandLineName;
        }

        public string Logo { get; }

        public string CommandLineName { get; }
    }
}