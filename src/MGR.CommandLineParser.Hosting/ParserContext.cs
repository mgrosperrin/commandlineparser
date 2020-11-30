using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MGR.CommandLineParser.Hosting
{
    internal class ParserContext
    {
        internal IEnumerable<string> Arguments { get; set; }
        internal int ParsingAndExecutionResult { get; set; }
        public Func<IParser, IEnumerable<string>, Task<ParsingResult>> ParseArguments { get; set; }
    }
}