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
        /// <param name="source">The converter.</param>
        /// <param name="targetType">
        ///     The target <see cref="Type" />.
        /// </param>
        /// <returns>
        ///     true if the <see cref="IConverter" /> can convert, false otherwise.
        /// </returns>
        public static bool CanConvertTo(this IConverter source, Type targetType)
        {
            Guard.NotNull(source, nameof(source));
            Guard.NotNull(targetType, nameof(targetType));

            var type = targetType.IsMultiValuedType() ? targetType.GetUnderlyingCollectionType() : targetType;
            return type.IsAssignableFrom(source.TargetType);
        }
    }
}