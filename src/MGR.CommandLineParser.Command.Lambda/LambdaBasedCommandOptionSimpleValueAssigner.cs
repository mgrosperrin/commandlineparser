using System.Diagnostics;

namespace MGR.CommandLineParser.Command.Lambda;

[DebuggerDisplay("Simple value assigner (current value:{Value})")]
internal class LambdaBasedCommandOptionSimpleValueAssigner : ILambdaBasedCommandOptionValueAssigner
{
    internal object? Value { get; private set; }
    public object? GetValue() => Value;

    public void AssignValue(object value) => Value = value;
}