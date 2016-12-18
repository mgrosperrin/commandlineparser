using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using MGR.CommandLineParser.Command;

namespace MGR.CommandLineParser.Tests.Commands
{
    public class IntTestCommand : CommandBase
    {
        [Display(ShortName = "s", Description = "A simple string value")]
        [Required]
        public string StrValue { get; set; }
        [Display(ShortName = "i", Description = "A simple integer value")]
        public int IntValue { get; set; }
        [Display(ShortName = "b", Description = "A boolean value")]
        public bool BoolValue { get; set; }
        [Display(ShortName = "il", Description = "A list of integer value")]
        public List<int> IntListValue { get; set; }


        protected override int ExecuteCommand()
        {
            throw new NotImplementedException();
        }
    }
}