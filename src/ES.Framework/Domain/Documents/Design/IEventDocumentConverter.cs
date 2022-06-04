using ES.Framework.Domain.Aggregates.Design;
using ES.Framework.Domain.Events.Design;
using ES.Framework.Domain.TypedIdentifiers.Design;

namespace ES.Framework.Domain.Documents.Design;

/// <summary>Factory to create <see cref="EventDocument" /> for one or more <see cref="IEvent" /></summary>
public interface IEventDocumentConverter<in TAggregate, TKey, in TState, TValue>
	where TAggregate : IAggregate<TKey, TState>
	where TKey : ITypedIdentifier<TValue>
	where TState : class, IAggregateState<TKey>, new()
{
	 /// <summary>Converts to event.</summary>
	 /// <param name="document">The document.</param>
	 /// <returns>IReadOnlyCollection&lt;IAggregateEvent&lt;TKey&gt;&gt;.</returns>
	 public IAggregateEvent<TKey> ToEvent(EventDocument document);

	 /// <summary>Creates an <see cref="EventDocument" /> for the each of the specified <see cref="IEvent" />.</summary>
	 /// <param name="events">The events.</param>
	 /// <returns><see cref="IReadOnlyCollection{EventDocument}" /></returns>
	 public IReadOnlyCollection<EventDocument> ToEventDocuments(IReadOnlyCollection<IAggregateEvent<TKey>> events);
}
