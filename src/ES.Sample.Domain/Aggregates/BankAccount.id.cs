using ES.Framework.Domain.TypedIdentifiers;

namespace ES.Sample.Domain.Aggregates;

/// <summary>Typed identifier for <see cref="BankAccount" /></summary>
public record BankAccountId : TypedIdentifier<BankAccountId, Guid>
{
	 /// <inheritdoc />
	 public BankAccountId(Guid value) : base(value) {
	 }
}

/// <summary>Class StatementId.</summary>
public record StatementId : TypedIdentifier<StatementId, Guid>
{
	 /// <inheritdoc />
	 public StatementId(Guid value) : base(value) {
	 }
}
