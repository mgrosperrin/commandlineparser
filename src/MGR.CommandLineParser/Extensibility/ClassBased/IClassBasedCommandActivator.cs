﻿using System;
using JetBrains.Annotations;
using MGR.CommandLineParser.Command;

namespace MGR.CommandLineParser.Extensibility.ClassBased;

/// <summary>
///     Defines the contract for the activator of <see cref="ICommandHandler" />.
/// </summary>
public interface IClassBasedCommandActivator
{
    /// <summary>
    ///     Activates (create an instance) of a <see cref="ICommandHandler" />.
    /// </summary>
    /// <param name="commandType">The type of the command.</param>
    /// <returns>The command.</returns>
    ICommandHandler ActivateCommand([NotNull] Type commandType);
}