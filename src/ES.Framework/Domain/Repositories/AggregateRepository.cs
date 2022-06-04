using ES.Framework.Domain.Abstractions;
using ES.Framework.Domain.Aggregates;
using ES.Framework.Domain.Aggregates.Design;
using ES.Framework.Domain.Collections;
using ES.Framework.Domain.Documents;
using ES.Framework.Domain.Documents.Design;
using ES.Framework.Domain.Exceptions;
using ES.Framework.Domain.Repositories.Design;
using ES.Framework.Domain.TypedIdentifiers.Design;

namespace ES.Framework.Domain.Repositories;

public class AggregateRepository<TAggregate, TKey, TState, TValue> : IAggregateRepository<TAggregate, TKey, TState, TValue>
	where TState : class, IAggregateState<TKey>, new()
	where TKey : ITypedIdentifier<TValue>
	where TAggregate : Aggregate<TKey, TState>
{
	 private readonly IDocumentRepository _documentRepository;
	 private readonly IEventDocumentConverter<TAggregate, TKey, TState, TValue> _eventDocumentConverter;
	 private readonly IPartitionKeyResolver<TAggregate, TKey, TState, TValue> _partitionKeyResolver;

	 /// <summary>Initializes a new instance of the <see cref="AggregateRepository{TAggregate, TKey, TState, TValue}" /> class.</summary>
	 /// <param name="documentRepository">The document repository.</param>
	 /// <param name="eventDocumentConverter">The event document converter.</param>
	 /// <param name="partitionKeyResolver">The partition key resolver.</param>
	 public AggregateRepository(IDocumentRepository documentRepository, IEventDocumentConverter<TAggregate, TKey, TState, TValue> eventDocumentConverter, IPartitionKeyResolver<TAggregate, TKey, TState, TValue> partitionKeyResolver) {
		  _documentRepository = documentRepository;
		  _eventDocumentConverter = eventDocumentConverter;
		  _partitionKeyResolver = partitionKeyResolver;
	 }

	 /// <inheritdoc />
	 public async Task<TAggregate> FindAsync(TKey id, CancellationToken cancellationToken) {
		  var partitionKey = _partitionKeyResolver.CreateSyntheticPartitionKey(id);
		  var enumerable = _documentRepository.GetAsyncEnumerable<EventDocument>(partitionKey, cancellationToken: cancellationToken);

		  var events = new EventCollection<TKey>();
		  await foreach(var document in enumerable.WithCancellation(cancellationToken)) {
				events.Add(_eventDocumentConverter.ToEvent(document));
		  }

		  if(!events.Any())
				return default;

		  var aggregate = Activator.CreateInstance<TAggregate>();
		  aggregate.Rehydrate(null, events);
		  return aggregate;
	 }

	 /// <inheritdoc />
	 public async Task<TAggregate> GetAsync(TKey id, CancellationToken cancellationToken) => await FindAsync(id, cancellationToken) ?? throw new AggregateNotFoundException(id, "Used AggregateRepository.Get(id) but no aggregate was found with this id.");

	 /// <inheritdoc />
	 public IAsyncEnumerable<TAggregate> GetEnumerableAsync(CancellationToken cancellationToken) {
		  throw new NotImplementedException();
	 }

	 /// <inheritdoc />
	 public async Task<TAggregate> SaveAsync(TAggregate aggregate, CancellationToken cancellationToken) {
		  if(!aggregate.HasUncommittedEvents)
				return aggregate;

		  var documents = _eventDocumentConverter.ToEventDocuments(aggregate._uncommittedEvents);
		  await _documentRepository.AddAsync(documents, cancellationToken);

		  aggregate.ClearUncommittedEvents();

		  return aggregate;
	 }
}
