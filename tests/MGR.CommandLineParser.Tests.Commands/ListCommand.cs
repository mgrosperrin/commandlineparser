using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using MGR.CommandLineParser.Command;

namespace MGR.CommandLineParser.Tests.Commands
{
    [Command(Description = "ListCommandDescription", Usage = "ListCommandUsageDescription")]
    public class ListCommand : CommandBase
    {
        private readonly List<string> _sources = new List<string>();

        [Display(Description = "ListCommandSourceDescription")]
        public ICollection<string> Source => _sources;

        [Display(Description = "ListCommandVerboseListDescription")]
        public bool Verbose { get; set; }

        [Display(Description = "ListCommandAllVersionsDescription")]
        public bool AllVersions { get; set; }

        [Display(Description = "ListCommandPrerelease")]
        public bool Prerelease { get; set; }

        [IgnoreOptionProperty]
        public object RepositoryFactory { get; }

        [IgnoreOptionProperty]
        public object SourceProvider { get; }

        protected override int ExecuteCommand() => 0;
    }
}