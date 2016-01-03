using System;
using System.IO;

namespace MGR.CommandLineParser.Converters
{
    /// <summary>
    ///     Converter for the type <see cref="FileSystemInfo" /> .
    /// </summary>
    public sealed class FileSystemInfoConverter : IConverter
    {
        /// <summary>
        ///     The target type for the converter (<see cref="FileSystemInfo" />).
        /// </summary>
        public Type TargetType => typeof (FileSystemInfo);

        /// <summary>
        ///     Converts the <paramref name="value" /> to an instnace of <see cref="FileInfo" /> or <see cref="DirectoryInfo" />.
        /// </summary>
        /// <param name="value">The original value provided by the user.</param>
        /// <param name="concreteTargetType">Not used.</param>
        /// <returns>The <see cref="FileInfo" /> or <see cref="DirectoryInfo" />.</returns>
        public object Convert(string value, Type concreteTargetType)
        {
            try
            {
                if (concreteTargetType == typeof (FileInfo))
                {
                    var fileInfo = new FileInfo(value);
                    return fileInfo;
                }
                if (concreteTargetType == typeof (DirectoryInfo))
                {
                    var fileInfo = new DirectoryInfo(value);
                    return fileInfo;
                }
                throw new CommandLineParserException(Constants.ExceptionMessages.FormatConverterUnableConvert(value, concreteTargetType));
            }
            catch (Exception exception)
            {
                throw new CommandLineParserException(Constants.ExceptionMessages.FormatConverterUnableConvert(value, concreteTargetType), exception);
            }
        }
    }
}