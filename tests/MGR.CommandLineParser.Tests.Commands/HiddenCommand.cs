using MGR.CommandLineParser.Command;

namespace MGR.CommandLineParser.Tests.Commands;

[Command(Description = "HiddenCommandDescription", Usage = "HiddenCommandUsage", HideFromHelpListing = true)]
public class HiddenCommand : CommandBase<HiddenCommand.HiddenCommandData>
{
    public class HiddenCommandData : HelpedCommandData { }
    protected override Task<int> ExecuteCommandAsync(HiddenCommandData commandData, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public HiddenCommand(IServiceProvider serviceProvider) : base(serviceProvider)
    {
    }
}