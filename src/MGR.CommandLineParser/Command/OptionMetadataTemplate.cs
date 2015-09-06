using System;
using System.Diagnostics;
using System.Reflection;
using MGR.CommandLineParser.Converters;

namespace MGR.CommandLineParser.Command
{
    [DebuggerDisplay("{OptionTemplate:{Name}")]
    internal sealed class OptionMetadataTemplate
    {
        internal OptionMetadataTemplate(PropertyInfo propertyOption, CommandMetadataTemplate commandMetadataTemplate)
        {
            PropertyOption = propertyOption;
            CommandMetadata = commandMetadataTemplate;
            Description = string.Empty;
        }

        internal string Name { get; set; }
        internal string ShortName { get; set; }
        internal string Description { get; set; }
        internal bool IsRequired { get; set; }
        internal IConverter Converter { get; set; }
        internal PropertyInfo PropertyOption { get; set; }
        internal CommandMetadataTemplate CommandMetadata { get; set; }
        internal object DefaultValue { get; set; }

        internal Type OptionType
        {
            get
            {
                if (PropertyOption.PropertyType.IsMultiValuedType())
                {
                    return PropertyOption.PropertyType.GetUnderlyingCollectionType();
                }
                return PropertyOption.PropertyType;
            }
        }


        internal OptionMetadata ToOptionMetadata(CommandMetadata commandMetadata) => new OptionMetadata(this, commandMetadata);
    }
}