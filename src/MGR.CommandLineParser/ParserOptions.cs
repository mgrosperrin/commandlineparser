using System.Globalization;
using System.Reflection;
using MGR.CommandLineParser.Properties;

namespace MGR.CommandLineParser;

/// <summary>
/// Defines the options for the parser.
/// </summary>
public sealed class ParserOptions
{
    /// <summary>
    /// Creates a new instance of <see cref="ParserOptions"/>.
    /// </summary>
    public ParserOptions()
    {
        var entryAssembly = Assembly.GetEntryAssembly();
        if (entryAssembly != null)
        {
            FromAssembly(entryAssembly);
        }
    }
    /// <summary>
    /// Initializes the <seealso cref="Logo"/> and the <seealso cref="CommandLineName"/> based on an assembly.
    /// </summary>
    /// <param name="entryAssembly">The <see cref="Assembly"/> from which to extract the <seealso cref="Logo"/> and the <seealso cref="CommandLineName"/>.</param>
    public void FromAssembly(Assembly entryAssembly)
    {
        Guard.NotNull(entryAssembly, nameof(entryAssembly));

        var entryAssemblyName = entryAssembly.GetName();
        CommandLineName = entryAssemblyName.Name;
        Logo = string.Format(CultureInfo.CurrentUICulture, Strings.ParserOptions_LogoFormat, entryAssemblyName.Name, entryAssemblyName.Version);
    }
    /// <summary>
    /// Gets or sets the logo used in the help by the parser.
    /// </summary>
    public string Logo { get; set; } = "Not initialized logo";

    /// <summary>
    /// Gets or sets the name of the executable to run used in the help by the parser.
    /// </summary>
    public string CommandLineName { get; set; } = "Not initialized command line name";
}