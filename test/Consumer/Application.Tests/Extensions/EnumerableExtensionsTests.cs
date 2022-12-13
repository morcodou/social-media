namespace Post.Query.Application.Extensions;

public class EnumerableExtensionsTests
{

    Fixture _fixture = new();

    [Fact]
    public void ForEach_GivenNullAction_ShouldDoNothing()
    {
        // Arrange
        var collection = _fixture.CreateMany<string>();
        Action<string> action = null!;


        // Act
        var actionCall = () => collection.ForEach(action);

        // Assert
        actionCall.Should().NotThrow();
    }

    [Fact]
    public void ForEach_GivenNullCollection_ShouldDoNothing()
    {
        // Arrange
        IEnumerable<string> collection = null!;
        var action = Mock.Of<Action<string>>();


        // Act
        collection.ForEach(action);

        // Assert
        Mock.Get(action)
            .Verify(x => x(It.IsAny<string>()), Times.Never);
    }

    [Fact]
    public void ForEach_GivenEmptyCollection_ShouldDoNothing()
    {
        // Arrange
        var collection = Enumerable.Empty<string>();
        var action = Mock.Of<Action<string>>();


        // Act
        collection.ForEach(action);

        // Assert
        Mock.Get(action)
            .Verify(x => x(It.IsAny<string>()), Times.Never);
    }


    [Fact]
    public void ForEach_GivenCollectionAndAction_ShouldApplyActionOnEveryItem()
    {
        // Arrange
        var collection = _fixture.CreateMany<string>();
        var action = Mock.Of<Action<string>>();


        // Act
        collection.ForEach(action);

        // Assert
        foreach (var item in collection)
        {
            Mock.Get(action)
                .Verify(x => x(item), Times.Once);
        }
    }


    [Fact]
    public void IsNullOrEmpty_GivenNull_ShouldReturnsTrue()
    {
        // Arrange
        IEnumerable<string> collection = null!;

        // Act
        var result = collection.IsNullOrEmpty();

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void IsNullOrEmpty_GivenEmptyCollection_ShouldReturnsTrue()
    {
        // Arrange
        var collection = Enumerable.Empty<string>();

        // Act
        var result = collection.IsNullOrEmpty();

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void IsNullOrEmpty_GivenCollection_ShouldReturnsFalse()
    {
        // Arrange
        var collection = _fixture.CreateMany<string>();

        // Act
        var result = collection.IsNullOrEmpty();

        // Assert
        result.Should().BeFalse();
    }
}