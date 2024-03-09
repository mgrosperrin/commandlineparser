using System.ComponentModel.DataAnnotations;
using MGR.CommandLineParser.Command;

namespace MGR.CommandLineParser.Tests.Commands;

[Command(Description = "InstallCommandDescription", Usage = "InstallCommandUsageSummary")]
public class InstallCommand : CommandBase<InstallCommand.InstallCommandData>
{
    public class InstallCommandData : HelpedCommandData
    {
        private readonly ICollection<string> _sources = new List<string>();

        [Display(Description = "InstallCommandSourceDescription")]
        public ICollection<string> Source => _sources;

        [Display(Description = "InstallCommandOutputDirDescription")]
        public string OutputDirectory { get; set; }

        [Display(Description = "InstallCommandVersionDescription")]
        public string Version { get; set; }

        [Display(Description = "InstallCommandExcludeVersionDescription", ShortName = "x")]
        public bool ExcludeVersion { get; set; }

        [Display(Description = "InstallCommandPrerelease")]
        public bool Prerelease { get; set; }

        [Display(Description = "InstallCommandNoCache")]
        public bool NoCache { get; set; }

        [IgnoreOptionProperty]
        public object RepositoryFactory { get; }

        [IgnoreOptionProperty]
        public object SourceProvider { get; }

        /// <remarks>
        /// Meant for unit testing.
        /// </remarks>
        [IgnoreOptionProperty]
        protected object CacheRepository => null;

        [IgnoreOptionProperty]
        private bool AllowMultipleVersions => !ExcludeVersion;
    }
    protected override Task<int> ExecuteCommandAsync(InstallCommandData commandData, CancellationToken cancellationToken) => Task.FromResult(0);

    public InstallCommand(IServiceProvider serviceProvider) : base(serviceProvider)
    {
    }
}