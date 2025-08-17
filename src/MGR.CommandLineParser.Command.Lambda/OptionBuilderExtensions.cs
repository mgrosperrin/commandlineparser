using System.ComponentModel.DataAnnotations;

namespace MGR.CommandLineParser.Command.Lambda;

/// <summary>
/// Extension methods for the <see cref="OptionBuilder"/>.
/// </summary>
public static class OptionBuilderExtensions
{
    /// <summary>
    /// Add a validation to the option.
    /// </summary>
    /// <typeparam name="TValidation">The type of the validation attribute.</typeparam>
    /// <param name="optionBuilder">The <see cref="OptionBuilder"/>.</param>
    /// <returns>The <see cref="OptionBuilder"/> to chain calls.</returns>
    public static OptionBuilder AddValidation<TValidation>(this OptionBuilder optionBuilder)
        where TValidation : ValidationAttribute, new()
    {
        optionBuilder.AddValidation(new TValidation());
        return optionBuilder;
    }

    /// <summary>
    /// Add the required validation to the option.
    /// </summary>
    /// <param name="optionBuilder">The <see cref="OptionBuilder"/>.</param>
    /// <returns>The <see cref="OptionBuilder"/> to chain calls.</returns>
    public static OptionBuilder Required(this OptionBuilder optionBuilder)
    {
        return optionBuilder.AddValidation<RequiredAttribute>();
    }
}