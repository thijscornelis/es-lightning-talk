using ES.Framework.Domain.TypedIdentifiers;

namespace ES.Framework.Tests;

public record GuidTypedIdentifier : TypedIdentifier<GuidTypedIdentifier, Guid>
{
	/// <inheritdoc />
	public GuidTypedIdentifier(Guid value) : base(value) {
	}
}