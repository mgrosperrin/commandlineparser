using System.Collections.Generic;
using MGR.CommandLineParser.Command;
using MGR.CommandLineParser.Extensibility;
using MGR.CommandLineParser.Tests.Commands;
using MGR.CommandLineParser.UnitTests;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace MGR.CommandLineParser.IntegrationTests
{
    public abstract class ConsoleLoggingTestsBase
    {
        private readonly IServiceCollection _serviceCollection;
        protected FakeConsole Console;
        protected ConsoleLoggingTestsBase()
        {
            _serviceCollection = new ServiceCollection();
            _serviceCollection.AddScoped<IConsole>(_ =>
           {
               Console = new FakeConsole();
               return Console;
           });
        }

        private static ParserOptions CreateParserOptions()
        {
            return new ParserOptions {
                Logo = "Integration Tests",
                CommandLineName = "test.exe"
            };
        }

        protected ParsingResult CallParse(IEnumerable<string> args)
        {
            var parsingResult = CallParse(CreateParserOptions(), args);
            return parsingResult;
        }
        protected ParsingResult CallParse(ParserOptions parserOptions, IEnumerable<string> args)
        {
            var parserBuilder = new ParserBuilder(parserOptions, _serviceCollection);
            parserBuilder.AddCommands(builder => builder.AddCommands<DeleteCommand>());
            var parser = parserBuilder.BuildParser();
            var parsingResult = parser.Parse(args);

            return parsingResult;
        }
        protected ParsingResult CallParse<TCommand>(IEnumerable<string> args)
        where TCommand : class, ICommand
        {
            var parsingResult = CallParse<TCommand>(CreateParserOptions(), args);

            return parsingResult;
        }

        protected ParsingResult CallParse<TCommand>(ParserOptions parserOptions, IEnumerable<string> args)
            where TCommand : class, ICommand
        {
            var parserBuilder = new ParserBuilder(parserOptions, _serviceCollection);
            parserBuilder.AddCommands(builder => builder.AddCommands<DeleteCommand>());
            var parser = parserBuilder.BuildParser();
            var parsingResult = parser.Parse<TCommand>(args);

            return parsingResult;
        }
        protected ParsingResult CallParseWithDefaultCommand<TCommand>(IEnumerable<string> args)
            where TCommand : class, ICommand
        {
            var parsingResult = CallParseWithDefaultCommand<TCommand>(CreateParserOptions(), args);

            return parsingResult;
        }

        protected ParsingResult CallParseWithDefaultCommand<TCommand>(ParserOptions parserOptions, IEnumerable<string> args)
            where TCommand : class, ICommand
        {
            var parserBuilder = new ParserBuilder(parserOptions, _serviceCollection);
            parserBuilder.AddCommands(builder => builder.AddCommands<DeleteCommand>());
            var parser = parserBuilder.BuildParser();
            var parsingResult = parser.ParseWithDefaultCommand<TCommand>(args);

            return parsingResult;
        }

        protected void AssertNoMessage()
        {
            var messages = Console.Messages;
            Assert.Empty(messages);
        }

        protected void AssertOneMessageLoggedToConsole<TMessage>(string expectedMessage)
                where TMessage : FakeConsole.Message
        {
            var messages = Console.Messages;
            Assert.Single(messages);
            Assert.IsType<TMessage>(messages[0]);
            Assert.Equal(expectedMessage, messages[0].ToString(), ignoreLineEndingDifferences: true);
        }
    }
}