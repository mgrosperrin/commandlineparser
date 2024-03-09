namespace MGR.CommandLineParser.Extensibility;

/// <summary>
/// Extensions methods for <see cref="IConsole"/>.
/// </summary>
public static class ConsoleExtensions
{
    private static readonly object[] EmptyArgs = [];
    /// <summary>
    /// Writes the current line terminator.
    /// </summary>
    /// <param name="console">The current console.</param>
    public static void WriteLine(this IConsole console) => console.Write(Environment.NewLine);

    /// <summary>
    /// Writes the specified string value, followed by the current line terminator to the standard output stream.
    /// </summary>
    /// <param name="console">The current console.</param>
    /// <param name="value">The value to write. </param>
    public static void WriteLine(this IConsole console, string value) => console.WriteLine(value, EmptyArgs);
    /// <summary>
    /// Writes the value representation of the specified array of objects, followed by the current line terminator, to the standard output stream using the specified format information.
    /// </summary>
    /// <param name="console">The current console.</param>
    /// <param name="format">A composite format string.</param>
    /// <param name="args">An array of objects to write using <paramref name="format"/>.</param>
    public static void WriteLine(this IConsole console, string format, params object[] args)
    {
        console.Write(format, args);
        console.Write(Environment.NewLine);
    }
    /// <summary>
    /// Writes the specified string value, followed by the current line terminator to the error output stream.
    /// </summary>
    /// <param name="console">The current console.</param>
    /// <param name="value">The error message to write.</param>
    public static void WriteLineError(this IConsole console, string value)
    {
        console.WriteError(value);
        console.WriteError(Environment.NewLine, EmptyArgs);
    }
    /// <summary>
    /// Writes the specified string value, followed by the current line terminator to the warning output stream.
    /// </summary>
    /// <param name="console">The current console.</param>
    /// <param name="value">The warning message to write.</param>
    public static void WriteLineWarning(this IConsole console, string value)
    {
        console.WriteWarning(value, EmptyArgs);
        console.WriteWarning(Environment.NewLine);
    }
}
