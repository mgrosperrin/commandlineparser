using System;
using System.Threading.Tasks;
using MGR.CommandLineParser.Command;

namespace MGR.CommandLineParser.Tests.Commands;
public class EnumCommand : CommandBase
{
    public AttributeTargets Target { get; set; }
    public EnumCommand(IServiceProvider serviceProvider) : base(serviceProvider)
    {
    }

    protected override Task<int> ExecuteCommandAsync() => Task.FromResult(0);
}
