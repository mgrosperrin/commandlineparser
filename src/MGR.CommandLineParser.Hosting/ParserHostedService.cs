using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;

namespace MGR.CommandLineParser.Hosting
{
    internal sealed class ParserHostedService : IHostedService
    {
        private readonly IParser _parser;
        private readonly IServiceProvider _serviceProvider;
        private readonly ParserContext _parserContext;
        private readonly IApplicationLifetime _applicationLifetime;

        public ParserHostedService(IParser parser, IServiceProvider serviceProvider, ParserContext parserContext, IApplicationLifetime applicationLifetime)
        {
            _parser = parser;
            _serviceProvider = serviceProvider;
            _parserContext = parserContext;
            _applicationLifetime = applicationLifetime;
        }
        public async Task StartAsync(CancellationToken cancellationToken)
        {
            var parsingAndExecutionResult = await ParseAndExecuteAsync();
            _parserContext.ParsingAndExecutionResult = parsingAndExecutionResult;
            _applicationLifetime.StopApplication();
        }

        private async Task<int> ParseAndExecuteAsync()
        {
            var parsingResult = _parser.Parse(_parserContext.Arguments, _serviceProvider);
            if (parsingResult.IsValid)
            {
                return await parsingResult.ExecuteAsync();
            }
            return (int)parsingResult.ParsingResultCode;
        }

        public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
    }
}
