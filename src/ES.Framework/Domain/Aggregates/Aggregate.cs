using ES.Framework.Domain.Aggregates.Design;
using ES.Framework.Domain.Collections;
using ES.Framework.Domain.Events;
using ES.Framework.Domain.Exceptions;
using ES.Framework.Domain.TypedIdentifiers.Design;
using System.Reflection;

namespace ES.Framework.Domain.Aggregates;

/// <summary>Base class for an Aggregate</summary>
public abstract class Aggregate<TKey, TState> : IAggregate<TKey, TState>
	where TKey : ITypedIdentifier
	where TState : class, IAggregateState<TKey>, new()
{
	 internal readonly EventCollection<TKey> _uncommittedEvents = new();
	 private readonly Dictionary<Type, EventHandlerAction<AggregateEvent<TKey>>> _handlers = new();

	 /// <summary>Initializes a new instance of the <see cref="Aggregate{TKey, TState}" /> class.</summary>
	 protected internal Aggregate() { }

	 /// <summary>Delegate action for an event handler.</summary>
	 /// <typeparam name="TEvent">The type of the event.</typeparam>
	 /// <param name="event">The event.</param>
	 protected internal delegate TState EventHandlerAction<in TEvent>(TEvent @event)
		 where TEvent : AggregateEvent<TKey>;

	 /// <summary>Gets a value indicating whether this instance has uncommitted events.</summary>
	 /// <value><c>true</c> if this instance has uncommitted events; otherwise, <c>false</c>.</value>
	 public bool HasUncommittedEvents => _uncommittedEvents.Any();

	 /// <inheritdoc />
	 public TKey Id => State.Id;

	 /// <inheritdoc />
	 public TState State { get; protected set; } = new();

	 /// <inheritdoc />
	 public IReadOnlyCollection<AggregateEvent<TKey>> UncommittedEvents => _uncommittedEvents;

	 /// <inheritdoc />
	 public long Version { get; private set; }

	 internal IReadOnlyDictionary<Type, EventHandlerAction<AggregateEvent<TKey>>> EventHandlers => _handlers;

	 internal static TAggregate Rehydrate<TAggregate>(Snapshot<TKey, TState> snapshot, EventCollection<TKey> eventCollection)
			 where TAggregate : Aggregate<TKey, TState> {
		  var aggregate = (TAggregate) Activator.CreateInstance(typeof(TAggregate), BindingFlags.NonPublic | BindingFlags.Instance, null, null, null);
		  if(aggregate == null)
				throw new AggregateMissingDefaultConstructor(typeof(TAggregate));
		  aggregate.Rehydrate(snapshot, eventCollection);
		  return aggregate;
	 }

	 internal void ClearUncommittedEvents() => _uncommittedEvents.Clear();

	 internal void Rehydrate(Snapshot<TKey, TState> snapshot, EventCollection<TKey> eventCollection) {
		  SetState(snapshot.State);
		  Version = snapshot.Version;
		  foreach(var @event in eventCollection)
				Play(@event);
	 }

	 /// <summary>Applies the specified event.</summary>
	 /// <typeparam name="TEvent">The type of event.</typeparam>
	 /// <param name="event">The event.</param>
	 protected void Apply<TEvent>(TEvent @event) where TEvent : AggregateEvent<TKey> {
		  Play(@event);
		  Record(@event);
	 }

	 /// <summary>Handles the specified handler.</summary>
	 /// <exception cref="HandlerAlreadyDefinedException"></exception>
	 protected void Handle<TEvent>(EventHandlerAction<TEvent> handler) where TEvent : AggregateEvent<TKey> {
		  if(_handlers.ContainsKey(typeof(TEvent)))
				throw new HandlerAlreadyDefinedException(typeof(TEvent));
		  _handlers[typeof(TEvent)] = e => handler((TEvent) e);
	 }

	 private void Play<TEvent>(TEvent @event) where TEvent : AggregateEvent<TKey> {
		  if(!_handlers.TryGetValue(@event.GetType(), out var eventHandler))
				throw new HandlerUndefinedException(GetType(), @event.GetType());
		  var state = eventHandler.Invoke(@event);
		  SetState(state);
		  Version++;
		  @event.SetVersion(Version);
	 }

	 private void Record<TEvent>(TEvent @event) where TEvent : AggregateEvent<TKey> => _uncommittedEvents.Add(@event);

	 /// <summary>Sets the state.</summary>
	 /// <param name="state">The state.</param>
	 private void SetState(TState state) => State = state;
}
