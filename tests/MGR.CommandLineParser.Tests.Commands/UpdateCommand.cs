using System.ComponentModel.DataAnnotations;
using MGR.CommandLineParser.Command;

namespace MGR.CommandLineParser.Tests.Commands;

[Command(Description = "UpdateCommandDescription", Usage = "<packages.config|solution>")]
public class UpdateCommand : CommandBase<UpdateCommand.UpdateCommandOptions>
{
    public class UpdateCommandOptions : HelpedCommandData
    {
        private readonly List<string> _sources = new List<string>();
        private readonly List<string> _ids = new List<string>();

        [IgnoreOptionProperty]
        public object RepositoryFactory { get; }

        [IgnoreOptionProperty]
        public object SourceProvider { get; }

        [Display(Description = "UpdateCommandSourceDescription")]
        public ICollection<string> Source => _sources;

        [Display(Description = "UpdateCommandIdDescription")]
        public ICollection<string> Id => _ids;

        [Display(Description = "UpdateCommandRepositoryPathDescription")]
        public string RepositoryPath { get; set; }

        [Display(Description = "UpdateCommandSafeDescription")]
        public bool Safe { get; set; }

        [Display(Description = "UpdateCommandSelfDescription")]
        public bool Self { get; set; }

        [Display(Description = "UpdateCommandVerboseDescription")]
        public bool Verbose { get; set; }

        [Display(Description = "UpdateCommandPrerelease")]
        public bool Prerelease { get; set; }
    }

    protected override Task<int> ExecuteCommandAsync(UpdateCommandOptions commandData, CancellationToken cancellationToken) => Task.FromResult(0);

    public UpdateCommand(IServiceProvider serviceProvider) : base(serviceProvider)
    {
    }
}