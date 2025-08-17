using System.Diagnostics;

namespace MGR.CommandLineParser.Command.Lambda;

[DebuggerDisplay("Dictionary value assigner (currently {Values.Count} values)")]
internal class LambdaBasedCommandOptionDictionaryValueAssigner : ILambdaBasedCommandOptionValueAssigner
{
    internal Dictionary<object, object> Values { get; } = new Dictionary<object, object>();
    public object GetValue() => Values;

    public void AssignValue(object value)
    {
        var kvp = (KeyValuePair<object, object>)value;
        Values.Add(kvp.Key, kvp.Value);
    }
}