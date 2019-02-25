using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Threading.Tasks;
using MGR.CommandLineParser.Command;

namespace MGR.CommandLineParser.Tests.Commands
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

        protected override Task<int> ExecuteCommandAsync() => Task.FromResult(0);
    }
}