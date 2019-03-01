using System;

namespace MGR.CommandLineParser.Extensibility.Command
{
    /// <summary>
    ///
    /// </summary>
    public abstract class CommandOptionMetadataBase : ICommandOptionMetadata
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="isRequired"></param>
        /// <param name="collectionType"></param>
        /// <param name="displayInfo"></param>
        /// <param name="defaultValue"></param>
        protected CommandOptionMetadataBase(bool isRequired, CommandOptionCollectionType collectionType, IOptionDisplayInfo displayInfo, string defaultValue)
        {
            IsRequired = isRequired;
            CollectionType = collectionType;
            DisplayInfo = displayInfo;
            DefaultValue = defaultValue;
        }
        /// <summary>
        ///
        /// </summary>
        public bool IsRequired { get; }
        /// <summary>
        ///
        /// </summary>
        public CommandOptionCollectionType CollectionType { get; }
        /// <summary>
        ///
        /// </summary>
        public IOptionDisplayInfo DisplayInfo { get; }
        /// <summary>
        ///
        /// </summary>
        public string DefaultValue { get; }

        /// <summary>
        ///
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
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
}
