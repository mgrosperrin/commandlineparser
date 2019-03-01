using System.Collections.Generic;

namespace MGR.CommandLineParser.Command.Lambda
{
    internal class LambdaBasedCommandOptionDictionnaryValueAssigner : ILambdaBasedCommandOptionValueAssigner
    {
        private readonly Dictionary<string, object> _values = new Dictionary<string, object>();
        public object GetValue() => _values;

        public void AssignValue(object value)
        {
            var kvp = (KeyValuePair<string, object>) value;
            _values.Add(kvp.Key, kvp.Value);
        }
    }
}