namespace ES.Framework.Domain.Events.Design;

public interface IEvent
{
	public EventId EventId => EventId.CreateNew();
	public DateTime OccuredOn => DateTime.UtcNow;
}

public interface IAggregateEvent<TKey> : IEvent
{
	public TKey Id { get; init; }
}
