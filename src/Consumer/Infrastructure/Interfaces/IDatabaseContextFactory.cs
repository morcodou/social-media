namespace Post.Query.Infrastructure.Interfaces;

public interface IDatabaseContextFactory
{
    IDatabaseContext Create();
}