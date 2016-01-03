using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MGR.CommandLineParser.Converters;
using Xunit;

namespace MGR.CommandLineParser.UnitTests.Converters
{
    public class FileSystemInfoConverterTests
    {
        [Fact]
        public void TargetType()
        {
            // Arrange
            IConverter converter = new FileSystemInfoConverter();
            var expectedType = typeof(FileSystemInfo);

            // Act
            var actual = converter.TargetType;

            // Assert
            Assert.Equal(expectedType, actual);
        }

        [Fact]
        public void FileInfo_WithValidPath_Conversion()
        {
            // Arrange
            IConverter converter = new FileSystemInfoConverter();
            var value = @"C:\temp\file.txt";

            // Act
            var actual = converter.Convert(value, typeof(FileInfo));

            // Assert
            Assert.NotNull(actual);
            Assert.IsType<FileInfo>(actual);
            Assert.Equal(value, ((FileInfo)actual).FullName);
        }

        [Fact]
        public void DirectoryInfo_WithValidPath_Conversion()
        {
            // Arrange
            IConverter converter = new FileSystemInfoConverter();
            var value = @"C:\temp\file.txt";

            // Act
            var actual = converter.Convert(value, typeof(DirectoryInfo));

            // Assert
            Assert.NotNull(actual);
            Assert.IsType<DirectoryInfo>(actual);
            Assert.Equal(value, ((DirectoryInfo)actual).FullName);
        }

        [Fact]
        [Trait(nameof(Exception), nameof(CommandLineParserException))]
        public void FileInfo_WithInValidPath_Conversion()
        {
            // Arrange
            IConverter converter = new FileSystemInfoConverter();
            var value = "Hello:world";
            var expectedType = typeof (FileInfo);
            var expectedExceptionMessage = Constants.ExceptionMessages.FormatConverterUnableConvert(value, expectedType);
            var expectedInnerExceptionMessage = "The given path's format is not supported.";

            // Act
            using (new LangageSwitcher("en-us"))
            {
                var actualException = Assert.Throws<CommandLineParserException>(() => converter.Convert(value, expectedType));

                // Assert
                Assert.Equal(expectedExceptionMessage, actualException.Message);
                Assert.NotNull(actualException.InnerException);
                var actualInnerExecption = Assert.IsAssignableFrom<NotSupportedException>(actualException.InnerException);
                Assert.Equal(expectedInnerExceptionMessage, actualInnerExecption.Message);
            }
        }
        [Fact]
        [Trait(nameof(Exception), nameof(CommandLineParserException))]
        public void DirectoryInfo_WithInValidPath_Conversion()
        {
            // Arrange
            IConverter converter = new FileSystemInfoConverter();
            var value = "Hello\\the:World";
            var expectedType = typeof(DirectoryInfo);
            var expectedExceptionMessage = Constants.ExceptionMessages.FormatConverterUnableConvert(value, expectedType);
            var expectedInnerExceptionMessage = "The given path's format is not supported.";

            // Act
            using (new LangageSwitcher("en-us"))
            {
                var actualException = Assert.Throws<CommandLineParserException>(() => converter.Convert(value, expectedType));

                // Assert
                Assert.Equal(expectedExceptionMessage, actualException.Message);
                Assert.NotNull(actualException.InnerException);
                var actualInnerExecption = Assert.IsAssignableFrom<NotSupportedException>(actualException.InnerException);
                Assert.Equal(expectedInnerExceptionMessage, actualInnerExecption.Message);
            }
        }
    }
}