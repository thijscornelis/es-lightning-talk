using ES.Framework.Domain.TypedIdentifiers;

namespace ES.Framework.Tests;

public record StringTypedIdentifier : TypedIdentifier<StringTypedIdentifier, string>
{
	/// <inheritdoc />
	public StringTypedIdentifier(string value) : base(value) {
	}
}

public record TypedIdentifierDto<TIdentifier>(TIdentifier Id);