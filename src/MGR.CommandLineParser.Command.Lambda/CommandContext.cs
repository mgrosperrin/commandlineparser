using System;
using System.Collections.Generic;

namespace MGR.CommandLineParser.Command.Lambda
{
    public class CommandContext
    {
        public IEnumerable<string> Arguments { get; set; }
        public T GetOption<T>(string name)
        {
            throw new NotImplementedException();
        }

        public IServiceProvider ServiceProvider { get; private set; }
        public IParserOptions ParserOptions { get; private set; }
    }
}