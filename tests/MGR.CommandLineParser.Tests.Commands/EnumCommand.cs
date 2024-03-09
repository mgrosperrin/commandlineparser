using MGR.CommandLineParser.Command;

namespace MGR.CommandLineParser.Tests.Commands;
public class EnumCommand : CommandBase<EnumCommand.EnumCommandData>
{
    public class EnumCommandData : HelpedCommandData
    {
        public AttributeTargets Target { get; set; }
    }
    public EnumCommand(IServiceProvider serviceProvider) : base(serviceProvider)
    {
    }

    protected override Task<int> ExecuteCommandAsync(EnumCommandData commandData, CancellationToken cancellationToken) => Task.FromResult(0);
}
