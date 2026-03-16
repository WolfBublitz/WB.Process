using FakeItEasy;
using WB.Logging;
using WB.Toolkit;

namespace ProcessTests.PropertyTests.LoggerPropertyTests;

public sealed class TheLoggerProperty
{
    [Test]
    public void ShouldLogStandardOutputMessages()
    {
        // Arrange
        ILogger logger = A.Fake<ILogger>();
        Process process = new("dotnet", ["--version"])
        {
            Logger = logger,
        };

        // Act
        _ = process.ExecuteAsync().ConfigureAwait(false);
        process.Dispose();

        // Assert
        A.CallTo(() => logger.Info(A<string>.Ignored)).MustHaveHappened();
    }
}