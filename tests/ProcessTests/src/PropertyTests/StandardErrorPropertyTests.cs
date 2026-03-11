using System.Collections.Generic;
using WB.Toolkit;
using R3;
using System.Threading.Tasks;
using AwesomeAssertions;

namespace ProcessTests.PropertyTests.StandardErrorPropertyTests;

public sealed class TheStandardErrorProperty
{
    [Test]
    public async Task ShouldProvideStandardErrorMessages()
    {
        // Arrange
        List<string> standardErrorMessages = [];
        using Process process = new("env", "-i", "/bin/bash", "--noprofile", "--norc", "-c", "echo test >&2");
        process.StandardError.Subscribe(standardErrorMessages.Add);

        // Act
        int exitCode = await process.ExecuteAsync().ConfigureAwait(false);

        // Assert
        exitCode.Should().Be(0, because: "the process should exit successfully");
        standardErrorMessages.Should().BeEquivalentTo(["test"], because: "there should be no error output by default");
    }
}