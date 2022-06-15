using ES.Framework.Domain.TypedIdentifiers;

namespace ES.Framework.Tests.Mocks;

public record GuidTypedIdentifier : TypedIdentifier<GuidTypedIdentifier, Guid>
{
	 /// <inheritdoc />
	 public GuidTypedIdentifier(Guid value) : base(value) {
	 }
}