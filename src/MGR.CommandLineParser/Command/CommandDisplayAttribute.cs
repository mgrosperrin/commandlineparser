using System;
using System.Runtime;

namespace MGR.CommandLineParser.Command
{
    /// <summary>
    /// Defines the description and the usage of the command.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class CommandDisplayAttribute : Attribute
    {
        private readonly LocalizableString _description = new LocalizableString(nameof(Description));
        private readonly LocalizableString _usage = new LocalizableString(nameof(Usage));
        private Type _resourceType;


        //[SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate")]
        internal string GetLocalizedDescription() => _description.GetLocalizableValue();

        //[SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate")]
        internal string GetLocalizedUsage() => _usage.GetLocalizableValue();

        /// <summary>
        /// Gets or sets the description of the command.
        /// </summary>
        /// <remarks>If the property <seealso cref="ResourceType"/> is not null, this is the name of the resource used to determine the description.</remarks>
        public string Description
        {
            get { return _description.Value; }
            set
            {
                if (_description.Value != value)
                {
                    _description.Value = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the usage of the command.
        /// </summary>
        /// <remarks>If the property <seealso cref="ResourceType"/> is not null, this is the name of the resource used to determine the usage.</remarks>
        public string Usage
        {
            get { return _usage.Value; }
            set
            {
                if (_usage.Value != value)
                {
                    _usage.Value = value;
                }
            }
        }

        /// <summary>
        /// The type of the resource used to determine the values.
        /// </summary>
        public Type ResourceType
        {
            [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
            get { return _resourceType; }
            set
            {
                if (_resourceType != value)
                {
                    _resourceType = value;
                    _description.ResourceType = value;
                    _usage.ResourceType = value;
                }
            }
        }
    }
}