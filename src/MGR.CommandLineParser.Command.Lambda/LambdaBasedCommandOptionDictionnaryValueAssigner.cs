using System.Collections.Generic;

namespace MGR.CommandLineParser.Command.Lambda
{
    internal class LambdaBasedCommandOptionDictionnaryValueAssigner : ILambdaBasedCommandOptionValueAssigner
    {
        private readonly Dictionary<object, object> _values = new Dictionary<object, object>();

        public object GetValue() => _values;

        public void AssignValue(object value)
        {
            var kvp = (KeyValuePair<object, object>) value;
            _values.Add(kvp.Key, kvp.Value);
        }
    }
}