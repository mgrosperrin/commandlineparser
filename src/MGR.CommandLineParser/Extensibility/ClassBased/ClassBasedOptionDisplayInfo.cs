using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Reflection;
using MGR.CommandLineParser.Extensibility.Command;

namespace MGR.CommandLineParser.Extensibility.ClassBased;

[DebuggerDisplay("ClassBased:Name={Name};ShortName={ShortName}")]
internal sealed class ClassBasedOptionDisplayInfo : IOptionDisplayInfo
{
    internal ClassBasedOptionDisplayInfo(PropertyInfo propertyInfo, IEnumerable<IPropertyOptionAlternateNameGenerator> optionAlternateNameGenerators)
    {
        Guard.NotNull(propertyInfo, nameof(propertyInfo));
        var displayAttribute = propertyInfo.GetCustomAttributes(typeof(DisplayAttribute), true).FirstOrDefault() as DisplayAttribute;
        Name = displayAttribute?.GetName() ?? propertyInfo.Name.AsKebabCase();
        ShortName = displayAttribute?.GetShortName() ?? string.Empty;
        Description = displayAttribute?.GetDescription() ?? string.Empty;

        AlternateNames = optionAlternateNameGenerators.SelectMany(
                generator => generator.GenerateAlternateNames(propertyInfo))
            .Distinct(StringComparer.CurrentCultureIgnoreCase)
            .Where(alternateName => !alternateName.Equals(Name, StringComparison.CurrentCultureIgnoreCase))
            .ToList();
    }

    /// <inheritdoc />
    public string Name { get; }

    /// <inheritdoc />
    public IEnumerable<string> AlternateNames { get; }

    /// <inheritdoc />
    public string ShortName { get; }

    /// <inheritdoc />
    public string Description { get; }
}