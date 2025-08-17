using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using MGR.CommandLineParser.Command;

namespace MGR.CommandLineParser.Tests.Commands;

[Command(Description = "SetApiKeyCommandDescription", Usage = "SetApiKeyCommandUsageDescription")]
public class SetApiKeyCommand : CommandBase<SetApiKeyCommand.SetApiKeyCommandData>
{
    public class SetApiKeyCommandData : HelpedCommandData
    {
        [Display(Description = "SetApiKeyCommandSourceDescription", ShortName = "src")]
        [DefaultValue("DefaultSource")]
        public string Source { get; set; }

        [IgnoreOptionProperty]
        public object SourceProvider { get; }

        [IgnoreOptionProperty]
        public object Settings { get; }
    }

    protected override Task<int> ExecuteCommandAsync(SetApiKeyCommandData commandData, CancellationToken cancellationToken) => Task.FromResult(0);

    public SetApiKeyCommand(IServiceProvider serviceProvider) : base(serviceProvider)
    {
    }
}