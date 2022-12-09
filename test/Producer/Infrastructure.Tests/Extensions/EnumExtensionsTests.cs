using System.ComponentModel;
using Post.Command.Application.Enums;

namespace Post.Command.Infrastructure.Extensions;

public class EnumExtensionsTests
{
    [Theory(DisplayName = "Convert Kafka PersistenceStatus to EventPersistenceStatus")]
    [InlineData(PersistenceStatus.NotPersisted, EventPersistenceStatus.NotPersisted)]
    [InlineData(PersistenceStatus.Persisted, EventPersistenceStatus.Persisted)]
    [InlineData(PersistenceStatus.PossiblyPersisted, EventPersistenceStatus.PossiblyPersisted)]
    public void ToEventPersistenceStatus_GivenPersistenceStatus_ShouldEventPersistenceStatusEquivalent(
        PersistenceStatus given,
        EventPersistenceStatus expected)
    {
        // Act
        var result = given.ToEventPersistenceStatus();

        // Assert
        result.Should().Be(expected);
    }
}