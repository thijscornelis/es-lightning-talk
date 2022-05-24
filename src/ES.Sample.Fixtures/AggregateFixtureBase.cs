using ES.Framework.Domain.Aggregates;
using ES.Framework.Domain.Aggregates.Design;
using ES.Framework.Domain.TypedIdentifiers.Design;

namespace ES.Sample.Fixtures;

public abstract class AggregateFixtureBase<TAggregate, TKey, TState> : FixtureBase
	where TAggregate : Aggregate<TKey, TState>
	where TKey : ITypedIdentifier
	where TState : class, IAggregateState<TKey>, new()
{
	public TAggregate Aggregate { get; protected set; }

	/// <inheritdoc />
	protected override void Arrange() {
		base.Arrange();
		Aggregate = ArrangeAggregate();
		Aggregate?.ClearUncommittedEvents();
	}

	/// <inheritdoc />
	protected override Task InternalActAsync() {
		Act();
		return Task.CompletedTask;
	}

	protected abstract void Act();

	protected virtual TAggregate ArrangeAggregate() => Activator.CreateInstance<TAggregate>();
}