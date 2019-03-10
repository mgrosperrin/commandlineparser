using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MGR.CommandLineParser.Command;
using MGR.CommandLineParser.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace MGR.CommandLineParser
{
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

        public async Task<ParsingResult> Parse<TCommand>(IEnumerable<string> args) where TCommand : class, ICommand => await ParseArguments(args, (parserEngine, arguments) =>
                   parserEngine.Parse<TCommand>(arguments));

        public async Task<ParsingResult> Parse(IEnumerable<string> args) => await ParseArguments(args, (parserEngine, arguments) =>
                parserEngine.Parse(arguments));

        public async Task<ParsingResult> ParseWithDefaultCommand<TCommand>(IEnumerable<string> args) where TCommand : class, ICommand => await ParseArguments(args, (parserEngine, arguments) =>
                    parserEngine.ParseWithDefaultCommand<TCommand>(arguments));

        private async Task<ParsingResult> ParseArguments(IEnumerable<string> args, Func<ParserEngine, Arguments, Task<ParsingResult>> callParse)
        {
            if (args == null)
            {
                return new ParsingResult(null, null, CommandParsingResultCode.NoArgumentsProvided);
            }

            var arguments = new Arguments(args);
            var loggerFactory = _serviceProvider.GetService<ILoggerFactory>() ?? NullLoggerFactory.Instance;
            var logger = loggerFactory.CreateLogger<LoggerCategory.Parser>();
            using (logger.BeginParsingArguments(Guid.NewGuid().ToString()))
            {
                logger.CreationOfParserEngine();
                var parserEngine = new ParserEngine(_serviceProvider, loggerFactory);

                var result = await callParse(parserEngine, arguments);
                return result;
            }
        }
    }
}