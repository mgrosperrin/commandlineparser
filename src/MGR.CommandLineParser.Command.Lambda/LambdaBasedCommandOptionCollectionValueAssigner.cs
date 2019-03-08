using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace MGR.CommandLineParser.Command.Lambda
{
    [DebuggerDisplay("Collection value assigner (currently {Values.Count} values)")]
    internal class LambdaBasedCommandOptionCollectionValueAssigner : ILambdaBasedCommandOptionValueAssigner
    {
        internal List<object> Values { get; } = new List<object>();
        public object GetValue() => Values.AsEnumerable();

        public void AssignValue(object value) => Values.Add(value);
    }
}