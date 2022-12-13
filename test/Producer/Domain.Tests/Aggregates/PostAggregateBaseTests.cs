namespace Post.Command.Domain.Aggregates;

public abstract class PostAggregateBaseTests
{
    protected readonly PostAggregate _sut;
    protected readonly Guid _id;
    protected readonly string _author;
    protected readonly string _message;
    protected readonly Fixture _fixture;

    protected PostAggregateBaseTests()
    {
        _fixture = new();
        _id = _fixture.Create<Guid>();
        _author = _fixture.Create<string>();
        _message = _fixture.Create<string>();

        _sut = new(_id, _author, _message);
    }
}