namespace ES.Framework.Domain.Events.Design;

public interface IEvent
{
	public EventId EventId => EventId.CreateNew();
	public DateTime OccuredOn => DateTime.UtcNow;
}
