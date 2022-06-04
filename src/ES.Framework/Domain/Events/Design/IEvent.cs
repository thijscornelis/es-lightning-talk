﻿namespace ES.Framework.Domain.Events.Design;

/// <summary>IEvent</summary>
public interface IEvent
{
	 /// <summary>Gets the event identifier.</summary>
	 /// <value>The event identifier.</value>
	 public EventId EventId => EventId.CreateNew();

	 /// <summary>Gets the occurred on.</summary>
	 /// <value>The occurred on.</value>
	 public DateTime OccurredOn => DateTime.UtcNow;
}
