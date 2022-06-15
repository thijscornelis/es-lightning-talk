using ES.Framework.Domain.Aggregates.Design;
using ES.Framework.Domain.Events;
using ES.Framework.Domain.Events.Design;
using ES.Framework.Domain.TypedIdentifiers.Design;

namespace ES.Framework.Domain.Documents.Design;

/// <summary>Factory to create <see cref="EventDocument" /> for one or more <see cref="IEvent" /></summary>
public interface IEventDocumentConverter
{
	 /// <summary>Converts to <see cref="AggregateEvent{TKey}" />.</summary>
	 /// <typeparam name="TKey">The type of the t key.</typeparam>
	 /// <typeparam name="TValue">The type of the t value.</typeparam>
	 /// <param name="document">The document.</param>
	 /// <returns>AggregateEvent&lt;TKey&gt;.</returns>
	 public AggregateEvent<TKey> ToAggregateEvent<TKey, TValue>(EventDocument document)
		 where TKey : ITypedIdentifier<TValue>;

	 /// <summary>Converts to event.</summary>
	 /// <param name="document">The document.</param>
	 /// <returns>IReadOnlyCollection&lt;IAggregateEvent&lt;TKey&gt;&gt;.</returns>
	 public IEvent ToEvent(EventDocument document);

	 /// <summary>Creates an <see cref="EventDocument" /> for the each of the specified <see cref="IEvent" />.</summary>
	 /// <param name="events">The events.</param>
	 /// <returns><see cref="IReadOnlyCollection{EventDocument}" /></returns>
	 public IReadOnlyCollection<EventDocument> ToEventDocuments<TAggregate, TKey, TState, TValue>(IReadOnlyCollection<AggregateEvent<TKey>> events)
		 where TAggregate : IAggregate<TKey, TState>
		 where TKey : ITypedIdentifier<TValue>
		 where TState : class, IAggregateState<TKey>, new();
}
