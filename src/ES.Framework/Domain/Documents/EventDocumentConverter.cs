using ES.Framework.Domain.Abstractions;
using ES.Framework.Domain.Aggregates.Design;
using ES.Framework.Domain.Documents.Design;
using ES.Framework.Domain.Events;
using ES.Framework.Domain.Events.Design;
using ES.Framework.Domain.Exceptions;
using ES.Framework.Domain.TypedIdentifiers.Design;
using ES.Framework.Infrastructure.Json;
using System.Net.NetworkInformation;

namespace ES.Framework.Domain.Documents;

/// <inheritdoc />
public class EventDocumentConverter : IEventDocumentConverter

{
	 private readonly IDateTimeProvider _dateTimeProvider;
	 private readonly IPartitionKeyResolver _partitionKeyResolver;

	 /// <summary>Initializes a new instance of the <see cref="EventDocumentConverter" /> class.</summary>
	 /// <param name="dateTimeProvider">The date time provider.</param>
	 /// <param name="partitionKeyResolver">The partition key resolver.</param>
	 public EventDocumentConverter(IDateTimeProvider dateTimeProvider, IPartitionKeyResolver partitionKeyResolver) {
		  _dateTimeProvider = dateTimeProvider;
		  _partitionKeyResolver = partitionKeyResolver;
	 }

	 private static object DeserializePayload(EventDocument document)
	 {
		  var jsonElement = (System.Text.Json.JsonElement) document.Payload;
		  var json = jsonElement.GetRawText();
		  var type = Type.GetType(document.EventType) ?? throw new EventTypeNotFoundException($"Type {document.EventType} could not be found in assemblies");
		  return json.Deserialize(type);
	 }

	 /// <inheritdoc />
	 public IEvent ToEvent(EventDocument document) => DeserializePayload(document) as IEvent;

	 /// <inheritdoc />
	 public AggregateEvent<TKey> ToAggregateEvent<TKey, TValue>(EventDocument document) where TKey : ITypedIdentifier<TValue> => DeserializePayload(document) as AggregateEvent<TKey>;

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

	 private EventDocument Create<TAggregate, TKey, TState, TValue>(IAggregateEvent<TKey> @event)
		 where TAggregate : IAggregate<TKey, TState>
		 where TKey : ITypedIdentifier<TValue>
		 where TState : class, IAggregateState<TKey>, new()
		 => new() {
		  Id = DocumentId.CreateNew(),
		  PartitionKey = _partitionKeyResolver.CreateSyntheticPartitionKey<TAggregate, TKey, TState, TValue>(@event.Id),
		  Timestamp = _dateTimeProvider.Now(),
		  Payload = @event,
		  AggregateId = @event.Id.Value,
		  AggregateType = typeof(TAggregate).FullName,
		  EventType = @event.GetType().AssemblyQualifiedName,
		  AggregateVersion = @event.Version
	 };
}
