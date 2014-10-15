using System;

namespace MGR.CommandLineParser.Command
{
    /// <summary>
    /// Ignore the property as option.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public sealed class IgnoreOptionPropertyAttribute : Attribute
    {
    }
}