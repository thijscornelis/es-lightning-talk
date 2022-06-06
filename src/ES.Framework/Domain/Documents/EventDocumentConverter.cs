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
public class EventDocumentConverter<TAggregate, TKey, TState, TValue> : IEventDocumentConverter<TAggregate, TKey, TState, TValue>
	where TAggregate : IAggregate<TKey, TState>
	where TKey : ITypedIdentifier<TValue>
	where TState : class, IAggregateState<TKey>, new()
{
	 private readonly IDateTimeProvider _dateTimeProvider;
	 private readonly IPartitionKeyResolver<TAggregate, TKey, TState, TValue> _partitionKeyResolver;

	 /// <summary>Initializes a new instance of the <see cref="EventDocumentConverter{TAggregate,TKey,TState, TValue}" /> class.</summary>
	 /// <param name="dateTimeProvider">The date time provider.</param>
	 /// <param name="partitionKeyResolver">The partition key resolver.</param>
	 public EventDocumentConverter(IDateTimeProvider dateTimeProvider, IPartitionKeyResolver<TAggregate, TKey, TState, TValue> partitionKeyResolver) {
		  _dateTimeProvider = dateTimeProvider;
		  _partitionKeyResolver = partitionKeyResolver;
	 }

	 /// <inheritdoc />
	 public AggregateEvent<TKey> ToEvent(EventDocument document) {
		  var jsonElement = (System.Text.Json.JsonElement) document.Payload;
		  var json = jsonElement.GetRawText();
		  var type = Type.GetType(document.EventType) ?? throw new EventTypeNotFoundException($"Type {document.EventType} could not be found in assemblies");
		  return json.Deserialize(type) as AggregateEvent<TKey>;
	 }

	 /// <inheritdoc />
	 public IReadOnlyCollection<EventDocument> ToEventDocuments(IReadOnlyCollection<AggregateEvent<TKey>> events) {
		  ArgumentNullException.ThrowIfNull(events);
		  var result = new List<EventDocument>();
		  foreach(var @event in events)
				result.Add(Create(@event));

		  return result;
	 }

	 private EventDocument Create(IAggregateEvent<TKey> @event) => new() {
		  Id = DocumentId.CreateNew(),
		  PartitionKey = _partitionKeyResolver.CreateSyntheticPartitionKey(@event.Id),
		  Timestamp = _dateTimeProvider.Now(),
		  Payload = @event,
		  AggregateId = @event.Id.Value,
		  AggregateType = typeof(TAggregate).FullName,
		  EventType = @event.GetType().AssemblyQualifiedName,
		  AggregateVersion = @event.Version
	 };
}
