using ES.Framework.Domain.Events.Design;
using ES.Framework.Domain.Projections.Design;
using ES.Framework.Domain.Repositories.Design;

namespace ES.Framework.Domain.Projections;

/// <inheritdoc />
public abstract class Projector<TProjection> : IProjector
	where TProjection : new()
{
	 private readonly Dictionary<Type, EventHandlerFunc<IEvent>> _handlers = new();
	 private readonly IProjectionRepository _repository;
	 private readonly Dictionary<Type, ProjectionIdResolverFunc<IEvent>> _resolvers = new();

	 /// <summary>Initializes a new instance of the <see cref="Projector{TProjection}" /> class.</summary>
	 /// <param name="repository">The repository.</param>
	 protected Projector(IProjectionRepository repository) => _repository = repository;

	 /// <summary>Delegate func for an event handler.</summary>
	 /// <typeparam name="TEvent">The type of the event.</typeparam>
	 /// <param name="event">The event.</param>
	 /// <param name="projection">The projection.</param>
	 protected delegate TProjection EventHandlerFunc<in TEvent>(TEvent @event, TProjection projection)
		 where TEvent : IEvent;

	 /// <summary>Delegate func for a ProjectionId resolver</summary>
	 /// <typeparam name="TEvent">The type of the event.</typeparam>
	 /// <param name="event">The event.</param>
	 /// <returns>ProjectionId.</returns>
	 protected delegate ProjectionId ProjectionIdResolverFunc<in TEvent>(TEvent @event)
		 where TEvent : IEvent;

	 /// <inheritdoc />
	 public bool CanHandle<TEvent>(TEvent @event) where TEvent : IEvent => _handlers.ContainsKey(@event.GetType());

	 /// <inheritdoc />
	 public async Task Handle<TEvent>(TEvent @event, CancellationToken cancellationToken) where TEvent : IEvent {
		  var projectionId = _resolvers[@event.GetType()].Invoke(@event);
		  var projection = _repository.Find<TProjection>(projectionId) ?? new();
		  projection = _handlers[@event.GetType()].Invoke(@event, projection);

		  if(projection == null) {
				await _repository.DeleteAsync<TProjection>(projectionId, cancellationToken);
		  }
		  else {
				await _repository.SaveAsync(projectionId, projection, cancellationToken);
		  }
	 }

	 /// <summary>Registration method for the specified handler.</summary>
	 /// <typeparam name="TEvent">The type of the event.</typeparam>
	 /// <param name="handler">The handler.</param>
	 /// <param name="projectionIdResolver">The projection resolver.</param>
	 /// <exception cref="System.ArgumentNullException">handler - Projector can not handle event without handler</exception>
	 /// <exception cref="System.ArgumentNullException">projectionResolver - Projector can not retrieve projections without projection resolver</exception>
	 protected void Handle<TEvent>(EventHandlerFunc<TEvent> handler, ProjectionIdResolverFunc<TEvent> projectionIdResolver) where TEvent : IEvent {
		  if(handler == null)
				throw new ArgumentNullException(nameof(handler), @"Projector can not handle event without handler");
		  if(projectionIdResolver == null)
				throw new ArgumentNullException(nameof(projectionIdResolver), @"Projector can not retrieve projections without projection resolver");

		  _handlers.Add(typeof(TEvent), (@event, projection) => handler((TEvent) @event, projection));
		  _resolvers.Add(typeof(TEvent), @event => projectionIdResolver((TEvent) @event));
	 }
}
