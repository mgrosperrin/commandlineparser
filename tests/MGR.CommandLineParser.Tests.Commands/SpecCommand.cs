using System.ComponentModel.DataAnnotations;
using MGR.CommandLineParser.Command;

namespace MGR.CommandLineParser.Tests.Commands;

[Command(Description = "SpecCommandDescription", Usage = "SpecCommandUsageSummary")]
public class SpecCommand : CommandBase<SpecCommand.SpecCommandData>
{
    public class SpecCommandData : HelpedCommandData
    {
        internal static readonly string SampleProjectUrl = "http://PROJECT_URL_HERE_OR_DELETE_THIS_LINE";
        internal static readonly string SampleLicenseUrl = "http://LICENSE_URL_HERE_OR_DELETE_THIS_LINE";
        internal static readonly string SampleIconUrl = "http://ICON_URL_HERE_OR_DELETE_THIS_LINE";
        internal static readonly string SampleTags = "Tag1 Tag2";
        internal static readonly string SampleReleaseNotes = "Summary of changes made in this release of the package.";
        internal static readonly string SampleDescription = "Package description";

        [Display(Description = "SpecCommandAssemblyPathDescription")]
        public string AssemblyPath { get; set; }

        [Display(Description = "SpecCommandForceDescription")]
        public bool Force { get; set; }
    }

    protected override Task<int> ExecuteCommandAsync(SpecCommandData commandData, CancellationToken cancellationToken) => Task.FromResult(0);

    public SpecCommand(IServiceProvider serviceProvider) : base(serviceProvider)
    {
    }
}