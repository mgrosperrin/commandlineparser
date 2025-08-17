namespace MGR.CommandLineParser.Hosting;

internal class ParserContext
{
    internal required IEnumerable<string> Arguments { get; set; }
    internal int ParsingAndExecutionResult { get; set; }
    public required Func<IParser, IEnumerable<string>, Task<ParsingResult>> ParseArguments { get; set; }
}