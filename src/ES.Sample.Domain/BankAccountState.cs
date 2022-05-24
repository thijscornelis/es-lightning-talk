using ES.Framework.Domain.Aggregates.Design;

namespace ES.Sample.Domain;

/// <summary>
/// State bag for <see cref="BankAccount"/>.
/// </summary>
public record BankAccountState : IAggregateState<BankAccountId>
{
	/// <inheritdoc />
	public BankAccountId Id { get; init; }

	/// <summary>
	/// Gets the name.
	/// </summary>
	/// <value>The name.</value>
	public string Name { get; init; }
}