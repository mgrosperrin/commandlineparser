using System.Collections.Generic;

namespace MGR.CommandLineParser.Command.OracleProcedure
{
    internal class Procedure
    {
        public string Name { get; set; }
        public IEnumerable<Parameter> Parameters { get; set; }
    }
}