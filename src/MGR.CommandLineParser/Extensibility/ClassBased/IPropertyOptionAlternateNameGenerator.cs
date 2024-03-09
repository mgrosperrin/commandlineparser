using System.Reflection;

namespace MGR.CommandLineParser.Extensibility.ClassBased;

/// <summary>
/// Defines the contract for the generation of alternate names.
/// </summary>
public interface IPropertyOptionAlternateNameGenerator
{
    /// <summary>
    /// Gets the alternate names for an option.
    /// </summary>
    /// <param name="propertyInfo">The <see cref="PropertyInfo" /> for the option.</param>
    /// <returns>The alternate names for the option.</returns>
    IEnumerable<string> GenerateAlternateNames(PropertyInfo propertyInfo);
}