using MGR.CommandLineParser.Command;
using MGR.CommandLineParser.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace MGR.CommandLineParser;

internal sealed class Parser : IParser
{
    private readonly ParserOptions _parserOptions;
    private readonly IServiceProvider _serviceProvider;

    internal Parser(ParserOptions parserOptions, IServiceProvider serviceProvider)
    {
        _parserOptions = parserOptions;
        _serviceProvider = serviceProvider;
    }

    public string Logo => _parserOptions.Logo;

    public string CommandLineName => _parserOptions.CommandLineName;

    public async Task<ParsingResult> Parse<TCommandHandler, TCommandData>(IEnumerable<string> arguments) where TCommandHandler : class, ICommandHandler<TCommandData>
        where TCommandData : CommandData, new() => await ParseArguments(arguments, (parserEngine, argumentsEnumerator) =>
               parserEngine.Parse<TCommandHandler, TCommandData>(argumentsEnumerator));

    public async Task<ParsingResult> Parse(IEnumerable<string> arguments) => await ParseArguments(arguments, (parserEngine, argumentsEnumerator) =>
            parserEngine.Parse(argumentsEnumerator));

    public async Task<ParsingResult> ParseWithDefaultCommand<TCommandHandler, TCommandData>(IEnumerable<string> arguments) where TCommandHandler : class, ICommandHandler<TCommandData>
        where TCommandData : CommandData, new() => await ParseArguments(arguments, (parserEngine, argumentsEnumerator) =>
                parserEngine.ParseWithDefaultCommand<TCommandHandler, TCommandData>(argumentsEnumerator));

    private async Task<ParsingResult> ParseArguments(IEnumerable<string> arguments, Func<ParserEngine, IEnumerator<string>, Task<ParsingResult>> callParse)
    {
        if (arguments == null)
        {
            return new ParsingResult(null, null, CommandParsingResultCode.NoArgumentsProvided);
        }

        var loggerFactory = _serviceProvider.GetService<ILoggerFactory>() ?? NullLoggerFactory.Instance;
        var logger = loggerFactory.CreateLogger<LoggerCategory.Parser>();
        using (logger.BeginParsingArguments(Guid.NewGuid().ToString()))
        {
            logger.CreationOfParserEngine();
            var parserEngine = new ParserEngine(_serviceProvider, loggerFactory);
            var argumentsEnumerator = arguments.GetArgumentsEnumerator();

            var result = await callParse(parserEngine, argumentsEnumerator);
            return result;
        }
    }
}