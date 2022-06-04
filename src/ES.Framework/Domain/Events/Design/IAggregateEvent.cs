namespace ES.Framework.Domain.Events.Design;

public interface IAggregateEvent<TKey> : IEvent
{
	 public TKey Id { get; init; }
	 public long Version { get; set; }
}
