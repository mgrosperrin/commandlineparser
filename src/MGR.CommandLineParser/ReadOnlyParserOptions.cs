using System;
using System.Collections.Generic;
using System.Linq;
using MGR.CommandLineParser.Converters;

namespace MGR.CommandLineParser
{
    internal sealed class ReadOnlyParserOptions : IParserOptions
    {
        public IConsole Console { get; private set; }

        public ICommandProvider CommandProvider { get; private set; }

        public string Logo { get; private set; }

        public string CommandLineName { get; private set; }

        public IEnumerable<IConverter> Converters { get; private set; }

        internal ReadOnlyParserOptions(IParserOptions options)
        {
            if (options == null)
            {
                throw new ArgumentNullException("options");
            }
            Console = options.Console;
            CommandProvider = options.CommandProvider;
            Logo = options.Logo;
            CommandLineName = options.CommandLineName;
            Converters = options.Converters.ToList();
        }
    }
}