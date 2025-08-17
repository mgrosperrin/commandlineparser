using System.Diagnostics;

namespace MGR.CommandLineParser.Extensibility.Command;

/// <summary>
/// Represents a base container for <see cref="ICommandOptionMetadata"/>.
/// </summary>

[DebuggerDisplay("Option {DisplayInfo.Name}, default value: {DefaultValue}")]
public abstract class CommandOptionMetadataBase : ICommandOptionMetadata
{
    /// <summary>
    /// Creates a new instance of <see cref="CommandOptionMetadataBase"/>.
    /// </summary>
    /// <param name="isRequired">Indicates if the option is required.</param>
    /// <param name="collectionType">Indicates the type of collection of the option.</param>
    /// <param name="displayInfo">The <see cref="IOptionDisplayInfo"/> of the option.</param>
    /// <param name="defaultValue">The default value of the option.</param>
    protected CommandOptionMetadataBase(bool isRequired, CommandOptionCollectionType collectionType, IOptionDisplayInfo displayInfo, string defaultValue)
    {
        IsRequired = isRequired;
        CollectionType = collectionType;
        DisplayInfo = displayInfo;
        DefaultValue = defaultValue;
    }

    /// <inheritdoc />
    public bool IsRequired { get; }

    /// <inheritdoc />
    public CommandOptionCollectionType CollectionType { get; }

    /// <inheritdoc />
    public IOptionDisplayInfo DisplayInfo { get; }

    /// <inheritdoc />
    public string DefaultValue { get; }

    /// <summary>
    /// Compute the <see cref="CommandOptionCollectionType"/> based on the <see cref="Type"/> of the option.
    /// </summary>
    /// <param name="type">The <see cref="Type"/> of the option.</param>
    /// <returns>The <see cref="CommandOptionCollectionType"/>.</returns>
    protected static CommandOptionCollectionType GetMultiValueIndicator(Type type)
    {
        if (type.IsDictionaryType())
        {
            return CommandOptionCollectionType.Dictionary;
        }
        if (type.IsCollectionType())
        {
            return CommandOptionCollectionType.Collection;
        }
        return CommandOptionCollectionType.None;
    }
}
