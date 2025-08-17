using MGR.CommandLineParser.Command;
using MGR.CommandLineParser.Extensibility;
using MGR.CommandLineParser.Tests.Commands;
using MGR.CommandLineParser.UnitTests;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace MGR.CommandLineParser.IntegrationTests;

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

    protected async Task<ParsingResult> CallParse(IEnumerable<string> args)
    {
        var parsingResult = await CallParse(CreateParserOptions(), args);
        return parsingResult;
    }
    protected async Task<ParsingResult> CallParse(ParserOptions parserOptions, IEnumerable<string> args)
    {
        var parserBuilder = new ParserBuilder(parserOptions, _serviceCollection);
        parserBuilder.AddCommands(builder => builder.AddCommands<DeleteCommand, DeleteCommand.DeleteCommandData>());
        var parser = parserBuilder.BuildParser();
        var parsingResult = await parser.Parse(args);

        return parsingResult;
    }
    protected async Task<ParsingResult> CallParse<TCommandHandler, TCommandData>(IEnumerable<string> args)
        where TCommandHandler : class, ICommandHandler<TCommandData>
        where TCommandData : CommandData, new()
    {
        var parsingResult = await CallParse<TCommandHandler, TCommandData>(CreateParserOptions(), args);

        return parsingResult;
    }

    protected async Task<ParsingResult> CallParse<TCommandHandler, TCommandData>(ParserOptions parserOptions, IEnumerable<string> args)
        where TCommandHandler : class, ICommandHandler<TCommandData>
        where TCommandData : CommandData, new()
    {
        var parserBuilder = new ParserBuilder(parserOptions, _serviceCollection);
        parserBuilder.AddCommands(builder => builder.AddCommands<DeleteCommand, DeleteCommand.DeleteCommandData>());
        var parser = parserBuilder.BuildParser();
        var parsingResult = await parser.Parse<TCommandHandler, TCommandData>(args);

        return parsingResult;
    }
    protected async Task<ParsingResult> CallParseWithDefaultCommand<TCommandHandler, TCommandData>(IEnumerable<string> args)
        where TCommandHandler : class, ICommandHandler<TCommandData>
        where TCommandData : CommandData, new()
    {
        var parsingResult = await CallParseWithDefaultCommand<TCommandHandler, TCommandData>(CreateParserOptions(), args);

        return parsingResult;
    }

    protected async Task<ParsingResult> CallParseWithDefaultCommand<TCommandHandler, TCommandData>(ParserOptions parserOptions, IEnumerable<string> args)
        where TCommandHandler : class, ICommandHandler<TCommandData>
        where TCommandData : CommandData, new()
    {
        var parserBuilder = new ParserBuilder(parserOptions, _serviceCollection);
        parserBuilder.AddCommands(builder => builder.AddCommands<TCommandHandler, TCommandData>());
        var parser = parserBuilder.BuildParser();
        var parsingResult = await parser.ParseWithDefaultCommand<TCommandHandler, TCommandData>(args);

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