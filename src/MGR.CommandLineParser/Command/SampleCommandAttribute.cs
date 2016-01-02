using System;
using System.Diagnostics.CodeAnalysis;

namespace MGR.CommandLineParser.Command
{
    /// <summary>
    ///     Defines samples for the command.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class SampleCommandAttribute : Attribute
    {
        /// <summary>
        ///     Gets or sets the samples for the command.
        /// </summary>
        [SuppressMessage("Microsoft.Performance", "CA1819:PropertiesShouldNotReturnArrays")]
        public string[] Samples { get; set; }
    }
}