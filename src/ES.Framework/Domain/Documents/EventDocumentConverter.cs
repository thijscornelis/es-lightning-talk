using ES.Framework.Domain.Abstractions;
using ES.Framework.Domain.Aggregates.Design;
using ES.Framework.Domain.Documents.Design;
using ES.Framework.Domain.Events;
using ES.Framework.Domain.Events.Design;
using ES.Framework.Domain.Exceptions;
using ES.Framework.Domain.TypedIdentifiers.Design;
using ES.Framework.Infrastructure.Json;

namespace ES.Framework.Domain.Documents;

/// <inheritdoc />
public class EventDocumentConverter : IEventDocumentConverter

{
	 private readonly IAggregatePartitionKeyResolver _aggregatePartitionKeyResolver;
	 private readonly IDateTimeProvider _dateTimeProvider;

	 /// <summary>Initializes a new instance of the <see cref="EventDocumentConverter" /> class.</summary>
	 /// <param name="dateTimeProvider">The date time provider.</param>
	 /// <param name="aggregatePartitionKeyResolver">The partition key resolver.</param>
	 public EventDocumentConverter(IDateTimeProvider dateTimeProvider, IAggregatePartitionKeyResolver aggregatePartitionKeyResolver) {
		  _dateTimeProvider = dateTimeProvider;
		  _aggregatePartitionKeyResolver = aggregatePartitionKeyResolver;
	 }

	 /// <inheritdoc />
	 public AggregateEvent<TKey> ToAggregateEvent<TKey, TValue>(EventDocument document) where TKey : ITypedIdentifier<TValue> => DeserializePayload(document) as AggregateEvent<TKey>;

	 /// <inheritdoc />
	 public IEvent ToEvent(EventDocument document) => DeserializePayload(document) as IEvent;

	 /// <inheritdoc />
	 public IReadOnlyCollection<EventDocument> ToEventDocuments<TAggregate, TKey, TState, TValue>(IReadOnlyCollection<AggregateEvent<TKey>> events)
	 where TAggregate : IAggregate<TKey, TState>
	 where TKey : ITypedIdentifier<TValue>
	 where TState : class, IAggregateState<TKey>, new() {
		  ArgumentNullException.ThrowIfNull(events);
		  var result = new List<EventDocument>();
		  foreach(var @event in events)
				result.Add(Create<TAggregate, TKey, TState, TValue>(@event));

		  return result;
	 }

	 private static object DeserializePayload(EventDocument document) {
		  var jsonElement = (System.Text.Json.JsonElement) document.Payload;
		  var json = jsonElement.GetRawText();
		  var type = Type.GetType(document.PayloadType) ?? throw new EventTypeNotFoundException($"Type {document.PayloadType} could not be found in assemblies");
		  return json.Deserialize(type);
	 }

	 private EventDocument Create<TAggregate, TKey, TState, TValue>(IAggregateEvent<TKey> @event)
		 where TAggregate : IAggregate<TKey, TState>
		 where TKey : ITypedIdentifier<TValue>
		 where TState : class, IAggregateState<TKey>, new()
		 => new() {
			  Id = @event.EventId,
			  PartitionKey = _aggregatePartitionKeyResolver.CreatePartitionKey<TAggregate, TKey, TState, TValue>(@event.Id),
			  Timestamp = _dateTimeProvider.Now(),
			  Payload = @event,
			  AggregateId = @event.Id.Value,
			  AggregateType = typeof(TAggregate).FullName,
			  PayloadType = @event.GetType().AssemblyQualifiedName,
			  AggregateVersion = @event.Version
		 };
}
