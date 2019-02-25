using System;

namespace MGR.CommandLineParser.Command
{
    /// <summary>
    /// Ignore the property as option.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class IgnoreOptionPropertyAttribute : Attribute
    {
    }
}