using System.Collections.Generic;
using WB.Toolkit;
using R3;
using System.Threading.Tasks;
using AwesomeAssertions;

namespace ProcessTests.PropertyTests.StandardOutputPropertyTests;

public sealed class TheStandardOutputProperty
{
    [Test]
    public async Task ShouldProvideStandardOutputMessages()
    {
        // Arrange
        List<string> standardOutputMessages = [];
        using Process process = new("echo", "test");
        process.StandardOutput.Subscribe(standardOutputMessages.Add);

        // Act
        int exitCode = await process.ExecuteAsync().ConfigureAwait(false);

        // Assert
        exitCode.Should().Be(0, because: "the process should exit successfully");
        standardOutputMessages.Should().BeEquivalentTo(["test"], because: "there should be no output by default");
    }

    [Test]
    public async Task ShouldProvideStandardOutputMessagesLineByLine()
    {
        // Arrange
        List<string> standardOutputMessages = [];
        using Process process = new("echo", "test1\ntest2");
        process.StandardOutput.Subscribe(standardOutputMessages.Add);

        // Act
        int exitCode = await process.ExecuteAsync().ConfigureAwait(false);

        // Assert
        exitCode.Should().Be(0, because: "the process should exit successfully");
        standardOutputMessages.Should().BeEquivalentTo(["test1", "test2"], because: "there should be no output by default");
    }
}