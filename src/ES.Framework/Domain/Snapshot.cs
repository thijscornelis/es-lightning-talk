using ES.Framework.Domain.Aggregates.Design;
using ES.Framework.Domain.TypedIdentifiers.Design;

namespace ES.Framework.Domain;

public class Snapshot<TKey, TState>
	where TState : class, IAggregateState<TKey>, new()
	where TKey : ITypedIdentifier
{
	 public TKey AggregateId { get; init; }
	 public SnapshotId Id { get; init; }
	 public TState State { get; init; }
	 public long Version { get; init; }
}
