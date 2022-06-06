using ES.Framework.Domain.Events;
using ES.Sample.Domain.Aggregates;

namespace ES.Sample.Domain.Events;

/// <summary>Event used when money is deposited into a <see cref="BankAccount" /></summary>
public record MoneyDeposited : AggregateEvent<BankAccountId>
{
	 /// <summary>Gets the amount.</summary>
	 /// <value>The amount.</value>
	 public decimal Amount { get; init; }

	 /// <inheritdoc />
	 public MoneyDeposited(BankAccountId id) : base(id) {
	 }
}
