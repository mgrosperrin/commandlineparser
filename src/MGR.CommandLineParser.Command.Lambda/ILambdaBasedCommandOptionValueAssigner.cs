namespace MGR.CommandLineParser.Command.Lambda;

internal interface ILambdaBasedCommandOptionValueAssigner
{
    object? GetValue();
    void AssignValue(object value);
}
