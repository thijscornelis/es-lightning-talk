using ES.Framework.Domain.Aggregates.Design;
using ES.Sample.Domain.ValueObjects;

namespace ES.Sample.Domain.Aggregates;

/// <summary>State bag for <see cref="BankAccount" />.</summary>
public record BankAccountState : IAggregateState<BankAccountId>
{
	 /// <inheritdoc />
	 public BankAccountId Id { get; init; }

	 /// <summary>Gets the name.</summary>
	 /// <value>The name.</value>
	 public string Name { get; init; }

	 /// <summary>Gets the withdrawal limit.</summary>
	 /// <value>The withdrawal limit.</value>
	 public WithdrawalLimit WithdrawalLimit { get; init; }

	 /// <summary>Gets the balance.</summary>
	 /// <value>The balance.</value>
	 public decimal Balance { get; init; }

	 /// <summary>Gets the withdrawals.</summary>
	 /// <value>The withdrawals.</value>
	 public List<Withdrawal> RecentWithdrawals { get; init; } = new List<Withdrawal>();
}
