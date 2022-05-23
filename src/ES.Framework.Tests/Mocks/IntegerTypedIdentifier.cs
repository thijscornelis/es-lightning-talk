using ES.Framework.Domain.TypedIdentifiers;

namespace ES.Framework.Tests.Mocks;

public record IntegerTypedIdentifier : TypedIdentifier<IntegerTypedIdentifier, int>
{
	 /// <inheritdoc />
	 public IntegerTypedIdentifier(int value) : base(value) {
	 }
}