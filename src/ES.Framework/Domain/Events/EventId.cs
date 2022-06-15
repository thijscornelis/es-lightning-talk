using ES.Framework.Domain.TypedIdentifiers;

namespace ES.Framework.Domain.Events;
public record EventId : TypedIdentifier<EventId, Guid>
{
	 /// <inheritdoc />
	 public EventId(Guid value) : base(value) {
	 }
}
