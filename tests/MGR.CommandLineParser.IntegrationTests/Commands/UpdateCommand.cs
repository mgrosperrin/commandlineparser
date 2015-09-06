using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using MGR.CommandLineParser.Command;

// ReSharper disable CheckNamespace
namespace MGR.CommandLineParser.Tests.Commands
// ReSharper restore CheckNamespace
{
    [CommandDisplay(Description = "UpdateCommandDescription", Usage = "<packages.config|solution>")]
    public class UpdateCommand : CommandBase
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

        protected override int ExecuteCommand() => 0;
    }
}