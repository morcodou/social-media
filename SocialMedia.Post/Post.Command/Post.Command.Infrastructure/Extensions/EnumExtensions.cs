using CQRS.Core.Enums;

namespace Confluent.Kafka
{
    public static class EnumExtensions
    {
        private static readonly Lazy<Dictionary<PersistenceStatus, EventPersistenceStatus>> _statusEnums = new Lazy<Dictionary<PersistenceStatus, EventPersistenceStatus>>(() => new()
        {
            { PersistenceStatus.NotPersisted, EventPersistenceStatus.NotPersisted },
            { PersistenceStatus.Persisted, EventPersistenceStatus.Persisted },
            { PersistenceStatus.PossiblyPersisted, EventPersistenceStatus.PossiblyPersisted },
        });

        public static EventPersistenceStatus ToEventPersistenceStatus(this PersistenceStatus status) =>
            _statusEnums.Value[status];
    }
}