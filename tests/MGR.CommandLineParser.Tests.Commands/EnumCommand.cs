using System;
using System.Threading.Tasks;
using MGR.CommandLineParser.Command;

namespace MGR.CommandLineParser.Tests.Commands;
public class EnumCommand : CommandBase<EnumCommand.EnumCommandData>
{
    public class EnumCommandData:HelpedCommandData
    {
        public AttributeTargets Target { get; set; }
    }
    public EnumCommand(IServiceProvider serviceProvider) : base(serviceProvider)
    {
    }

    protected override Task<int> ExecuteCommandAsync(EnumCommand.EnumCommandData commandData) => Task.FromResult(0);
}
