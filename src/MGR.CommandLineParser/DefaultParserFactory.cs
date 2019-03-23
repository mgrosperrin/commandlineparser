using Microsoft.Extensions.Options;

namespace MGR.CommandLineParser
{
    internal sealed class DefaultParserFactory : IParserFactory
    {
        private readonly IOptionsMonitor<ParserOptions> _optionsMonitor;

        public DefaultParserFactory(IOptionsMonitor<ParserOptions> optionsMonitor)
        {
            _optionsMonitor = optionsMonitor;
        }

        public IParser CreateParser()
        {
            var options = _optionsMonitor.CurrentValue;
            var parser = new Parser(options);
            return parser;
        }
    }
}