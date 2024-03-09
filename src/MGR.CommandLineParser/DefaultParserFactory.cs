using Microsoft.Extensions.Options;

namespace MGR.CommandLineParser;

internal sealed class DefaultParserFactory : IParserFactory
{
    private readonly IOptions<ParserOptions> _optionsMonitor;
    private readonly IServiceProvider _serviceProvider;

    public DefaultParserFactory(IOptions<ParserOptions> optionsMonitor, IServiceProvider serviceProvider)
    {
        _optionsMonitor = optionsMonitor;
        _serviceProvider = serviceProvider;
    }

    public IParser CreateParser()
    {
        var options = _optionsMonitor.Value;
        var parser = new Parser(options, _serviceProvider);
        return parser;
    }
}