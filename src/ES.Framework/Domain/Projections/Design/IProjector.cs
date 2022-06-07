using ES.Framework.Domain.Events.Design;

namespace ES.Framework.Domain.Projections.Design;

/// <summary>
/// Design Projector that will allow to create/update/delete projections as a reaction to events being written to cosmos db.
/// </summary>
public interface IProjector
{
	 /// <summary>
	 /// Determines whether this instance can handle an event of type TEvent.
	 /// </summary>
	 /// <typeparam name="TEvent">The type of the event.</typeparam>
	 /// <returns><c>true</c> if this instance can handle; otherwise, <c>false</c>.</returns>
	 bool CanHandle<TEvent>(TEvent @event)
		where TEvent : IEvent;

	 /// <summary>
	 /// Handles the specified event.
	 /// </summary>
	 /// <typeparam name="TEvent">The type of the t event.</typeparam>
	 /// <param name="event">The event.</param>
	 /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
	 /// <returns>Task.</returns>
	 Task Handle<TEvent>(TEvent @event, CancellationToken cancellationToken)
		 where TEvent : IEvent;
}