using Microsoft.Extensions.Hosting;

namespace MGR.CommandLineParser.Hosting;

internal sealed class ParserHostedService : IHostedService
{
    private readonly IParser _parser;
    private readonly ParserContext _parserContext;
    private readonly IHostApplicationLifetime _hostApplicationLifetime;

    public ParserHostedService(IParser parser, ParserContext parserContext, IHostApplicationLifetime hostApplicationLifetime)
    {
        _parser = parser;
        _parserContext = parserContext;
        _hostApplicationLifetime = hostApplicationLifetime;
    }
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        var parsingAndExecutionResult = await ParseAndExecuteAsync(cancellationToken);
        _parserContext.ParsingAndExecutionResult = parsingAndExecutionResult;
        _hostApplicationLifetime.StopApplication();
    }

    private async Task<int> ParseAndExecuteAsync(CancellationToken cancellationToken)
    {
        var parsingResult = await _parserContext.ParseArguments(_parser, _parserContext.Arguments);
        if (parsingResult.IsValid)
        {
            return await parsingResult.ExecuteAsync(cancellationToken);
        }
        return (int)parsingResult.ParsingResultCode;
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}
