using ES.Framework.Domain.Abstractions;
using ES.Framework.Domain.Aggregates.Design;
using ES.Framework.Domain.Documents;
using ES.Framework.Domain.Documents.Design;
using ES.Framework.Domain.TypedIdentifiers.Design;
using Moq;

namespace ES.Sample.Fixtures;

public abstract class EventDocumentFactoryFixture<TAggregate, TKey, TState, TValue> : ES.Sample.Fixtures.FixtureBase
	where TAggregate : IAggregate<TKey, TState>
	where TKey : ITypedIdentifier<TValue>
	where TState : class, IAggregateState<TKey>, new()
{
	 public TAggregate Aggregate { get; private set; }
	 public IEventDocumentConverter<TAggregate, TKey, TState, TValue> Converter { get; private set; }
	 public Mock<IDateTimeProvider> DateTimeProviderMock { get; } = new() { CallBase = true };

	 public Mock<IPartitionKeyResolver<TAggregate, TKey, TState, TValue>> PartitionKeyResolverMock { get; } =
		 new() { CallBase = true };

	 public IReadOnlyCollection<EventDocument> Result { get; private set; }

	 protected virtual void Act() => Result = Converter.ToEventDocuments(Aggregate.UncommittedEvents);

	 /// <inheritdoc />
	 protected override void Arrange() {
		  base.Arrange();
		  Aggregate = ArrangeAggregate();
		  Converter = ArrangeFactory();
		  ArrangePartitionKeyResolver(PartitionKeyResolverMock);
		  ArrangeDateTimeProvider(DateTimeProviderMock);
	 }

	 protected abstract TAggregate ArrangeAggregate();

	 protected virtual void ArrangeDateTimeProvider(Mock<IDateTimeProvider> mock) {
	 }

	 protected virtual void ArrangePartitionKeyResolver(Mock<IPartitionKeyResolver<TAggregate, TKey, TState, TValue>> mock) {
	 }

	 /// <inheritdoc />
	 protected override Task InternalActAsync() {
		  Act();
		  return Task.CompletedTask;
	 }

	 private IEventDocumentConverter<TAggregate, TKey, TState, TValue> ArrangeFactory() => new EventDocumentConverter<TAggregate, TKey, TState, TValue>(DateTimeProviderMock.Object, PartitionKeyResolverMock.Object);
}
