namespace MGR.CommandLineParser.Command.OracleProcedure
{
    internal class Parameter
    {
        public string Name { get; set; }
        public bool HasDefaultValue { get; set; }
        public string DefaultValue { get; set; }
        public string DataType { get; set; }
        public Direction Direction { get; set; }
    }
}