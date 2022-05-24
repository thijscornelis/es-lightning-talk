using ES.Framework.Domain.TypedIdentifiers;

namespace ES.Sample.Domain;

/// <summary>
/// Typed identifier for <see cref="BankAccount"/>
/// </summary>
public record BankAccountId : TypedIdentifier<BankAccountId, Guid>
{
	/// <inheritdoc />
	public BankAccountId(Guid value) : base(value) {
	}
}