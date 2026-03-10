using System.Collections.Generic;
using AwesomeAssertions;
using WB.Process;

namespace ProcessTests.PropertyTests.ArgumentsProperyTests;

public sealed class TheArgumentsProperty
{
    [Test]
    public void ShouldBeEmptyAtDefault()
    {
        // Arrange
        using Process process = new("test");

        // Act
        IReadOnlyCollection<string> arguments = process.Arguments;

        // Assert
        arguments.Should().BeEmpty();
    }

    [Test]
    public void ShouldProvideTheArgumentsFromConstructor()
    {
        // Arrange
        using Process process = new("test", "a", "b", "c");

        // Act
        IReadOnlyCollection<string> arguments = process.Arguments;

        // Assert
        arguments.Should().BeEquivalentTo(["a", "b", "c"]);
    }
}