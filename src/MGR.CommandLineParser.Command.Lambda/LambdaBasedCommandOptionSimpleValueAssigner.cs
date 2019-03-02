namespace MGR.CommandLineParser.Command.Lambda
{
    internal class LambdaBasedCommandOptionSimpleValueAssigner : ILambdaBasedCommandOptionValueAssigner
    {
        private object _value;

        public object GetValue() => _value;

        public void AssignValue(object value) => _value = value;
    }
}