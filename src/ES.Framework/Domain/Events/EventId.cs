using ES.Framework.Domain.TypedIdentifiers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ES.Framework.Domain.Events;
public record EventId : TypedIdentifier<EventId, Guid>
{
	/// <inheritdoc />
	public EventId(Guid value) : base(value)
	{
	}
}
