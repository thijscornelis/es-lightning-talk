using ES.Framework.Domain.TypedIdentifiers.Design;

namespace ES.Framework.Domain.Aggregates.Design;

public interface IAggregateState<TKey>
where TKey : ITypedIdentifier
{
	 /// <summary>Gets the identifier.</summary>
	 /// <value>The identifier.</value>
	 public TKey Id { get; init; }

	 /// <summary>Gets the snapshot frequency.</summary>
	 /// <value>The snapshot frequency.</value>
	 public long SnapshotFrequency => 20;
}
