using System;
using System.Threading.Tasks;
using MGR.CommandLineParser.Command;

namespace MGR.CommandLineParser.Tests.Commands
{
    [Command(Description = "HiddenCommandDescription", Usage = "HiddenCommandUsage", HideFromHelpListing = true)]
    public class HiddenCommand : CommandBase
    {
        protected override Task<int> ExecuteCommandAsync()
        {
            throw new NotImplementedException();
        }

        public HiddenCommand(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }
    }
}