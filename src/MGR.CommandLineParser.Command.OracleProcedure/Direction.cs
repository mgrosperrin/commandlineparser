using System;

namespace MGR.CommandLineParser.Command.OracleProcedure
{
    [Flags]
    internal enum Direction
    {
        In = 1,
        Out = 2,
        InOut = 3
    }
}