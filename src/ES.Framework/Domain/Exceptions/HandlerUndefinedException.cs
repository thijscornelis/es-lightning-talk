namespace ES.Framework.Domain.Exceptions;

[Serializable]
public class HandlerUndefinedException : ArgumentNullException
{
	/// <summary>Initializes a new instance of the <see cref="HandlerUndefinedException" /> class.</summary>
	/// <param name="aggregateType">Type of the aggregate.</param>
	/// <param name="eventType">Type of the event.</param>
	public HandlerUndefinedException(Type aggregateType, Type eventType) : base($@"You MUST define a handler for {eventType.Name} in {aggregateType.Name} aggregate!") {
	}
}