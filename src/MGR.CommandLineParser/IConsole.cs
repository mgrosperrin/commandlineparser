namespace MGR.CommandLineParser
{
    /// <summary>
    /// Defines e console to log the parser activity.
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
        /// Writes the current line terminator.
        /// </summary>
        void WriteLine();
        /// <summary>
        /// Writes the specified string value, followed by the current line terminator to the standard output stream .
        /// </summary>
        /// <param name="value">The value to write. </param>
        void WriteLine(string value);
        /// <summary>
        /// Writes the value representation of the specified array of objects, followed by the current line terminator, to the standard output stream using the specified format information.
        /// </summary>
        /// <param name="format">A composite format string.</param>
        /// <param name="args">An array of objects to write using <paramref name="format"/>.</param>
        void WriteLine(string format, params object[] args);
        /// <summary>
        /// Writes the specified string value, followed by the current line terminator to the error output stream .
        /// </summary>
        /// <param name="value"></param>
        void WriteError(string value);
        /// <summary>
        /// Writes the value representation of the specified array of objects, followed by the current line terminator, to the error output stream using the specified format information.
        /// </summary>
        /// <param name="format">A composite format string.</param>
        /// <param name="args">An array of objects to write using <paramref name="format"/>.</param>
        void WriteError(string format, params object[] args);
        /// <summary>
        /// Writes the specified string value, followed by the current line terminator to the warning output stream .
        /// </summary>
        /// <param name="value"></param>
        void WriteWarning(string value);
        /// <summary>
        /// Writes the value representation of the specified array of objects, followed by the current line terminator, to the warning output stream using the specified format information.
        /// </summary>
        /// <param name="format">A composite format string.</param>
        /// <param name="args">An array of objects to write using <paramref name="format"/>.</param>
        void WriteWarning(string format, params object[] args);
        /// <summary>
        /// Writes the specified string value, followed by the current line terminator to the standard output stream, justified to the console size, starting at the specified <paramref name="startIndex"/>.
        /// </summary>
        /// <param name="startIndex"></param>
        /// <param name="value"></param>
        void PrintJustified(int startIndex, string value);
    }
}