using ES.Framework.Domain.Aggregates.Design;
using ES.Framework.Domain.Events.Design;
using ES.Framework.Domain.Exceptions;
using ES.Framework.Domain.TypedIdentifiers;
using ES.Framework.Domain.TypedIdentifiers.Design;

namespace ES.Framework.Domain.Aggregates;

/// <summary>Base class for an Aggregate</summary>
public abstract class Aggregate<TKey, TState> : IAggregate<TKey, TState>
	where TKey : ITypedIdentifier
	where TState : class, IAggregateState<TKey>, new()
{
	 private readonly EventCollection _uncommittedEvents = new();
	 private readonly Dictionary<Type, EventHandlerAction<IEvent>> _handlers = new();

	 /// <summary>Initializes a new instance of the <see cref="Aggregate{TKey, TState}" /> class.</summary>
	 protected internal Aggregate() { }

	 /// <summary>Initializes a new instance of the <see cref="Aggregate{TKey, TState}" /> class.</summary>
	 protected internal Aggregate(TKey id) : this() => Id = id;

	 /// <inheritdoc />
	 public virtual TState State { get; protected set; } = new();

	 /// <summary>Sets the state.</summary>
	 /// <param name="state">The state.</param>
	 private void SetState(TState state) => State = state;

	 /// <inheritdoc />
	 public TKey Id { get; private set; }

	 /// <inheritdoc />
	 public IReadOnlyCollection<IEvent> UncommittedEvents => _uncommittedEvents;

	 /// <inheritdoc />
	 public long Version { get; private set; }

	 private void Play<T>(T @event) where T : IEvent {
		  if(!_handlers.TryGetValue(@event.GetType(), out var eventHandler))
				throw new HandlerUndefinedException(GetType(), @event.GetType());
		  var state = eventHandler.Invoke(@event);
		  SetState(state);
		  Version++;
	 }

	 private void Rehydrate(TKey aggregateId, long aggregateVersion, TState aggregateState) {
		  Id = aggregateId;
		  Version = aggregateVersion;
		  SetState(aggregateState);
	 }

	 protected void Apply<TEvent>(TEvent @event) where TEvent : IEvent {
		  Play(@event);
		  Record(@event);
	 }

	 /// <summary>Handles the specified handler.</summary>
	 /// <exception cref="HandlerAlreadyDefinedException"></exception>
	 protected void Handle<TEvent>(EventHandlerAction<TEvent> handler) where TEvent : IEvent {
		  if(_handlers.ContainsKey(typeof(TEvent)))
				throw new HandlerAlreadyDefinedException(typeof(TEvent));
		  _handlers[typeof(TEvent)] = e => handler((TEvent) e);
	 }

	 private void Record<TEvent>(TEvent @event) where TEvent : IEvent => _uncommittedEvents.Add(@event);
	 internal void ClearUncommittedEvents() => _uncommittedEvents.Clear();
	 protected internal IReadOnlyDictionary<Type, EventHandlerAction<IEvent>> GetEventHandlers() => _handlers;
	 /// <summary>Delegate action for an event handler.</summary>
	 /// <typeparam name="TEvent">The type of the event.</typeparam>
	 /// <param name="event">The event.</param>
	 protected internal delegate TState EventHandlerAction<in TEvent>(TEvent @event);
}