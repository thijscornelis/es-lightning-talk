using ES.Framework.Domain.Events.Design;
using System.Collections;

namespace ES.Framework.Domain.Aggregates;

/// <summary>Closed collection which contains IEvent instances</summary>
internal class EventCollection : IReadOnlyCollection<IEvent>
{
	private readonly HashSet<IEvent> _events = new();

	/// <summary>Initializes a new instance of the <see cref="EventCollection" /> class.</summary>
	/// <param name="events">The events.</param>
	public EventCollection(params IEvent[] events) {
		foreach(var @event in events)
			_events.Add(@event);
	}

	/// <summary>Gets the number of elements in the collection.</summary>
	/// <value>The count.</value>
	public int Count => _events.Count;

	/// <summary>Gets a value indicating whether this instance has value.</summary>
	/// <value><c>true</c> if this instance has value; otherwise, <c>false</c>.</value>
	public bool HasValue => _events.Count > 0;

	/// <summary>Adds the specified event.</summary>
	/// <typeparam name="TEvent"></typeparam>
	/// <param name="event">The event.</param>
	/// <returns>EventCollection.</returns>
	public EventCollection Add<TEvent>(TEvent @event) where TEvent : IEvent {
		_events.Add(@event);
		return this;
	}

	/// <summary>Clears this instance.</summary>
	public void Clear() => _events.Clear();

	/// <summary>Gets the enumerator.</summary>
	/// <returns>IEnumerator&lt;Event&gt;.</returns>
	public IEnumerator<IEvent> GetEnumerator() => _events.GetEnumerator();

	/// <summary>Returns an enumerator that iterates through a collection.</summary>
	/// <returns>An <see cref="T:System.Collections.IEnumerator" /> object that can be used to iterate through the collection.</returns>
	IEnumerator IEnumerable.GetEnumerator() => _events.GetEnumerator();
}