using ES.Framework.Domain.Events.Design;

namespace ES.Framework.Domain.Events;
/// <summary>Base event type for aggregate events.</summary>
/// <typeparam name="TKey">The type of the aggregate identifier.</typeparam>
public abstract record AggregateEvent<TKey> : IAggregateEvent<TKey>
{
	 /// <summary>Initializes a new instance of the <see cref="AggregateEvent{TKey}" /> class.</summary>
	 /// <param name="id">The identifier.</param>
	 public AggregateEvent(TKey id) => Id = id;

	 /// <inheritdoc />
	 public TKey Id { get; init; }

	 /// <inheritdoc />
	 public long Version { get; private set; }

	 internal void SetVersion(long version) => Version = version;
}
