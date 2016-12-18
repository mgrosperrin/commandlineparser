using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.IO;
using MGR.CommandLineParser.Command;

namespace MGR.CommandLineParser.IntegrationTests.Commands
{
    public class ImportCommand : CommandBase
    {
        [Display(ShortName = "p")]
        [DefaultValue(50)]
        public int MaxItemInParallel { get; set; }

        [Display(ShortName = "o")]
        public DirectoryInfo OutputDirectory { get; set; }

        [Display(ShortName = "of")]
        public FileInfo OutputFile { get; set; }

        protected override int ExecuteCommand()
        {
            return 0;
        }
    }
}