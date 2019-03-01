using System.Collections.Generic;
using System.Linq;

namespace MGR.CommandLineParser.Command.Lambda
{
    internal class LambdaBasedCommandOptionCollectionValueAssigner : ILambdaBasedCommandOptionValueAssigner
    {
        private readonly List<object> _values = new List<object>();
        public object GetValue() => _values.AsEnumerable();

        public void AssignValue(object value) => _values.Add(value);
    }
}