using ES.Framework.Domain.Events.Design;
using ES.Framework.Domain.TypedIdentifiers.Design;

namespace ES.Framework.Domain.Aggregates.Design;

public interface IAggregate<out TKey, out TState>
	where TKey : ITypedIdentifier
	where TState : class, IAggregateState<TKey>, new()
{
	/// <summary>Gets a value indicating whether this instance has uncommitted events.</summary>
	public bool HasUncommittedEvents => UncommittedEvents.Any();

	/// <summary>Gets the uncommitted events.</summary>
	/// <value>The uncommitted events.</value>
	public IReadOnlyCollection<IEvent> UncommittedEvents { get; }

	/// <summary>Gets the identifier.</summary>
	/// <value>The identifier.</value>
	public TKey Id => State.Id;

	/// <summary>Gets the state.</summary>
	/// <value>The state.</value>
	public TState State { get; }

	/// <summary>Gets the version.</summary>
	/// <value>The version.</value>
	long Version { get; }

	/// <summary>Gets the snapshot frequency.</summary>
	long GetSnapshotFrequency() => State.SnapshotFrequency;
}