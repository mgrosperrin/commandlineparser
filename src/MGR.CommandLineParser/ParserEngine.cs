using MGR.CommandLineParser.Command;
using MGR.CommandLineParser.Diagnostics;
using MGR.CommandLineParser.Extensibility;
using MGR.CommandLineParser.Extensibility.Command;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace MGR.CommandLineParser;

internal class ParserEngine
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<LoggerCategory.Parser> _logger;

    internal ParserEngine(IServiceProvider serviceProvider, ILoggerFactory loggerFactory)
    {
        _serviceProvider = serviceProvider;
        _logger = loggerFactory.CreateLogger<LoggerCategory.Parser>();
    }

    internal async Task<ParsingResult> Parse<TCommandHandler, TCommandData>(IEnumerator<string> argumentsEnumerator) where TCommandHandler : class, ICommandHandler<TCommandData>
        where TCommandData : CommandData, new()
    {
        using (_logger.BeginParsingForSpecificCommandType(typeof(TCommandHandler)))
        {
            var commandTypeProviders = _serviceProvider.GetServices<ICommandTypeProvider>();
            var commandType = await commandTypeProviders.GetCommandType<TCommandHandler>();
            var parsingResult = ParseImpl(argumentsEnumerator, commandType);
            if (parsingResult.ParsingResultCode == CommandParsingResultCode.NoCommandFound)
            {
                _logger.NoCommandFoundAfterSpecificParsing();
            }
            else
            {
                _logger.CommandFoundAfterSpecificParsing(typeof(TCommandHandler), parsingResult.ParsingResultCode,
                    parsingResult.ValidationResults);
            }

            return parsingResult;
        }
    }

    internal async Task<ParsingResult> ParseWithDefaultCommand<TCommandHandler, TCommandData>(IEnumerator<string> argumentsEnumerator)
        where TCommandHandler : class, ICommandHandler<TCommandData>
        where TCommandData : CommandData, new()
    {
        using (_logger.BeginParsingWithDefaultCommandType(typeof(TCommandHandler)))
        {
            var commandName = argumentsEnumerator.GetNextCommandLineItem();
            if (commandName != null)
            {
                _logger.ArgumentProvidedWithDefaultCommandType(commandName);
                var commandTypeProviders = _serviceProvider.GetServices<ICommandTypeProvider>();
                var commandType = await commandTypeProviders.GetCommandType(commandName);
                if (commandType != null)
                {
                    _logger.CommandTypeFoundWithDefaultCommandType(commandName);
                    return ParseImpl(argumentsEnumerator, commandType);
                }

                _logger.NoCommandTypeFoundWithDefaultCommandType(commandName, typeof(TCommandHandler));
                var withArgumentsCommandResult = await Parse<TCommandHandler, TCommandData>(argumentsEnumerator.PrefixWith(commandName));
                return withArgumentsCommandResult;

            }

            _logger.NoArgumentProvidedWithDefaultCommandType(typeof(TCommandHandler));
            var noArgumentsCommandResult = await Parse<TCommandHandler, TCommandData>(argumentsEnumerator);
            return noArgumentsCommandResult;
        }
    }

    internal async Task<ParsingResult> Parse(IEnumerator<string> argumentsEnumerator)
    {
        _logger.ParseForNotAlreadyKnownCommand();
        var commandName = argumentsEnumerator.GetNextCommandLineItem();
        if (commandName == null)
        {
            _logger.NoCommandNameForNotAlreadyKnownCommand();
            var helpWriter = _serviceProvider.GetRequiredService<IHelpWriter>();
            await helpWriter.WriteCommandListing();
            return new ParsingResult(null, null, CommandParsingResultCode.NoCommandNameProvided);
        }

        using (_logger.BeginParsingUsingCommandName(commandName))
        {
            var commandTypeProviders = _serviceProvider.GetServices<ICommandTypeProvider>();
            var commandType = await commandTypeProviders.GetCommandType(commandName);
            if (commandType == null)
            {
                _logger.NoCommandTypeFoundForNotAlreadyKnownCommand(commandName);
                var helpWriter = _serviceProvider.GetRequiredService<IHelpWriter>();
                await helpWriter.WriteCommandListing();
                return new ParsingResult(null, null, CommandParsingResultCode.NoCommandFound);
            }

            _logger.CommandTypeFoundForNotAlreadyKnownCommand(commandName);
            return ParseImpl(argumentsEnumerator, commandType);
        }

    }

    private ParsingResult ParseImpl(IEnumerator<string> argumentsEnumerator, ICommandType commandType)
    {
        var commandObjectBuilder = ExtractCommandLineOptions(commandType, argumentsEnumerator);
        if (commandObjectBuilder == null)
        {
            return new ParsingResult(null, null, CommandParsingResultCode.CommandParametersNotValid);
        }
        var validation = commandObjectBuilder.Validate(_serviceProvider);
        if (!validation.IsValid)
        {
            _logger.ParsedCommandIsNotValid();
            var helpWriter = _serviceProvider.GetRequiredService<IHelpWriter>();
            helpWriter.WriteHelpForCommand(commandType);
            return new ParsingResult(commandObjectBuilder.GenerateCommandObject(), validation.ValidationErrors, CommandParsingResultCode.CommandParametersNotValid);
        }
        return new ParsingResult(commandObjectBuilder.GenerateCommandObject(), null, CommandParsingResultCode.Success);
    }
    private ICommandObjectBuilder? ExtractCommandLineOptions(ICommandType commandType, IEnumerator<string> argumentsEnumerator)
    {
        var commandObjectBuilder = commandType.CreateCommandObjectBuilder(_serviceProvider);
        if (commandObjectBuilder == null)
        {
            return null;
        }
        var alwaysPutInArgumentList = false;
        while (true)
        {
            var argument = argumentsEnumerator.GetNextCommandLineItem();
            if (argument == null)
            {
                break;
            }
            if (argument.Equals(Constants.EndOfOptions))
            {
                alwaysPutInArgumentList = true;
                continue;
            }

            if (alwaysPutInArgumentList || !argument.StartsWith(StringComparison.OrdinalIgnoreCase, Constants.OptionStarter))
            {
                commandObjectBuilder.AddArguments(argument);
                continue;
            }

            var starterLength = 2;
            Func<ICommandObjectBuilder, string, ICommandOption?> commandOptionFinder = (co, optionName) => co.FindOption(optionName);
            if (!argument.StartsWith(Constants.LongNameOptionStarter))
            {
                starterLength = Constants.ShortNameOptionStarter.Length;
                commandOptionFinder = (co, optionName) => co.FindOptionByShortName(optionName);
            }
            var optionText = argument.Substring(starterLength);
            var value = string.Empty;
            var splitIndex = optionText.IndexOf(Constants.OptionSplitter);
            if (splitIndex > 0)
            {
                value = optionText.Substring(splitIndex + 1);
                optionText = optionText.Substring(0, splitIndex);
            }

            var option = commandOptionFinder(commandObjectBuilder, optionText);
            if (option == null)
            {
                var console = _serviceProvider.GetRequiredService<IConsole>();
                console.WriteLineError(Constants.ExceptionMessages.FormatParserOptionNotFoundForCommand(commandType.Metadata.Name, optionText));
                return null;
            }

            if (option.ShouldProvideValue)
            {
                if (string.IsNullOrEmpty(value))
                {
                    value = argumentsEnumerator.GetNextCommandLineItem() ?? string.Empty;
                }
            }

            option.AssignValue(value);
        }
        return commandObjectBuilder;
    }
}
