using ES.Framework.Domain.Aggregates;
using ES.Framework.Domain.Aggregates.Design;
using ES.Framework.Domain.TypedIdentifiers.Design;

namespace ES.Sample.Fixtures;

public abstract class AggregateFixtureBase<TAggregate, TKey, TState> : FixtureBase
	where TAggregate : Aggregate<TKey, TState>
	where TKey : ITypedIdentifier
	where TState : class, IAggregateState<TKey>, new()
{
	 /// <summary>Gets or sets the aggregate.</summary>
	 /// <value>The aggregate.</value>
	 public TAggregate Aggregate { get; protected set; }

	 protected abstract void Act();

	 /// <inheritdoc />
	 protected override void Arrange() {
		  base.Arrange();
		  Aggregate = ArrangeAggregate();
		  Aggregate?.ClearUncommittedEvents();
	 }

	 protected virtual TAggregate ArrangeAggregate() => Activator.CreateInstance<TAggregate>();

	 /// <inheritdoc />
	 protected override Task InternalActAsync() {
		  Act();
		  return Task.CompletedTask;
	 }
}
