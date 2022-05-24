using ES.Framework.Domain.Aggregates.Design;
using ES.Framework.Domain.Events.Design;
using ES.Framework.Domain.Exceptions;
using ES.Framework.Domain.TypedIdentifiers.Design;
using System.Reflection;

namespace ES.Framework.Domain.Aggregates;

/// <summary>Base class for an Aggregate</summary>
public abstract class Aggregate<TKey, TState> : IAggregate<TKey, TState>
	where TKey : ITypedIdentifier
	where TState : class, IAggregateState<TKey>, new()
{
	private readonly Dictionary<Type, EventHandlerAction<IEvent>> _handlers = new();
	internal readonly EventCollection _uncommittedEvents = new();

	/// <summary>Initializes a new instance of the <see cref="Aggregate{TKey, TState}" /> class.</summary>
	protected internal Aggregate() { }

	internal IReadOnlyDictionary<Type, EventHandlerAction<IEvent>> EventHandlers => _handlers;

	/// <inheritdoc />
	public TState State { get; protected set; } = new();

	/// <inheritdoc />
	public TKey Id => State.Id;

	/// <inheritdoc />
	public IReadOnlyCollection<IEvent> UncommittedEvents => _uncommittedEvents;

	/// <inheritdoc />
	public long Version { get; private set; }

	/// <summary>Sets the state.</summary>
	/// <param name="state">The state.</param>
	private void SetState(TState state) => State = state;

	private void Play<T>(T @event) where T : IEvent {
		if(!_handlers.TryGetValue(@event.GetType(), out var eventHandler))
			throw new HandlerUndefinedException(GetType(), @event.GetType());
		var state = eventHandler.Invoke(@event);
		SetState(state);
		Version++;
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

	internal void Rehydrate(Snapshot<TKey, TState> snapshot, EventCollection eventCollection) {
		SetState(snapshot.State);
		Version = snapshot.Version;
		foreach(var @event in eventCollection)
			Play(@event);
	 }

	internal static TAggregate Rehydrate<TAggregate>(Snapshot<TKey, TState> snapshot, EventCollection eventCollection)
		where TAggregate : Aggregate<TKey, TState> {
		var aggregate = (TAggregate) Activator.CreateInstance(typeof(TAggregate) , BindingFlags.NonPublic | BindingFlags.Instance, null, null, null);
		if(aggregate == null) throw new AggregateMissingDefaultConstructor(typeof(TAggregate));
		aggregate.Rehydrate(snapshot, eventCollection);
		return aggregate;
	}

	/// <summary>Delegate action for an event handler.</summary>
	/// <typeparam name="TEvent">The type of the event.</typeparam>
	/// <param name="event">The event.</param>
	protected internal delegate TState EventHandlerAction<in TEvent>(TEvent @event);
}