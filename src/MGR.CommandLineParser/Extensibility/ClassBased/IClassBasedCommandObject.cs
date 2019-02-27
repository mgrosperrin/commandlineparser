﻿using MGR.CommandLineParser.Command;
using MGR.CommandLineParser.Extensibility.Command;

namespace MGR.CommandLineParser.Extensibility.ClassBased
{
    /// <summary>
    /// Represents an interface that allow accessing to the raw <see cref="ICommand"/> instance.
    /// </summary>
    public interface IClassBasedCommandObject
    {
        /// <summary>
        /// Gets the raw <see cref="ICommand"/> instance for the <see cref="ICommandObjectBuilder"/>.
        /// </summary>
        ICommand Command { get; }
    }
}