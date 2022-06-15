using ES.Framework.Domain.TypedIdentifiers;

namespace ES.Framework.Tests.Mocks;

public record StringTypedIdentifier : TypedIdentifier<StringTypedIdentifier, string>
{
	 /// <inheritdoc />
	 public StringTypedIdentifier(string value) : base(value) {
	 }
}