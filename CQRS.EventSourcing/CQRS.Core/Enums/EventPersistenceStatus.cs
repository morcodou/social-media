namespace CQRS.Core.Enums
{
    public enum EventPersistenceStatus
    {
        NotPersisted = 0,
        PossiblyPersisted = 1,
        Persisted = 2
    }
}