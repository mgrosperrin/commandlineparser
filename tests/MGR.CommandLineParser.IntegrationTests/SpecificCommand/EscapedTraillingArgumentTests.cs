﻿using System.Collections.Generic;
using MGR.CommandLineParser.Tests.Commands;
using Xunit;

namespace MGR.CommandLineParser.IntegrationTests.SpecificCommand
{
    public class EscapedTraillingArgumentTests : ConsoleLoggingTestsBase
    {
        [Fact]
        public void ParseWithValidArgsAnDoubleDash()
        {
            // Arrange
            var parserBuild = new ParserBuilder();
            var parser = parserBuild.BuildParser();
            IEnumerable<string> args = new[] {"-Strvalue:custom value", "-i", "42", "Custom argument value", "-b", "--", "firstArg", "-i", "32"};
            var expectedReturnCode = CommandParsingResultCode.Success;
            var expectedStrValue = "custom value";
            var expectedNbOfArguments = 4;
            var expectedArgumentsValue = "Custom argument value";
            var expectedIntValue = 42;

            // Act
            var actual = parser.Parse<IntTestCommand>(args);

            // Assert
            Assert.True(actual.IsValid);
            Assert.Equal(expectedReturnCode, actual.ParsingResultCode);
            Assert.IsType<IntTestCommand>(actual.Command);
            Assert.Equal(expectedStrValue, actual.Command.StrValue);
            Assert.Equal(expectedIntValue, actual.Command.IntValue);
            Assert.Null(actual.Command.IntListValue);
            Assert.Equal(expectedNbOfArguments, actual.Command.Arguments.Count);
            Assert.Equal(new List<string> {expectedArgumentsValue, "firstArg", "-i", "32"}, actual.Command.Arguments);
            Assert.True(actual.Command.BoolValue);
        }
    }
}