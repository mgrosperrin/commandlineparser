using MGR.CommandLineParser.Command;

namespace MGR.CommandLineParser.Tests.Commands
{
    [Command(Description = "HiddenCommandDescription", Usage = "HiddenCommandUsage", HideFromHelpListing = true)]
    public class HiddenCommand : CommandBase
    {
        protected override int ExecuteCommand()
        {
            throw new System.NotImplementedException();
        }
    }
}