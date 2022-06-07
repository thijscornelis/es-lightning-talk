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
	 /// <summary>Gets the aggregate.</summary>
	 /// <value>The aggregate.</value>
	 public TAggregate Aggregate { get; private set; }

	 /// <summary>Gets the converter.</summary>
	 /// <value>The converter.</value>
	 public IEventDocumentConverter Converter { get; private set; }

	 /// <summary>Gets the date time provider mock.</summary>
	 /// <value>The date time provider mock.</value>
	 public Mock<IDateTimeProvider> DateTimeProviderMock { get; } = new() { CallBase = true };

	 /// <summary>Gets the partition key resolver mock.</summary>
	 /// <value>The partition key resolver mock.</value>
	 public Mock<IPartitionKeyResolver> PartitionKeyResolverMock { get; } =
		 new() { CallBase = true };

	 /// <summary>Gets the result.</summary>
	 /// <value>The result.</value>
	 public IReadOnlyCollection<EventDocument> Result { get; private set; }

	 protected virtual void Act() => Result = Converter.ToEventDocuments<TAggregate, TKey, TState, TValue>(Aggregate.UncommittedEvents);

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

	 protected virtual void ArrangePartitionKeyResolver(Mock<IPartitionKeyResolver> mock) {
	 }

	 /// <inheritdoc />
	 protected override Task InternalActAsync() {
		  Act();
		  return Task.CompletedTask;
	 }

	 private IEventDocumentConverter ArrangeFactory() => new EventDocumentConverter(DateTimeProviderMock.Object, PartitionKeyResolverMock.Object);
}
