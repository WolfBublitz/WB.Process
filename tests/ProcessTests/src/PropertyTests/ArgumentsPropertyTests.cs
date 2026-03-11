using System.Collections.Generic;
using AwesomeAssertions;
using WB.Toolkit;

namespace ProcessTests.PropertyTests.ArgumentsPropertyTests;

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