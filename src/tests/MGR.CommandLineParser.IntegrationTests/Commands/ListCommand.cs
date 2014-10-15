using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using MGR.CommandLineParser.Command;

// ReSharper disable CheckNamespace
namespace MGR.CommandLineParser.Tests.Commands
// ReSharper restore CheckNamespace
{
    [CommandDisplay(Description = "ListCommandDescription", Usage = "ListCommandUsageDescription")]
    public class ListCommand : CommandBase
    {
        private readonly List<string> _sources = new List<string>();

        [Display(Description = "ListCommandSourceDescription")]
        public ICollection<string> Source
        {
            get { return _sources; }
        }

        [Display(Description = "ListCommandVerboseListDescription")]
        public bool Verbose { get; set; }

        [Display(Description = "ListCommandAllVersionsDescription")]
        public bool AllVersions { get; set; }

        [Display(Description = "ListCommandPrerelease")]
        public bool Prerelease { get; set; }

        [IgnoreOptionProperty]
        public object RepositoryFactory { get; private set; }

        [IgnoreOptionProperty]
        public object SourceProvider { get; private set; }

        protected override int ExecuteCommand()
        {
            return 0;
        }
    }
}