namespace MGR.CommandLineParser.Extensibility;

/// <summary>
/// Defines the console to log the parser activity.
/// </summary>
public interface IConsole
{
    /// <summary>
    /// Writes the value representation of the specified array of objects to the standard output stream using the specified format information.
    /// </summary>
    /// <param name="format">A composite format string.</param>
    /// <param name="args">An array of objects to write using <paramref name="format"/>.</param>
    void Write(string format, params object[] args);

    /// <summary>
    /// Writes the value representation of the specified array of objects, followed by the current line terminator, to the error output stream using the specified format information.
    /// </summary>
    /// <param name="format">A composite format string.</param>
    /// <param name="args">An array of objects to write using <paramref name="format"/>.</param>
    void WriteError(string format, params object[] args);

    /// <summary>
    /// Writes the value representation of the specified array of objects, followed by the current line terminator, to the warning output stream using the specified format information.
    /// </summary>
    /// <param name="format">A composite format string.</param>
    /// <param name="args">An array of objects to write using <paramref name="format"/>.</param>
    void WriteWarning(string format, params object[] args);
}