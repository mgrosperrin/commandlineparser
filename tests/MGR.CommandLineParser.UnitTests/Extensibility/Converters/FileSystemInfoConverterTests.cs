using MGR.CommandLineParser.Extensibility.Converters;
using Xunit;

namespace MGR.CommandLineParser.UnitTests.Extensibility.Converters;

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
}