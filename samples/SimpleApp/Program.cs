using MGR.CommandLineParser.Command.Lambda;
using MGR.CommandLineParser.Hosting.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace SimpleApp;

internal class Program
{
    private static async Task<int> Main(string[] args)
    {
        await ParserBuilderCalls.CreateAndCallDefaultParserBuilderAsync();
        WriteSeparator();
        await ParserBuilderCalls.CreateAndCallCustomizedParserBuilderAsync();
        WriteSeparator();
        await ParserBuilderCalls.CreateAndCallCustomizedWithCommandsParserBuilderAsync();
        WriteSeparator();

        await ParserFactoryCalls.ConfigureParserAndGetParserFactoryAsync();
        WriteSeparator();
        await ParserFactoryCalls.ConfigureParserAndGetParserFactoryWithDefaultCommandAsync();
        WriteSeparator();
        await ParserFactoryCalls.ConfigureParserAndGetParserFactoryWithSpecificCommandAsync();
        WriteSeparator();

        await HostBuilderCalls.CreateAndCallDefaultParserBuilderAsync();
        WriteSeparator();

        //await Tester.RunSampleTests();

        var hostBuilder = new HostBuilder();
        hostBuilder.ConfigureParser(builder =>
        {
            builder.AddCommand("run",
                commandBuilder =>
                {
                    commandBuilder.AddOption<int>("opt", "o",
                        o => o.Required());
                },
                (context, cancellationToken) =>
                {
                    var opt = context.GetOptionValue<int>("opt");
                    return Task.FromResult(opt);
                });
        });
        var parsingResult = await hostBuilder.ParseCommandLineAndExecuteAsync(args, CancellationToken.None);
        return parsingResult;
    }

    static void WriteSeparator()
    {
        Console.WriteLine();
        Console.WriteLine("-------------");
        Console.WriteLine();
    }
}