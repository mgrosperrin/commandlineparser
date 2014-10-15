using System;

// ReSharper disable CheckNamespace

namespace MGR.CommandLineParser.Converters
// ReSharper restore CheckNamespace
{
    /// <summary>
    ///     Extensions methods for the type <see cref="IConverter" />.
    /// </summary>
    public static class ConverterExtensions
    {
        /// <summary>
        ///     Indicates if the specified <see cref="IConverter" /> can convert to the specified <see cref="Type" />.
        /// </summary>
        /// <param name="converter">The converter.</param>
        /// <param name="targetType">
        ///     The target <see cref="Type" />.
        /// </param>
        /// <returns>
        ///     true if the <see cref="IConverter" /> can convert, false otherwise.
        /// </returns>
        public static bool CanConvertTo(this IConverter converter, Type targetType)
        {
            if (converter == null)
            {
                throw new ArgumentNullException("converter");
            }
            if (targetType == null)
            {
                throw new ArgumentNullException("targetType");
            }
            Type type = targetType.IsMultiValuedType() ? targetType.GetUnderlyingCollectionType() : targetType;
            return type.IsAssignableFrom(converter.TargetType);
        }
    }
}